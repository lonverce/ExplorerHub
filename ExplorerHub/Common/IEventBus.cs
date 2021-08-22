using System;

namespace ExplorerHub
{
    /// <summary>
    /// 事件消息发布对象
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        void PublishEvent(IEventData eventData);
    }

    /// <summary>
    /// 事件数据
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        string Name { get; }
    }
}