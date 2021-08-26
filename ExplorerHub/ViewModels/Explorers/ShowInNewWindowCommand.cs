using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub.ViewModels.Explorers
{
    public class ShowInNewWindowCommand : ICommand, IDisposable
    {
        private readonly IHubWindowsManager _windowsManager;
        private readonly IViewModelRepository<ExplorerHubViewModel> _hubRepository;
        private readonly ExplorerViewModel _model;
        private ExplorerHubViewModel _hubModel;

        public bool CanExecute { get; private set; }

        public ShowInNewWindowCommand(
            IHubWindowsManager windowsManager,
            IViewModelRepository<ExplorerHubViewModel> hubRepository,
            ExplorerViewModel model)
        {
            _windowsManager = windowsManager;
            _hubRepository = hubRepository;
            _model = model;

            _model.PropertyChanged += ModelOnPropertyChanged;
            StartMonitoring();   
        }

        private void SetCanExecute(bool canExec)
        {
            if (CanExecute == canExec)
            {
                return;
            }

            CanExecute = canExec;

            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void StartMonitoring()
        {
            if (_model.OwnerId == -1)
            {
                _hubModel = null;
                SetCanExecute(false);
            }
            else
            {
                if (!_hubRepository.TryGetModelById(_model.OwnerId, out _hubModel))
                {
                    SetCanExecute(false);
                    throw new ArgumentException("Owner not found");
                }

                _hubModel.Explorers.CollectionChanged += OwnerCollectionChanged;
                SetCanExecute(_hubModel.Explorers.Count > 1);
            }
        }

        private void OwnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != _hubModel.Explorers)
            {
                return;
            }

            SetCanExecute(_hubModel.Explorers.Count > 1);
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(_model.OwnerId))
            {
                return;
            }

            if (_hubModel != null)
            {
                _hubModel.Explorers.CollectionChanged -= OwnerCollectionChanged;
                _hubModel = null;
            }
            
            StartMonitoring();
        }

        bool ICommand.CanExecute(object parameter) => CanExecute;

        void ICommand.Execute(object parameter)
        {
            Execute();  
        }

        public void Execute()
        {
            var hubModel = _hubModel;
            hubModel.CloseBrowser.Execute(_model, false);
            _windowsManager.CreateHubWindow().AddBrowser.Execute(_model, 0);
        }

        public event EventHandler CanExecuteChanged;

        public void Dispose()
        {
            if (_hubModel == null)
            {
                return;
            }

            _hubModel.Explorers.CollectionChanged -= OwnerCollectionChanged;
        }
    }
}