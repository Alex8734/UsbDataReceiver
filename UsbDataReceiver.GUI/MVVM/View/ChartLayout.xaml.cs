using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using InteractiveDataDisplay.WPF;
using NationalInstruments.DAQmx;
using UsbDataReceiver;
using UsbDataReceiver.GUI.MVVM.ViewModel;

namespace UsbDataReceiver.GUI.MVVM.View;

public partial class ChartLayout : UserControl
{
    private readonly DispatcherTimer _timer = new()
    {
        Interval = TimeSpan.FromMilliseconds(200)
    };
    public ChartLayout(MeasuredDevice device)
    {
        InitializeComponent();

        if (plotter.Title is TextBlock titleTextBlock)
        {
            titleTextBlock.Text = device.Name;
        }
        var vm = DataContext as ChartLayoutViewModel;
        if(vm is null) return;
        while (device.Data.Count < 1)
        {
            //wait until Data is initialized
            Thread.Sleep(10);
        }
        foreach (var (key,_) in device.Data)
        {
            vm.AddLine(key);
        }

        foreach (var graph in vm.LineGraphs)
        {
            lines.Children.Add(graph);
        }

        _timer.Tick += (sender, args) =>
        {
            vm.UpdateData(device.Data);
        };
        _timer.Start();
    }

    private void ChartLayout_OnUnloaded(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        
    }

    private void ChartLayout_OnLoaded(object sender, RoutedEventArgs e)
    {
        _timer.Start();
    }
}

