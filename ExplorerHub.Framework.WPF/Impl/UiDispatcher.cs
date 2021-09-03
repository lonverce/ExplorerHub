using System;
using System.Threading.Tasks;
using System.Windows;

namespace ExplorerHub.Framework.WPF.Impl
{
    internal sealed class UiDispatcher : IUiDispatcher
    {
        private readonly Application _app;

        public UiDispatcher(Application app)
        {
            _app = app;
        }

        public async Task InvokeAsync(Action action)
        {
            await _app.Dispatcher.InvokeAsync(action);
        }

        public async Task InvokeAsync(Func<Task> asyncFunc)
        {
            await _app.Dispatcher.InvokeAsync(asyncFunc);
        }
    }
}
