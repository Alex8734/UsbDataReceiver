﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using InteractiveDataDisplay.WPF;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.GUI.MVVM.View;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class DataDisplayViewModel : ObservableObject
{
    public ObservableCollection<ChartItem> Views { get; }
    private string _deviceName;
    public Stack<SolidColorBrush> Strokes { get; }
    public string DeviceName
    {
        get => _deviceName;
        set
        {
            _deviceName = value;
            OnPropertyChanged();
        }
        
    }

    public DataDisplayViewModel()
    {
        Strokes = new Stack<SolidColorBrush>();
        Views = new ObservableCollection<ChartItem>();
        _deviceName = string.Empty;

        foreach (var stroke in Line.Strokes.Reverse())
        {
            Strokes.Push(stroke);
        }
        
    }
    public void SetData(List<MeasurementData> data,string deviceName)
    {
        DeviceName = "Device";
        if (Views.Count > 0)
        {
            Views.Clear();
            OnPropertyChanged(nameof(Views));
        }
        if(data.Count < 1)
            return;
        foreach (var key in data.First().Data
                     .Where(d => !d.Key.Contains("Max") && !d.Key.Contains("Min"))
                     .Select(v => v.Key))
        {
            var chartLayout = new ChartItem(key, data, key,Strokes.Pop());
            Views.Add(chartLayout);
        }
    }
    public void SetDevice(MeasuredDevice device)
    {
        DeviceName = device.Name;
        if (Views.Count > 0)
        {
            Views.Clear();
            OnPropertyChanged(nameof(Views));
        }
        foreach (var key in device.Data
                     .Where(d => !d.Key.Contains("Max") && !d.Key.Contains("Min"))
                     .Select(v => v.Key))
        {
            var chartLayout = new ChartItem(device,Strokes.Pop(), key);
            Views.Add(chartLayout);
        }
        OnPropertyChanged(nameof(Views));
    }
}