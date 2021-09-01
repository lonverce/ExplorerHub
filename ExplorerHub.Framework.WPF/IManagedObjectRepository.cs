using System.Collections.Generic;

namespace ExplorerHub.Framework.WPF
{
    public interface IManagedObjectRepository<T> : IEnumerable<T>
        where T : IManagedObject
    {
        T Create();

        void Delete(int id);

        bool TryGetModelById(int id, out T model);
    }
}