using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.Explorers
{
    public class SearchCommand : ICommand
    {
        private readonly ExplorerViewModel _owner;
        private readonly IShellUrlParser _folderManager;

        public SearchCommand(ExplorerViewModel owner, IShellUrlParser folderManager)
        {
            _owner = owner;
            _folderManager = folderManager;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var address = (string) parameter;
            Execute(address:address);
        }

        public void Execute(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                _owner.FlushData();
                return;
            }

            if (Path.IsPathRooted(address))
            {
                try
                {
                    _owner.Browser.Navigate(ShellObject.FromParsingName(address));
                }
                catch (ShellException e)
                {
                    if (e.InnerException is FileNotFoundException)
                    {
                        MessageBox.Show("无法定位到指定路径", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show(e.Message, "未知错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    _owner.FlushData();
                }
            }
            else if (_folderManager.KnownFolders.TryGetValue(address, out var folder))
            {
                _owner.Browser.Navigate(folder.First());
            }
            else
            {
                MessageBox.Show("错误路径");
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}