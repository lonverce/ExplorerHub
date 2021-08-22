using System;
using System.Windows.Media.Imaging;
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

        private ShellObject DisplayingTarget
        {
            get => _displayingTarget;
            set
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

        #endregion

        #region Commands

        [InjectProperty]
        public SearchCommand Search { get; set; }

        [InjectProperty]
        public NavBackCommand NavigateBackwardCommand { get; set; }

        [InjectProperty]
        public NavForwardCommand NavigateForwardCommand { get; set; }

        [InjectProperty]
        public GoToParentCommand GoToParentCommand { get; set; }
        
        [InjectProperty]
        public ShowInNewWindowCommand ShowInNewWindowCommand { get; set; }

        #endregion

        #region Constructor

        public ExplorerViewModel(
            int managedObjectId,
            ShellObject initialTarget)
        {
            ManagedObjectId = managedObjectId;

            Browser = new ExplorerBrowser();
            var log = Browser.NavigationLog;
            log.NavigationLogChanged += NavigationLogOnNavigationLogChanged;

            if (initialTarget != null)
            {
                Browser.Navigate(initialTarget);
            }
        }

        public ExplorerViewModel(int managedObjectId)
            :this(managedObjectId, null)
        {
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            var log = Browser.NavigationLog;
            log.NavigationLogChanged -= NavigationLogOnNavigationLogChanged;
            Browser.Dispose();
        }
        
        #endregion

        #region Private methods
        private void NavigationLogOnNavigationLogChanged(object sender, NavigationLogEventArgs e)
        {
            var log = (ExplorerBrowserNavigationLog)sender;
            DisplayingTarget = log.CurrentLocation;
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
        }

        public void FlushData()
        {
            OnNavigationUpdated();
        }
        #endregion
    }
}