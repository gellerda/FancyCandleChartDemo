using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
