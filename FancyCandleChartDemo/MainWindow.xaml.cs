/* 
    Copyright 2019 Dennis Geller.

    This file is part of FancyCandleChartDemo.

    FancyCandleChartDemo is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FancyCandleChartDemo is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FancyCandleChartDemo.  If not, see<https://www.gnu.org/licenses/>. */

using System;
using System.Windows;
using System.Windows.Controls;

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
