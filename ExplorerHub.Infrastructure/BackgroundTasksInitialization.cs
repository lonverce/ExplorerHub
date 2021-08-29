using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.OwnedInstances;

namespace ExplorerHub.AppInitializations
{
    /// <summary>
    /// 启动所有后台任务
    /// </summary>
    public class BackgroundTasksInitialization : IAppInitialization, IDisposable
    {
        private readonly Owned<IBackgroundTask>[] _taskCollections;
        private bool _inited;

        public BackgroundTasksInitialization(IEnumerable<Owned<IBackgroundTask>> taskFactories)
        {
            _taskCollections = taskFactories.ToArray();
        }

        public void InitializeAppComponents()
        {
            foreach (var taskOwner in _taskCollections)
            {
                taskOwner.Value.Start();
            }

            _inited = true;
        }

        public void Dispose()
        {
            if (!_inited)
            {
                return;
            }

            foreach (var taskOwner in _taskCollections.Reverse())
            {
                taskOwner.Value.Stop();
            }
        }
    }
}
