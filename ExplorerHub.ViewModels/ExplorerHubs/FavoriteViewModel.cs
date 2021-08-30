using System;
using System.IO;
using System.Windows.Media.Imaging;
using ExplorerHub.Applications.Favorites;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class FavoriteViewModel
    {
        public Guid Id { get; }

        public string Name { get; }

        public string LocationUrl { get; }

        public BitmapSource Logo { get; }

        public FavoriteViewModel(FavoriteDto data)
        {
            Id = data.Id;
            Name = data.Name;
            LocationUrl = data.Url;
            
            using var logoStream = new MemoryStream(data.Icon, false);
            var decoder = new JpegBitmapDecoder(logoStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            var frame = decoder.Frames[0];
            Logo = frame;
        }
    }
}