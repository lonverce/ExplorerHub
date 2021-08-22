using System.Collections.Generic;

namespace ExplorerHub.ViewModels
{
    public interface IViewModelRepository<T> : IEnumerable<T>
        where T : IManagedObject
    {
        T Create();

        void Delete(int id);

        bool TryGetModelById(int id, out T model);
    }
}