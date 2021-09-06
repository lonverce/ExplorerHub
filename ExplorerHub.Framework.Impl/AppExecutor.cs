using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorerHub.Framework
{
    public class AppExecutor
    {
        private readonly IEnumerable<Func<IAppInitialization>> _appInitializations;
        private readonly ILogger<AppExecutor> _logger;
        private readonly Stack<IAppInitialization> _initializations = new Stack<IAppInitialization>();

        public AppExecutor(IEnumerable<Func<IAppInitialization>> appInitializations, ILogger<AppExecutor> logger)
        {
            _appInitializations = appInitializations;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            foreach (var factory in _appInitializations)
            {
                var initialization = factory();
                _logger.Log(LogLevel.Debug, $"正在初始化 {initialization.GetType().FullName}");
                await initialization.InitializeAppComponentsAsync();
                _initializations.Push(initialization);
            }
        }

        public async Task StopAsync()
        {
            while (_initializations.Any())
            {
                var initialization = _initializations.Pop();
                _logger.Log(LogLevel.Debug, $"正在释放 {initialization.GetType().FullName}");
                await initialization.ReleaseAppComponentAsync();
            }
        }
    }
}
