using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExplorerHub.Infrastructures
{
    public class AppElector
    {
        public const string PipeName = "ExplorerHub-E8DDAEB3-CBEE-4DA8-BF41-95AAD0D769BC";

        public bool TryRunForLeader(out IAppLeader leader)
        {
            try
            {
                var server = new NamedPipeServerStream(PipeName,
                        PipeDirection.In,
                        1, PipeTransmissionMode.Message);
                leader = new AppLeader(server);
                return true;
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
                leader = null;
                return false;
            }
        }

        public void SendMessageToLeader(string[] msg)
        {
            using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
            client.Connect(1000);
            using var writer = new StreamWriter(client, Encoding.UTF8, 4096);
            writer.Write(JsonConvert.SerializeObject(msg, Formatting.None));
            client.WaitForPipeDrain();
        }

        class AppLeader : IAppLeader
        {
            private readonly NamedPipeServerStream _server;

            public AppLeader(NamedPipeServerStream server)
            {
                _server = server;
            }

            public async Task<string[]> ReadMessageFromFollowerAsync(CancellationToken cancellation)
            {
                using var reader = new StreamReader(_server, Encoding.UTF8, 
                    false, 4096, true);

                try
                {
                    await _server.WaitForConnectionAsync(cancellation);
                    var data = await reader.ReadLineAsync();
                    return JsonConvert.DeserializeObject<string[]>(data);
                }
                finally
                {
                    _server.Disconnect();
                }
            }

            public void Quit()
            {
                _server.Dispose();
            }
        }
    }
}
