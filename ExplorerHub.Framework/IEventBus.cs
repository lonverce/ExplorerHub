using System;
using System.Threading.Tasks;

namespace ExplorerHub.Framework
{
    /// <summary>
    /// 应用程序事件总线
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        void PublishEvent(IEventData eventData);
    }

    /// <summary>
    /// 应用程序事件数据
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        string Name { get; }
    }
}