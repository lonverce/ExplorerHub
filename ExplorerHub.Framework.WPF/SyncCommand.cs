using System;
using System.Windows.Input;

namespace ExplorerHub.Framework.WPF
{
    public abstract class SyncCommand : ICommand
    {
        public virtual bool CanExecute(object parameter) => true;

        [UserEntry]
        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }
    }

    /// <summary>
    /// 指示此方法是用户操作入口, 底层框架应注意捕获异常
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UserEntryAttribute : Attribute
    {
    }
}