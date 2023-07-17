using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using InteractiveDataDisplay.WPF;
using NationalInstruments.DAQmx;
using UsbDataReceiver;
using UsbDataReceiver.GUI.MVVM.ViewModel;

namespace UsbDataReceiver.GUI.MVVM.View;

public partial class ChartLayout : UserControl
{

    public ChartLayout(MeasuredDevice device)
    {
        InitializeComponent();
        if (plotter.Title is TextBlock titleTextBlock)
        {
            titleTextBlock.Text = device.Name;
        }
        var vm = DataContext as ChartLayoutViewModel;
        if(vm is null) return;
        foreach (var (key,_) in device.Data)
        {
            vm.AddLine(key);
        }
        vm.UpdateData(device.Data);
    }
}

