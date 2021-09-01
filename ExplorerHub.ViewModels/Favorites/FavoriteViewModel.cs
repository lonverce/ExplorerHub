using System;
using System.IO;
using System.Windows.Media.Imaging;
using Autofac.Features.OwnedInstances;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Framework;

namespace ExplorerHub.ViewModels.Favorites
{
    public class FavoriteViewModel
    {
        public Guid Id { get; }

        public string Name { get; }

        public string LocationUrl { get; }

        public BitmapSource Logo { get; }

        [InjectProperty]
        public OpenFavoriteLinkCommand OpenFavoriteLink { get; set; }

        [InjectProperty]
        public RemoveFavoriteLinkCommand RemoveFavoriteLink { get; set; }
        
        public delegate Owned<FavoriteViewModel> ConstructFunc(FavoriteDto data);

        public FavoriteViewModel(FavoriteDto data)
        {
            Id = data.Id;
            Name = data.Name;
            LocationUrl = data.Url;
            
            using var logoStream = new MemoryStream(data.Icon, false);
            var decoder = new PngBitmapDecoder(logoStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
            var frame = decoder.Frames[0];
            Logo = frame;
        }
    }
}