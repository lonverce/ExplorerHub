using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;

namespace ExplorerHub.Framework
{
    internal sealed class BackgroundTaskManager
    {
        private readonly IEnumerable<Func<Owned<IBackgroundTask>>> _backgroundTaskFactories;
        private readonly Stack<Owned<IBackgroundTask>> _backgroundTasks;

        public BackgroundTaskManager(IEnumerable<Func<Owned<IBackgroundTask>>> backgroundTaskFactories)
        {
            _backgroundTaskFactories = backgroundTaskFactories;
            _backgroundTasks = new Stack<Owned<IBackgroundTask>>();
        }

        public async Task StartAsync()
        {
            foreach (var backgroundTaskFactory in _backgroundTaskFactories)
            {
                var taskOwner = backgroundTaskFactory();
                await taskOwner.Value.StartAsync();
                _backgroundTasks.Push(taskOwner);
            }
        }

        public async Task StopAsync()
        {
            while (_backgroundTasks.Any())
            {
                await using var taskOwner = _backgroundTasks.Pop();
                await taskOwner.Value.StopAsync();
            }
        }
    }
}
