namespace ExplorerHub.Framework
{
    /// <summary>
    /// 框架中的初始化流程接口
    /// </summary>
    /// <remarks>
    /// 实现此接口的对象, 在注册到容器后, 将被框架最先调用执行以初始化应用程序. 
    /// </remarks>
    public interface IAppInitialization
    {
        /// <summary>
        /// 实现此方法, 以执行应用程序初始化工作.
        /// </summary>
        void InitializeAppComponents();
    }
}