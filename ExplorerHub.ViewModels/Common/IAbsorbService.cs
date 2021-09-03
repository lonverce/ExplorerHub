using System;
using System.Threading.Tasks;
using ExplorerHub.ViewModels.Explorers;

namespace ExplorerHub.ViewModels
{
    public interface IAbsorbService
    {
        /// <summary>
        /// 吸入外部Shell窗体到当前进程
        /// </summary>
        /// <param name="shellWindow"></param>
        /// <returns></returns>
        /// <exception cref="AbsorbFailureException">外部Shell窗体的导航路径无法被当前进程打开</exception>
        Task<ExplorerViewModel> AbsorbAsync(IShellWindow shellWindow);
    }

    public class AbsorbFailureException : Exception
    {
        public string LocationUrl { get; }

        public string Detail { get; }

        public AbsorbFailureException(string locationUrl, string detail)
        {
            LocationUrl = locationUrl;
            Detail = detail;
        }

        public override string Message => $"无法定位到 '{LocationUrl}'. 详细错误: {Detail}";
    }
}