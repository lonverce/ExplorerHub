using System;
using System.Collections.Generic;

namespace ExplorerHub.Framework.Domain
{
    public abstract class AggregateRoot : Entity
    {
        private List<IEventData> _domainEventDataList;

        public void AddDomainEvent(IEventData eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            if (_domainEventDataList == null)
            {
                _domainEventDataList = new List<IEventData>();
            }

            _domainEventDataList.Add(eventData);
        }

        public void ClearDomainEvents()
        {
            _domainEventDataList?.Clear();
        }

        public IReadOnlyList<IEventData> GetDomainEvents()
        {
            return _domainEventDataList?.ToArray() ?? Array.Empty<IEventData>();
        }
    }
}
