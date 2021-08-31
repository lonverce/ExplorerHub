using System.Threading;
using System.Threading.Tasks;

namespace ExplorerHub
{
    public interface IAppLeader
    {
        Task<string[]> ReadMessageFromFollowerAsync(CancellationToken cancellation);

        void Quit();
    }
}