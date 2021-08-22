using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.OwnedInstances;

namespace ExplorerHub.Infrastructures
{
    /// <summary>
    /// 托管对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManagedObjectPool<T> : IViewModelRepository<T>, IDisposable
        where T : class, IManagedObject
    {
        private readonly ManagedObjectConstructFunc _objectFactory;
        private readonly Dictionary<int, Owned<T>> _pool;
        private int _idCnt = 1;

        public delegate Owned<T> ManagedObjectConstructFunc(int managedObjectId);
        
        public ManagedObjectPool(ManagedObjectConstructFunc objectFactory)
        {
            _objectFactory = objectFactory;
            _pool = new Dictionary<int, Owned<T>>();
        }

        private int CreateId() => _idCnt++;
        
        public void Dispose()
        {
            var items = _pool.Values.ToArray();
            _pool.Clear();

            foreach (var item in items)
            {
                item.Dispose();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _pool.Values.Select(owned =>owned.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Create()
        {
            var newId = CreateId();
            var newItem = _objectFactory(newId);
            _pool.Add(newItem.Value.ManagedObjectId, newItem);
            return newItem.Value;
        }

        public void Delete(int id)
        {
            if (!_pool.TryGetValue(id, out var owned))
            {
                return;
            }

            if (!_pool.Remove(id))
            {
                return;
            }

            owned.Dispose();
        }

        public bool TryGetModelById(int id, out T model)
        {
            if (!_pool.TryGetValue(id, out var owned))
            {
                model = default;
                return false;
            }

            model = owned.Value;
            return true;
        }
    }
}
