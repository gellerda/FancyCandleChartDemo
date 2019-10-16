using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FancyCandleChartDemo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //-----------------------------------------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
        }
        //-----------------------------------------------------------------------------------------------------------------
        void SetCandlesFromEmbeddedResourceTextFile(object sender, SelectionChangedEventArgs args)
        {
            ListBox senderListBox = sender as ListBox;
            if (senderListBox.SelectedIndex == -1) return;

            if (selectCandlesFromWeb != null) selectCandlesFromWeb.SelectedIndex = -1;
            ListBoxItem senderListBoxItem = senderListBox.SelectedItem as ListBoxItem;
            dataContextVM.SetCandlesFromEmbeddedResourceTextFile(senderListBoxItem.Content.ToString());
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void SetCandlesFromWeb(object sender, RoutedEventArgs e)
        {
            ListBox senderListBox = sender as ListBox;
            if (senderListBox.SelectedIndex == -1) return;

            if (selectCandlesFromFile != null) selectCandlesFromFile.SelectedIndex = -1;
            ListBoxItem senderListBoxItem = senderListBox.SelectedItem as ListBoxItem;
            dataContextVM.SetCandlesFromWeb(senderListBoxItem.Content.ToString());
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void SetCentralDateTime(object sender, RoutedEventArgs e)
        {
            DateTime d = centralDate.SelectedDate.Value;
            myCandleChart.CenterOnDateTime(new DateTime(d.Year, d.Month, d.Day, (int)centralHour.SelectedItem, (int)centralMinute.SelectedItem, 0));
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void SetVisibleCandlesRange(object sender, RoutedEventArgs e)
        {
            DateTime d0 = date0.SelectedDate.Value;
            DateTime d1 = date1.SelectedDate.Value;
            myCandleChart.SetVisibleCandlesRange(new DateTime(d0.Year, d0.Month, d0.Day, (int)hour0.SelectedItem, (int)minute0.SelectedItem, 0),
                                                  new DateTime(d1.Year, d1.Month, d1.Day, (int)hour1.SelectedItem, (int)minute1.SelectedItem, 0));
        }
        //-----------------------------------------------------------------------------------------------------------------
    }
}
