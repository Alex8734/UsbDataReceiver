using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using UsbDataReceiver.GUI.MVVM.ViewModel;

namespace UsbDataReceiver.GUI.Core;

public class LegendItemVisibilityToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if(value is Dictionary<string, Line> lines 
           && parameter is string key 
           && lines.TryGetValue(key, out var line))
        {
            return line.LegendVisibility ? Visibility.Visible : Visibility.Hidden;
        }
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return Visibility.Hidden;
    }
}

public class VisibilityToCheckedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return ((Visibility)value) == Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
    }
}