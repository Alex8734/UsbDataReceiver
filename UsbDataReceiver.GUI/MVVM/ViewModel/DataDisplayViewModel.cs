using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using InteractiveDataDisplay.WPF;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.GUI.MVVM.View;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class DataDisplayViewModel : ObservableObject
{
    public ObservableCollection<ChartLayout> Views { get; }

    public DataDisplayViewModel()
    {
        Views = new ObservableCollection<ChartLayout>();
    }

    public void SetDevice(MeasuredDevice device)
    {
        if (Views.Count > 0)
        {
            Views.Clear();
            OnPropertyChanged(nameof(Views));
        }
        foreach (var key in device.Data
                     .Where(d => !d.Key.Contains("Max") && !d.Key.Contains("Min"))
                     .Select(v => v.Key))
        {
            var chartLayout = new ChartLayout(device, key);
            Views.Add(chartLayout);
        }
        OnPropertyChanged(nameof(Views));
    }
}