﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ExplorerHub.Applications.Favorites;

namespace ExplorerHub.ViewModels.Explorers
{
    public class AddFavoriteCommand : ICommand
    {
        private readonly ExplorerViewModel _vm;
        private readonly IFavoriteApplication _favoriteApplication;

        public AddFavoriteCommand(ExplorerViewModel vm, IFavoriteApplication favoriteApplication)
        {
            _vm = vm;
            _favoriteApplication = favoriteApplication;
            vm.PropertyChanged += VmOnPropertyChanged;
        }

        private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_vm.IsCurrentNavigationInFavorite) ||
                e.PropertyName == nameof(_vm.DisplayingTarget))
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter) =>
            !_vm.IsCurrentNavigationInFavorite && _vm.DisplayingTarget?.IsFileSystemObject == true;

        public virtual void Execute(object parameter)
        {
            var jpegEncoder = new JpegBitmapEncoder();
            using var ms = new MemoryStream();
            jpegEncoder.Frames.Add(BitmapFrame.Create(_vm.Logo));
            jpegEncoder.Save(ms);
            
            _favoriteApplication.AddFavorite(new AddFavoriteRequest
            {
                Name = _vm.Title,
                Url = _vm.NavigationPath,
                Icon = ms.ToArray()
            });
        }

        public event EventHandler CanExecuteChanged;
    }
}
