using System.Collections.ObjectModel;
using ExplorerHub.ViewModels.Explorers;
using GongSolutions.Wpf.DragDrop;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class ExplorerHubViewModel : ViewModelBase, IManagedObject
    {
        private int _selectedIndex = -1;

        public ObservableCollection<ExplorerViewModel> Explorers { get; }

        [InjectProperty]
        public AddBrowserCommand AddBrowserCommand { get; set; }

        [InjectProperty]
        public CloseBrowserCommand CloseBrowserCommand { get; set; }

        [InjectProperty(ResolvedType = typeof(ExplorerHubDropTarget))]
        public IDropTarget DropTarget { get; set; }

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

        public ExplorerHubViewModel(int managedObjectId)
        {
            ManagedObjectId = managedObjectId;
            Explorers = new ObservableCollection<ExplorerViewModel>();
        }
    }
}
