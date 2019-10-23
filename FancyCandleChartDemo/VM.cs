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
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices; // [CallerMemberName]
using System.IO;
using FancyCandles;
using System.Windows;
using System.Reflection;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Windows.Threading;

namespace FancyCandleChartDemo
{
    public class VM : DependencyObject, INotifyPropertyChanged
    {
        public VM()
        {
            httpClient = new HttpClient();
            сandlesUpdateTimer = new DispatcherTimer();
            сandlesUpdateTimer.Interval = new TimeSpan(0, 0, updateCandlesFromInternetTimer_step);
            сandlesUpdateTimer.Tick += new EventHandler(OnUpdateCandlesFromInternetTimerTick);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ReplaceCandle()
        {
            Candle old_cndl = Candles[Candles.Count - 1] as Candle;
            Candle new_cndl = new Candle() { O = old_cndl.O, H = 1.005 * old_cndl.H, L = old_cndl.L, C = old_cndl.C, V = old_cndl.V, t = old_cndl.t };
            Candles[Candles.Count - 1] = new_cndl;
        }

        public void AddCandle()
        {
            Candle old_cndl = Candles[Candles.Count - 1] as Candle;
            Candle new_cndl = new Candle() { O = old_cndl.O, H = old_cndl.H, L = old_cndl.L, C = old_cndl.C, V = old_cndl.V, t = old_cndl.t };
            Candles.Add(new_cndl);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        public string Ticker
        {
            get { return (string)GetValue(TickerProperty); }
            set { SetValue(TickerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ticker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickerProperty =
            DependencyProperty.Register("Ticker", typeof(string), typeof(VM), new PropertyMetadata("Hello"));
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetCandlesFromEmbeddedResourceTextFile(string embeddedResourceTextFileName)
        {
            //---------
            ObservableCollection<ICandle> LoadCandlesFromEmbeddedResourceTextFile(out string ticker)
            {
                ObservableCollection<ICandle> candles = new ObservableCollection<ICandle>();
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("FancyCandleChartDemo." + embeddedResourceTextFileName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadLine(); // First line contains titles. 

                    IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-GB");
                    ticker = null;
                    while ((result = reader.ReadLine()) != null)
                    {
                        string[] arr = result.Split(';');
                        if (ticker == null) ticker = arr[0];
                        string str_date = arr[2];
                        string str_time = arr[3];
                        DateTime t = new DateTime(int.Parse(str_date.Substring(0, 4)), int.Parse(str_date.Substring(4, 2)), int.Parse(str_date.Substring(6, 2)), int.Parse(str_time.Substring(0, 2)), int.Parse(str_time.Substring(2, 2)), 0);
                        ICandle cndl = new Candle() { t = t, O = double.Parse(arr[4], provider), H = double.Parse(arr[5], provider), L = double.Parse(arr[6], provider), C = double.Parse(arr[7], provider), V = long.Parse(arr[8]) };
                        candles.Add(cndl);
                    }
                }
                return candles;
            }
            //---------

            сandlesUpdateTimer.Stop();

            string ticker_;
            Candles = LoadCandlesFromEmbeddedResourceTextFile(out ticker_);
            Ticker = ticker_;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        bool isCandlesLoading = false;
        public bool IsCandlesLoading
        {
            get { return isCandlesLoading; }
            set
            {
                isCandlesLoading = value;
                OnPropertyChanged();
            }
        }

        readonly string exchangeNameForRestApi = "coinbase-pro";
        string tickerNameForRestApi;

        public async void SetCandlesFromWeb(string tickerNameForRestApi_)
        {
            tickerNameForRestApi = tickerNameForRestApi_;
            //---------
            async Task<ObservableCollection<ICandle>> LoadCandlesFromWeb()
            {
                IsCandlesLoading = true;
                ObservableCollection<ICandle> candles_ = null;
                try
                {
                    //HttpResponseMessage response = await client.GetAsync("https://api.cryptowat.ch/markets/coinbase-pro/btcusd/ohlc?before=1566239760&after=1566232020&periods=60");
                    HttpResponseMessage response = await httpClient.GetAsync($"https://api.cryptowat.ch/markets/{exchangeNameForRestApi}/{tickerNameForRestApi}/ohlc?periods=60");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //MessageBox.Show("Good! " + responseBody);
                    JObject jObj = JObject.Parse(responseBody);
                    JToken jToken = jObj["result"]["60"];
                    int N = jToken.Count();

                    candles_ = new ObservableCollection<ICandle>();
                    for (int i = 0; i < N; i++)
                    {
                        JToken jCandle = jToken[i];
                        lastCandleUnixTime = (long)(jCandle[0]);
                        DateTime t = UnixTimeStampToDateTime(lastCandleUnixTime);
                        ICandle cndl = new Candle() { t = t, O = (double)(jCandle[1]), H = (double)(jCandle[2]), L = (double)(jCandle[3]), C = (double)(jCandle[4]), V = (long)(jCandle[5]) };
                        candles_.Add(cndl);
                    }

                    сandlesUpdateTimer.Start();
                }
                catch (HttpRequestException ee)
                {
                    MessageBox.Show(ee.Message);
                }

                IsCandlesLoading = false;
                return candles_;
            }
            //---------

            Candles = await LoadCandlesFromWeb();
            Ticker = tickerNameForRestApi;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        readonly int updateCandlesFromInternetTimer_step = 3;
        DispatcherTimer сandlesUpdateTimer;
        long lastCandleUnixTime = 0;
        HttpClient httpClient;

        async void OnUpdateCandlesFromInternetTimerTick(object sender, EventArgs e)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://api.cryptowat.ch/markets/{exchangeNameForRestApi}/{tickerNameForRestApi}/ohlc?after={lastCandleUnixTime}&periods=60");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JToken jCandles = JObject.Parse(responseBody)["result"]["60"];
                int N = jCandles.Count();

                for (int i = 0; i < N; i++)
                {
                    JToken jCandle = jCandles[i];
                    long unixTime = (long)(jCandle[0]);
                    if (unixTime < lastCandleUnixTime) continue;

                    DateTime t = UnixTimeStampToDateTime(unixTime);
                    ICandle cndl = new Candle() { t = t, O = (double)(jCandle[1]), H = (double)(jCandle[2]), L = (double)(jCandle[3]), C = (double)(jCandle[4]), V = (long)(jCandle[5]) };
                    if (unixTime == lastCandleUnixTime)
                    {
                        if (!Candles[Candles.Count - 1].IsEqualByValue(cndl))
                            Candles[Candles.Count - 1] = cndl;
                    }
                    else
                    {
                        Candles.Add(cndl);
                        lastCandleUnixTime = unixTime;
                    }
                }
            }
            catch (HttpRequestException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<ICandle> candles;
        public ObservableCollection<ICandle> Candles
        {
            get { return candles; }
            set
            {
                candles = value;
                OnPropertyChanged();
            }
        }
        //--------------------------------------------------------------------------------------------------
        //---------------- INotifyPropertyChanged ----------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
    //*****************************************************************************************************************
    //*****************************************************************************************************************
}
