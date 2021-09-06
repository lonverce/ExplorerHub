using CommandLine;

namespace ExplorerHub
{
    /// <summary>
    /// 应用程序启动参数
    /// </summary>
    public class AppStartupOptions
    {
        [Value(0, Required = false, HelpText = "打开文件夹路径")]
        public string Directory { get; set; }

        [Option('m', "mini", Required = false, HelpText = "迷你化启动, 不打开默认浏览页面", Default = false)]
        public bool MiniStart { get; set; }
    }
}