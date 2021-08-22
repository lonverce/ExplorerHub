using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExplorerHub.ViewModels
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        private readonly IOperationValidator _validator;

        public interface IOperationValidator
        {
            void OnAdding(T item);
        }

        public ObservableCollectionEx(IOperationValidator validator, IEnumerable<T> data)
            :base(data)
        {
            _validator = validator;
        }

        protected override void InsertItem(int index, T item)
        {
            _validator.OnAdding(item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            _validator.OnAdding(item);
            base.SetItem(index, item);
        }
    }
}
