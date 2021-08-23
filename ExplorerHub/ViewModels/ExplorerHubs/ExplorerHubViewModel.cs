using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
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
            Explorers.CollectionChanged += ExplorersOnCollectionChanged;
        }

        private void ExplorersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var explorers = Explorers.ToArray();

            if (!explorers.Any())
            {
                return;
            }

            if (explorers.Length == 1)
            {
                explorers.Single().Position = ItemPositionType.Both;
            }
            else
            {
                explorers.First().Position = ItemPositionType.Head;
                foreach (var explorer in explorers.Skip(1).Take(explorers.Length-2))
                {
                    explorer.Position = ItemPositionType.None;
                }

                explorers.Last().Position = ItemPositionType.Tail;
            }
        }
    }
}
