using System.Threading;
using System.Threading.Tasks;

namespace ExplorerHub.Framework
{
    /// <summary>
    /// 应用程序单例代表
    /// </summary>
    /// <remarks>
    /// 为了确保应用程序在当前计算机系统中最多仅有一个运行进程, 应用程序在执行<see cref="IAppInitialization"/>的初始化流程前
    /// 应先申请<see cref="IAppLeader"/>实例, 若申请成功, 则代表当前进程具有应用的执行权; 否则, 应用程序应终止启动流程, 并将
    /// 启动参数发送到具有应用执行权的进程.
    /// </remarks>
    public interface IAppLeader
    {
        /// <summary>
        /// 等待来自另一个进程的启动参数
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<string[]> ReadMessageFromFollowerAsync(CancellationToken cancellation);

        /// <summary>
        /// 主动放弃已有的执行权
        /// </summary>
        void Quit();
    }
}