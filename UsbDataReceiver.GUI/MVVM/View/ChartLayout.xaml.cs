using System;
using System.Collections.Generic;
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
    /// <summary>
    ///     Creates a Chart, that displays the given Values from The Device
    /// </summary>
    /// <param name="device">the measured Device</param>
    /// <param name="measurePortKey">if you want to measure just a Port like (Voltage) with (MaxVoltage) and (MinVoltage) add just the (Voltage) key!</param>
    public ChartLayout(MeasuredDevice device, string? measurePortKey = null)
    {
        InitializeComponent();

        if (plotter.Title is TextBlock titleTextBlock)
        {
            titleTextBlock.Text = measurePortKey ?? device.Name;
        }
        if(DataContext is not ChartLayoutViewModel vm) return;
        
        var data = measurePortKey is null 
            ? device.Data 
            : device.Data.Where(d => d.Key.Contains(measurePortKey))
                .ToDictionary(kv => kv.Key,kv => kv.Value);

        while (data.Count < 1)
        {
            //wait until Data is initialized
            Thread.Sleep(10);
        }
        foreach (var (key,_) in data)
        {
            vm.AddLine(key);
        }

        foreach (var graph in vm.LineGraphs)
        {
            lines.Children.Add(graph);
        }

        _timer.Tick += (sender, args) =>
        {
            vm.UpdateData(data);
            if(vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.X)) >= plotter.PlotWidth )
            {
                
                plotter.PlotOriginX = vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.X)) -plotter.PlotWidth;
                plotter.PlotHeight = vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.Y));
            }
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

    private void Plotter_OnDragLeave(object sender, DragEventArgs e)
    {
        if(sender is not Chart plotter) return;
        plotter.IsAutoFitEnabled = true;
    }
}

