using System;
using System.Windows.Input;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    class AddFavoriteCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            
        }

        public event EventHandler CanExecuteChanged;
    }
}
