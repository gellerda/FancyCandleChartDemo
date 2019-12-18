using System.Windows;
using System.Windows.Navigation;

namespace FancyCandleChartDemo
{
    public partial class IntroWindow : Window
    {
        public IntroWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
        //-----------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------
    }
}
