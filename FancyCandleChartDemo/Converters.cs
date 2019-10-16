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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;

namespace FancyCandleChartDemo
{
    //*******************************************************************************************************************************************************************
    class NotBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //*******************************************************************************************************************************************************************
    class BoolAndNotBoolConverter : IMultiValueConverter
    {
        // values[0] - bool bool1
        // values[1] - bool bool2
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool bool1 = (bool)values[0];
            bool bool2 = (bool)values[1];
            return bool1 && (!bool2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }

    }
    //*******************************************************************************************************************************************************************
    class PenFromBrushAndThicknessConverter : IMultiValueConverter
    {
        // values[0] - Brush brush
        // values[1] - double thickness
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = (Brush)values[0];
            double thickness = (double)values[1];
            return new Pen(brush, thickness);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }

    }
    //*******************************************************************************************************************************************************************
    //*******************************************************************************************************************************************************************
    //*******************************************************************************************************************************************************************
}
