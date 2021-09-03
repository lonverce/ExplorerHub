using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels.Favorites;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.Explorers
{
    public class ExplorerViewModel : ViewModelBase,IManagedObject, IDisposable
    {
        #region Fields

        private string _title;
        private string _navigationPath;
        private ShellObject _displayingTarget;
        private int _ownerId = -1;
        private readonly FavoriteViewModelProvider _favoriteViewModelProvider;
        private readonly IEventBus _eventBus;
        private bool _isCurrentNavigationInFavorite;

        #endregion

        #region Properties

        public int ManagedObjectId { get; }

        public ExplorerBrowser Browser { get; }

        public string NavigationPath
        {
            get => _navigationPath;
            private set
            {
                _navigationPath = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            private set
            {
                _title = value;
                OnPropertyChanged();
            }
        } 

        public BitmapSource Logo { get; private set; }

        public ShellObject DisplayingTarget
        {
            get => _displayingTarget;
            private set
            {
                _displayingTarget = value ?? throw new ArgumentNullException();
                OnPropertyChanged();
                OnNavigationUpdated();
            }
        }
        
        public int OwnerId
        {
            get => _ownerId;
            set
            {
                _ownerId = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentNavigationInFavorite
        {
            get => _isCurrentNavigationInFavorite;
            private set
            {
                _isCurrentNavigationInFavorite = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        [InjectProperty]
        public SearchCommand Search { get; set; }

        [InjectProperty]
        public NavBackCommand NavigateBackward { get; set; }

        [InjectProperty]
        public NavForwardCommand NavigateForward { get; set; }

        [InjectProperty]
        public GoToParentCommand GoToParent { get; set; }
        
        [InjectProperty]
        public ShowInNewWindowCommand ShowInNewWindow { get; set; }

        [InjectProperty]
        public CloseExplorerCommand CloseExplorer { get; set; }

        [InjectProperty]
        public AddFavoriteCommand AddToFavorite { get; set; }

        [InjectProperty]
        public RemoveFavoriteCommand RemoveFavorite { get; set; }
        
        #endregion

        #region Constructor

        public ExplorerViewModel(
            int managedObjectId,
            ShellObject initialTarget,
            FavoriteViewModelProvider favoriteViewModelProvider,
            IEventBus eventBus)
        {
            _favoriteViewModelProvider = favoriteViewModelProvider;
            _eventBus = eventBus;
            ManagedObjectId = managedObjectId;

            Browser = new ExplorerBrowser
            {
                BorderStyle = BorderStyle.None
            };
            var log = Browser.NavigationLog;
            log.NavigationLogChanged += NavigationLogOnNavigationLogChanged;

            if (initialTarget != null)
            {
                Browser.Navigate(initialTarget);
            }

            favoriteViewModelProvider.Favorites.CollectionChanged += FavoritesOnCollectionChanged;
            CheckFavorite();
        }
        
        public ExplorerViewModel(int managedObjectId, FavoriteViewModelProvider favoriteViewModelProvider, IEventBus eventBus)
            :this(managedObjectId, null, favoriteViewModelProvider, eventBus)
        {
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            var log = Browser.NavigationLog;
            log.NavigationLogChanged -= NavigationLogOnNavigationLogChanged;
        }
        
        #endregion

        #region Private methods

        private void CheckFavorite()
        {
            if (_displayingTarget == null)
            {
                IsCurrentNavigationInFavorite = false;
            }
            else if(!_displayingTarget.IsFileSystemObject)
            {
                IsCurrentNavigationInFavorite = false;
            }
            else
            {
                IsCurrentNavigationInFavorite =
                    _favoriteViewModelProvider.Favorites.Any(model => string.Equals(model.LocationUrl, NavigationPath));
            }
        }

        private void FavoritesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CheckFavorite();
        }

        private void NavigationLogOnNavigationLogChanged(object sender, NavigationLogEventArgs e)
        {
            var log = (ExplorerBrowserNavigationLog)sender;
            DisplayingTarget = log.CurrentLocation;
            _eventBus.PublishEventAsync(new ExplorerNavigationUpdatedEventData(ManagedObjectId));
        }

        private void OnNavigationUpdated()
        {
            var target = DisplayingTarget;

            if (target == null)
            {
                return;
            }

            NavigationPath = target.IsFileSystemObject ? target.ParsingName : target.ToString();
            Title = target.Name;
            Logo = target.Thumbnail.SmallBitmapSource;
            OnPropertyChanged(nameof(Logo));
            CheckFavorite();
        }

        public void FlushData()
        {
            OnNavigationUpdated();
        }

        #endregion
    }
}