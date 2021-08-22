using System;

namespace ExplorerHub
{
    /// <summary>
    /// 事件订阅者
    /// </summary>
    /// <remarks>实现类需配合<see cref="EventSubscriberAttribute"/>使用</remarks>
    public interface IEventSubscriber
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        void Handle(IEventData eventData);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EventSubscriberAttribute : Attribute
    {
        public EventSubscriberAttribute(string eventName)
        {
            EventName = eventName;
        }

        /// <summary>
        /// 订阅的事件名称
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// 是否应在UI线程中执行处理，默认为false
        /// </summary>
        public bool UiHandle { get; set; } = false;
    }
}