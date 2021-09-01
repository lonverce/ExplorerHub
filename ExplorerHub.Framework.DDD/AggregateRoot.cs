using System;
using System.Collections.Generic;

namespace ExplorerHub.Framework.Domain
{
    /// <summary>
    /// DDD概念中的聚合根
    /// </summary>
    public abstract class AggregateRoot : Entity
    {
        private List<IEventData> _domainEventDataList;

        /// <summary>
        /// 添加领域事件, 当此实体提交事务变更后, 这些事件将由应用框架主动被发布到<see cref="IEventBus"/>中
        /// </summary>
        /// <param name="eventData"></param>
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

        /// <summary>
        /// 清空领域事件
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEventDataList?.Clear();
        }

        /// <summary>
        /// 获取所有未被发布的领域事件
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<IEventData> GetDomainEvents()
        {
            return _domainEventDataList?.ToArray() ?? Array.Empty<IEventData>();
        }

        /// <summary>
        /// 指示当前领域聚合根是否存在未发布的领域事件
        /// </summary>
        /// <returns></returns>
        public bool HasDomainEvents() => _domainEventDataList?.Count > 0;
    }
}
