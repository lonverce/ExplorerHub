using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace ExplorerHub.ViewModels
{
    public class WindowsFormsHostHelper
    {
        public static readonly DependencyProperty ChildFormProperty = DependencyProperty.RegisterAttached(
            "ChildForm", typeof(Control), 
            typeof(WindowsFormsHostHelper), new PropertyMetadata(OnChildFormChanged));

        private static void OnChildFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = (WindowsFormsHost) d;
            host.Child = e.NewValue as Control;
        }

        public static Control GetChildForm(DependencyObject obj) => (Control) obj.GetValue(ChildFormProperty);

        public static void SetChildForm(DependencyObject obj, Control value) => obj.SetValue(ChildFormProperty, value);
    }
}
