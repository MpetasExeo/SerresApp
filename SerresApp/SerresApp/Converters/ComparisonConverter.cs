﻿
using System;
using System.Globalization;

using Xamarin.Forms;

namespace SerresApp.Converters
{

    public class ComparisonConverter : IValueConverter
    {
        public object Convert(object value , Type targetType , object parameter , CultureInfo culture)
        {
            NumberFormatInfo fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";

            var allParams = ((string)parameter).Split(';');
            var compValue = double.Parse(allParams[0] , fmt);
            var comparison = allParams[1];

            switch (comparison)
            {
                case "<":
                    return (double)value < compValue;
                case ">":
                    return (double)value > compValue;
                case "!=":
                    return (double)value != compValue;
                case "==":
                default:
                    return (double)value == compValue;
            }
        }
        public object ConvertBack(object value , Type targetType , object parameter , CultureInfo culture)
        {
            return null;
        }
    }
}