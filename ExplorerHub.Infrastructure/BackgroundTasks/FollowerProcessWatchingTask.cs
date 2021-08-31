using System;
using System.Threading;
using System.Threading.Tasks;
using ExplorerHub.Events;

namespace ExplorerHub.Infrastructure.BackgroundTasks
{
    /// <summary>
    /// 监听由Follower进程发送的启动请求，并推送事件<see cref="FollowerStartupEventData"/>
    /// </summary>
    public class FollowerProcessWatchingTask : IBackgroundTask
    {
        private readonly IAppLeader _leader;
        private readonly IEventBus _eventBus;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _evt = new ManualResetEvent(false);

        public FollowerProcessWatchingTask(
            IAppLeader leader, 
            IEventBus eventBus)
        {
            _leader = leader;
            _eventBus = eventBus;
        }

        public void Start()
        {
            Task.Run(ReadThread);
        }

        private async Task ReadThread()
        {
            try
            {
                while (!_tokenSource.IsCancellationRequested)
                {
                    var msg = await _leader.ReadMessageFromFollowerAsync(_tokenSource.Token);
                    _eventBus.PublishEvent(new FollowerStartupEventData(msg));
                }
            }
            catch (OperationCanceledException e)when (e.CancellationToken == _tokenSource.Token)
            {
                return;
            }
            finally
            {
                _evt.Set();
            }
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            _evt.WaitOne(TimeSpan.FromSeconds(3));
            _leader.Quit();
            _evt.Dispose();
        }
    }
}
