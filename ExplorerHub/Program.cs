using System.Linq;
using System.Windows;
using CommandLine;

namespace ExplorerHub
{
    public class Program
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThread]
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<AppStartupOptions>(args)
                .WithParsed(options =>
                {
                    var app = new App(options);
                    app.InitializeComponent();
                    app.Run();
                })
                .WithNotParsed(errors =>
                {
                    MessageBox.Show($"启动参数错误:\n{string.Join(",", errors.Select(error => error.Tag))}", "ExplorerHub", MessageBoxButton.OK, MessageBoxImage.Error);
                });
        }
    }
}
