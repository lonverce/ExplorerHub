using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExplorerHub.Framework.BackgroundTasks
{
    /// <summary>
    /// 监听由Follower进程发送的启动请求，并推送事件<see cref="FollowerStartupEventData"/>
    /// </summary>
    internal class FollowerProcessWatchingTask : IBackgroundTask
    {
        private readonly IAppLeader _leader;
        private readonly IEventBus _eventBus;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Task _bgTask = Task.CompletedTask;

        public FollowerProcessWatchingTask(
            IAppLeader leader, 
            IEventBus eventBus)
        {
            _leader = leader;
            _eventBus = eventBus;
        }

        public async Task StartAsync()
        {
            _bgTask = Task.Run(ReadThread);
            await Task.CompletedTask;
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
        }

        public async Task StopAsync()
        {
            _tokenSource.Cancel();
            await _bgTask;
            _leader.Quit();
        }
    }
}
