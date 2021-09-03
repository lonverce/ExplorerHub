using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Framework.WPF;

namespace ExplorerHub.ViewModels.Explorers
{
    public class AddFavoriteCommand : AsyncCommand
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

        public override bool CanExecute(object parameter) =>
            !_vm.IsCurrentNavigationInFavorite && _vm.DisplayingTarget?.IsFileSystemObject == true;

        public override async Task ExecuteAsync(object parameter)
        {
            var jpegEncoder = new PngBitmapEncoder();
            using var ms = new MemoryStream();
            jpegEncoder.Frames.Add(BitmapFrame.Create(_vm.Logo));
            jpegEncoder.Save(ms);
            
            await _favoriteApplication.AddFavoriteAsync(new AddFavoriteRequest
            {
                Name = _vm.Title,
                Url = _vm.NavigationPath,
                Icon = ms.ToArray()
            });
        }

        public override event EventHandler CanExecuteChanged;
    }
}
