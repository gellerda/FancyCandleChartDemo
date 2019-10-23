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
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;

namespace FancyCandleChartDemo
{
    //**************************************************************************************************************************************************
    public class ByteTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[0-9]{0,3}$");

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string new_str = Text.Substring(0, SelectionStart) + e.Text + Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
            if (!regex.IsMatch(new_str) || (new_str.Length > 0 && int.Parse(new_str) > 255))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
    //**************************************************************************************************************************************************
    public class NumbersAndSpacesTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[0-9 ]*$");

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string new_str = Text.Substring(0, SelectionStart) + e.Text + Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
            if (!regex.IsMatch(new_str))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
    //**************************************************************************************************************************************************
    public class DoubleTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[0-9]*(.[0-9]*)?$");
        private static readonly NumberStyles parseStyles = NumberStyles.Float;
        private static readonly IFormatProvider parseProvider = CultureInfo.CreateSpecificCulture("en-GB");

        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        //----------------------------------------------------------------------------------------------------------------------------------
        public DoubleTextBox()
        {
            MinValue = double.MinValue;
            MaxValue = double.MaxValue;
            TextChanged += new TextChangedEventHandler(OnTextChanged);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            DoubleTextBox thisTextBox = sender as DoubleTextBox;
            if (sender == null)
                return;

            if (thisTextBox.Text.Length > 0 && thisTextBox.Text[0] == '.')
            {
                thisTextBox.Text = "0" + thisTextBox.Text;
                thisTextBox.SelectionStart = thisTextBox.Text.Length;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string new_str = Text.Substring(0, SelectionStart) + e.Text + Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
            if (!regex.IsMatch(new_str))
                e.Handled = true;
            else
            {
                double d = double.Parse(new_str, parseStyles, parseProvider);
                if (d > MaxValue || d < MinValue)
                    e.Handled = true;
            }
            base.OnPreviewTextInput(e);
        }
    }
    //**************************************************************************************************************************************************
    public class Hex8DigitTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[#]?[0-9a-fA-F]{0,8}$");
        //----------------------------------------------------------------------------------------------------------------------------------
        public Hex8DigitTextBox()
        {
            TextChanged += new TextChangedEventHandler(OnTextChanged);
            Loaded += OnLoaded;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        static void OnLoaded(object sender, RoutedEventArgs e)
        {
            Hex8DigitTextBox thisTextBox = sender as Hex8DigitTextBox;
            thisTextBox.Width = thisTextBox.MeasureString("#DDDDDDDD").Width;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Hex8DigitTextBox thisTextBox = sender as Hex8DigitTextBox;
            if (sender == null)
                return;

            if (thisTextBox.Text.Length == 0 || thisTextBox.Text[0] != '#')
            {
                thisTextBox.Text = "#" + thisTextBox.Text;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string new_str = Text.Substring(0, SelectionStart) + e.Text + Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
            if (!regex.IsMatch(new_str))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
    //**************************************************************************************************************************************************
    static class TextBoxExtensionMethods
    {
        //----------------------------------------------------------------------------------------------------------------------------------
        public static Size MeasureString(this TextBox thisTextBox, string stringToMeasure)
        {
            var formattedText = new FormattedText(
                stringToMeasure,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(thisTextBox.FontFamily, thisTextBox.FontStyle, thisTextBox.FontWeight, thisTextBox.FontStretch),
                thisTextBox.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                1);

            return new Size(formattedText.Width, formattedText.Height);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
    }
    //**************************************************************************************************************************************************
    //**************************************************************************************************************************************************
}
