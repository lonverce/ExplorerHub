using ExplorerHub.Infrastructures;
using ExplorerHub.ViewModels;

namespace ExplorerHub
{
    /// <summary>
    /// 托管对象
    /// </summary>
    public interface IManagedObject
    {
        /// <summary>
        /// 托管对象在<see cref="ManagedObjectPool{T}"/>中的标识Id
        /// </summary>
        int ManagedObjectId { get; }
    }
}