namespace ExplorerHub
{
    /// <summary>
    /// 托管对象
    /// </summary>
    public interface IManagedObject
    {
        /// <summary>
        /// 托管对象的标识Id
        /// </summary>
        int ManagedObjectId { get; }
    }
}