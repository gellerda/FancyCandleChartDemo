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
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

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
