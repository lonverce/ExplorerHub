using System;

namespace ExplorerHub.Framework.WPF
{
    public interface ICommandExceptionHandler
    {
        void HandleException(Exception e);
    }
}
