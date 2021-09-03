using System;
using System.Threading.Tasks;

namespace ExplorerHub.Framework
{
    public interface IUiDispatcher
    {
        Task InvokeAsync(Action action);

        Task InvokeAsync(Func<Task> asyncFunc);
    }
}