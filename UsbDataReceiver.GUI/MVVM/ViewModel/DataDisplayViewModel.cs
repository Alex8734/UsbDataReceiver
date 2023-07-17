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

    public List<ChartLayout> Views { get; }
    public DataDisplayViewModel()
    {
        Views = new List<ChartLayout>();
    }
    
    public void AddDevice(MeasuredDevice device)
    {
        Views.Add(new ChartLayout(device));
        OnPropertyChanged();
    }
}