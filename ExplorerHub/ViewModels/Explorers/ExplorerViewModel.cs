using System;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ExplorerHub.Events;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.Explorers
{
    [Flags]
    public enum ItemPositionType
    {
        None = 0,
        Head = 1,
        Tail = 2,
        Both = Head | Tail
    }

    public class ExplorerViewModel : ViewModelBase,IManagedObject, IDisposable
    {
        private readonly IEventBus _eventBus;

        #region Fields

        private string _title;
        private string _navigationPath;
        private ShellObject _displayingTarget;
        private int _ownerId = -1;
        private ItemPositionType _position;

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

        public ItemPositionType Position
        {
            get => _position;
            set
            {
                if (_position == value)
                {
                    return;
                }

                _position = value;
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
            ShellObject initialTarget,
            IEventBus eventBus)
        {
            _eventBus = eventBus;
            ManagedObjectId = managedObjectId;

            Browser = new ExplorerBrowser();
            Browser.BorderStyle = BorderStyle.None;
            var log = Browser.NavigationLog;
            log.NavigationLogChanged += NavigationLogOnNavigationLogChanged;

            if (initialTarget != null)
            {
                Browser.Navigate(initialTarget);
            }
        }

        public ExplorerViewModel(int managedObjectId, IEventBus eventBus)
            :this(managedObjectId, null, eventBus)
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
        private void NavigationLogOnNavigationLogChanged(object sender, NavigationLogEventArgs e)
        {
            var log = (ExplorerBrowserNavigationLog)sender;
            DisplayingTarget = log.CurrentLocation;
            _eventBus.PublishEvent(new ExplorerNavigationUpdatedEventData(ManagedObjectId));
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