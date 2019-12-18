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
using System.Windows.Navigation;

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
        public void OnEverythingLoaded(object sender, RoutedEventArgs e)
        {
            IntroWindow popup = new IntroWindow();
            popup.ShowDialog();
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
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
        private void SetVisibleCandlesRangeByCenter(object sender, RoutedEventArgs e)
        {
            DateTime t = centralDate.SelectedDate.Value;
            myCandleChart.SetVisibleCandlesRangeCenter(new DateTime(t.Year, t.Month, t.Day, (int)centralHour.SelectedItem, (int)centralMinute.SelectedItem, 0));
        }
        //-----------------------------------------------------------------------------------------------------------------
        private void SetVisibleCandlesRangeByBounds(object sender, RoutedEventArgs e)
        {
            DateTime t0 = date0.SelectedDate.Value;
            DateTime t1 = date1.SelectedDate.Value;
            myCandleChart.SetVisibleCandlesRangeBounds(new DateTime(t0.Year, t0.Month, t0.Day, (int)hour0.SelectedItem, (int)minute0.SelectedItem, 0),
                                                  new DateTime(t1.Year, t1.Month, t1.Day, (int)hour1.SelectedItem, (int)minute1.SelectedItem, 0));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;

            if (menuItem.Header.ToString() == "View FancyCandles Documentation")
                System.Diagnostics.Process.Start("https://gellerda.github.io/FancyCandles/articles/overview.html");
            else if (menuItem.Header.ToString() == "FancyCandles GitHub Repo")
                System.Diagnostics.Process.Start("https://github.com/gellerda/FancyCandles");
            else if (menuItem.Header.ToString() == "FancyCandles NuGet Package")
                System.Diagnostics.Process.Start("https://www.nuget.org/packages/FancyCandles/");
            else if (menuItem.Header.ToString() == "FancyCandles Demo GitHub Repo")
                System.Diagnostics.Process.Start("https://github.com/gellerda/FancyCandleChartDemo");
            else if (menuItem.Header.ToString() == "About FancyCandles Demo")
            {
                IntroWindow popup = new IntroWindow();
                popup.ShowDialog();
            }
        }
        //-----------------------------------------------------------------------------------------------------------------
    }
}
