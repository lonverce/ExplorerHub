using System.Windows;
using ExplorerHub.ViewModels.Explorers;
using GongSolutions.Wpf.DragDrop;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class ExplorerHubDropTarget : IDropTarget
    {
        private readonly IViewModelRepository<ExplorerHubViewModel> _hubRepository;
        private readonly ExplorerHubViewModel _vm;

        public ExplorerHubDropTarget(
            IViewModelRepository<ExplorerHubViewModel> hubRepository,
            ExplorerHubViewModel vm)
        {
            _hubRepository = hubRepository;
            _vm = vm;
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is ExplorerViewModel))
            {
                return;
            }

            if (dropInfo.TargetItem is ExplorerViewModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.All;
                dropInfo.DestinationText = "放置到这里";
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is ExplorerViewModel sourceItem))
            {
                return;
            }

            if (dropInfo.TargetItem is ExplorerViewModel targetItem)
            {
                if (sourceItem == targetItem)
                {
                    return;
                }

                var sourceIndex = _vm.Explorers.IndexOf(sourceItem);
                var targetIndex = _vm.Explorers.IndexOf(targetItem);

                if (sourceIndex != -1)
                {
                    _vm.Explorers.Move(sourceIndex, targetIndex);
                }
                else
                {
                    // 跨窗体插入
                    if (!_hubRepository.TryGetModelById(sourceItem.OwnerId, out var ownerHub))
                    {
                        return;
                    }

                    ownerHub.CloseBrowser.Execute(sourceItem, false);
                    _vm.AddBrowser.Execute(sourceItem, targetIndex);
                }
            }
        }
    }
}