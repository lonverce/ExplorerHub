using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Start()
        {
            foreach (var backgroundTaskFactory in _backgroundTaskFactories)
            {
                var taskOwner = backgroundTaskFactory();
                taskOwner.Value.Start();
                _backgroundTasks.Push(taskOwner);
            }
        }

        public void Stop()
        {
            while (_backgroundTasks.Any())
            {
                using var taskOwner = _backgroundTasks.Pop();
                taskOwner.Value.Stop();
            }
        }
    }
}
