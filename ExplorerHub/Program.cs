using System.Windows;

namespace ExplorerHub
{
    public class Program
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
