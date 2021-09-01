using System.Collections.ObjectModel;
using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels.Explorers;
using ExplorerHub.ViewModels.Favorites;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class ExplorerHubViewModel : ViewModelBase, IManagedObject
    {
        private int _selectedIndex = -1;

        public ObservableCollection<ExplorerViewModel> Explorers { get; }

        public ObservableCollection<FavoriteViewModel> Favorites { get; }

        [InjectProperty]
        public AddBrowserCommand AddBrowser { get; set; }

        [InjectProperty]
        public CloseBrowserCommand CloseBrowser { get; set; }

        [InjectProperty]
        public ExplorerHubDropTarget DropTarget { get; set; }

        public int ManagedObjectId { get; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public ExplorerHubViewModel(int managedObjectId, FavoriteViewModelProvider favoriteViewModelProvider)
        {
            ManagedObjectId = managedObjectId;
            Explorers = new ObservableCollection<ExplorerViewModel>();
            Favorites = favoriteViewModelProvider.Favorites;
        }
    }
}
