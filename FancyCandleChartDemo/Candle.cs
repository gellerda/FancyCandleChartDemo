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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FancyCandles;

namespace FancyCandleChartDemo
{
    //**************************************************************************************************************************
    public class Candle : ICandle
    {
        public DateTime t { get; set; } // Момент времени включая дату и время
        public double O { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double C { get; set; }
        public long V { get; set; }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Возвращает историю Daily, построенную по истории меньшего таймфрейма.
        // lowerTFCandles должны быть упорядочены по времени.
        // minTimeThread, maxTimeThread - ограничители для времени свечек. Если время не попадает в диапазон [minTimeThread, maxTimeThread], то такая свечка игнорируется.
        // timeOfEveryCandle - время, которое будет иметь каждая дневная свеча
        public static List<Candle> ConvertToHistoryD(IList<Candle> lowerTFCandles, TimeSpan minTimeThread, TimeSpan maxTimeThread, TimeSpan timeOfEveryCandle)
        {
            if (lowerTFCandles == null) return null;

            List<Candle> res = new List<Candle>();
            /* Прикольно, но работает медленно
            return lowerTFCandles
                        .Where(c => c.t.TimeOfDay >= minTimeThread && c.t.TimeOfDay <= maxTimeThread)
                        .GroupBy(c => c.t.Date).Select(g => new Candle() { t = g.First<Candle>().t, O=g.First().O, C=g.Last().C, H=g.Select(gc=>gc.H).Max(), L=g.Select(gc => gc.L).Min(), V=g.Select(gc => gc.V).Sum() })
                        .ToList();*/

            DateTime curDay = DateTime.MinValue;
            double curO = 0, curH = 0, curL = 0, curC = 0;
            long curV = 0;

            for (int i = 0; i < lowerTFCandles.Count; i++)
            {
                Candle lowerTFCandle = lowerTFCandles[i];

                if (lowerTFCandle.t.TimeOfDay < minTimeThread || lowerTFCandle.t.TimeOfDay > maxTimeThread) continue;

                if (lowerTFCandle.t.Date != curDay)
                {
                    if (curDay != DateTime.MinValue)
                        res.Add(new Candle()
                        {
                            t = new DateTime(curDay.Year, curDay.Month, curDay.Day, timeOfEveryCandle.Hours, timeOfEveryCandle.Minutes, 0),
                            O = curO,
                            H = curH,
                            L = curL,
                            C = curC,
                            V = curV
                        });

                    curDay = lowerTFCandle.t.Date;
                    curO = lowerTFCandle.O;
                    curL = lowerTFCandle.L;
                    curH = lowerTFCandle.H;
                    curC = lowerTFCandle.C;
                    curV = lowerTFCandle.V;
                }
                else
                {
                    if (curL > lowerTFCandle.L) curL = lowerTFCandle.L;
                    if (curH < lowerTFCandle.H) curH = lowerTFCandle.H;
                    curC = lowerTFCandle.C;
                    curV += lowerTFCandle.V;
                }
            }

            if (lowerTFCandles.Count > 0 && curDay != DateTime.MinValue)
                res.Add(new Candle()
                {
                    t = new DateTime(curDay.Year, curDay.Month, curDay.Day, timeOfEveryCandle.Hours, timeOfEveryCandle.Minutes, 0),
                    O = curO,
                    H = curH,
                    L = curL,
                    C = curC,
                    V = curV
                });

            return res;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Возвращает историю H1, построенную по истории меньшего таймфрейма.
        // lowerTFCandles должны быть упорядочены по времени.
        // minTimeThread, maxTimeThread - ограничители для времени свечек. Если время не попадает в диапазон [minTimeThread, maxTimeThread], то такая свечка игнорируется.
        // Каждая часовая свечка будет иметь время с нулевыми минутами и секундами.
        public static List<Candle> ConvertToHistoryH1(IList<Candle> lowerTFCandles, TimeSpan minTimeThread, TimeSpan maxTimeThread)
        {
            if (lowerTFCandles == null) return null;
            if (lowerTFCandles.Count == 0) return new List<Candle>();

            List<Candle> res = new List<Candle>();
            DateTime curHour = DateTime.MinValue; //Храним только компоненты от года до часа.
            double curO = 0, curH = 0, curL = 0, curC = 0;
            long curV = 0;

            for (int i = 0; i < lowerTFCandles.Count; i++)
            {
                Candle lowerTFCandle = lowerTFCandles[i];

                if (lowerTFCandle.t.TimeOfDay < minTimeThread || lowerTFCandle.t.TimeOfDay > maxTimeThread) continue;

                if (curHour == DateTime.MinValue || lowerTFCandle.t.Hour != curHour.Hour)
                {
                    if (curHour != DateTime.MinValue)
                        res.Add(new Candle()
                        {
                            t = new DateTime(curHour.Year, curHour.Month, curHour.Day, curHour.Hour, 0, 0),
                            O = curO,
                            H = curH,
                            L = curL,
                            C = curC,
                            V = curV
                        });

                    curHour = new DateTime(lowerTFCandle.t.Year, lowerTFCandle.t.Month, lowerTFCandle.t.Day, lowerTFCandle.t.Hour, 0, 0);
                    curO = lowerTFCandle.O;
                    curL = lowerTFCandle.L;
                    curH = lowerTFCandle.H;
                    curC = lowerTFCandle.C;
                    curV = lowerTFCandle.V;
                }
                else
                {
                    if (curL > lowerTFCandle.L) curL = lowerTFCandle.L;
                    if (curH < lowerTFCandle.H) curH = lowerTFCandle.H;
                    curC = lowerTFCandle.C;
                    curV += lowerTFCandle.V;
                }
            }

            if (curHour != DateTime.MinValue)
                res.Add(new Candle()
                {
                    t = new DateTime(curHour.Year, curHour.Month, curHour.Day, curHour.Hour, 0, 0),
                    O = curO,
                    H = curH,
                    L = curL,
                    C = curC,
                    V = curV
                });

            return res;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
    //**************************************************************************************************************************
    public static class CandleCollectionsExtension
    {
        public static int BinarySearchOfExistingCandleInObservableCollection(this ObservableCollection<Candle> candles, Candle candleToFind)
        {
            CandleComparerByDatetime comparer = new CandleComparerByDatetime();

            int i0 = 0, i1 = candles.Count - 1;

            int res = comparer.Compare(candleToFind, candles[i0]);
            if (res == 0) return i0;
            else if (res < 0) return -1;

            res = comparer.Compare(candleToFind, candles[i1]);
            if (res == 0) return i1;
            else if (res > 0) return -1;

            while (true)
            {
                if ((i0 + 1) == i1) return i1;

                int i = (i0 + i1) / 2;
                res = comparer.Compare(candleToFind, candles[i]);
                if (res == 0) return i;
                else if (res > 0)
                    i0 = i;
                else
                    i1 = i;
            }
        }
    }
    //**************************************************************************************************************************
    public static class ICandleExtensionMethods
    {
        public static bool IsEqualByValue(this ICandle cndl1, ICandle cndl2)
        {
            return cndl1.O == cndl2.O && cndl1.H == cndl2.H && cndl1.L == cndl2.L && cndl1.C == cndl2.C && cndl1.V == cndl2.V && cndl1.t == cndl2.t;
        }
    }
    //**************************************************************************************************************************
    public class CandleComparerByDatetime : Comparer<Candle>
    {
        public override int Compare(Candle c1, Candle c2)
        {
            if (c1.t == c2.t)
                return 0;
            else if (c1.t > c2.t)
                return 1;
            else
                return -1;
        }
    }
    //**************************************************************************************************************************
    //**************************************************************************************************************************
    //**************************************************************************************************************************
}
