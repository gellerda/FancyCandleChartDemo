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

using System.Collections.Generic;
using System.Linq;

namespace FancyCandleChartDemo
{
    public class DateAndTimeDataProvider
    {
        static List<int> hours;
        static List<int> minutes;
        //------------------------------------------------------------------------------------------------------------------------------------------
        static DateAndTimeDataProvider()
        {
            hours = Enumerable.Range(0, 24).ToList();
            minutes = Enumerable.Range(0, 60).ToList();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        public static List<int> GetHours()
        {
            return hours;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        public static List<int> GetMinutes()
        {
            return minutes;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------
    }
}
