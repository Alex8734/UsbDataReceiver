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

public partial class ChartItem : UserControl
{
    private readonly DispatcherTimer _timer = new()
    {
        Interval = TimeSpan.FromMilliseconds(200)
    };

    public ChartItem(string title, List<MeasurementData> logData,string measurePortKey, SolidColorBrush stroke)
    {
        InitializeComponent();

        if (plotter.Title is TextBlock titleTextBlock)
        {
            titleTextBlock.Text = title;
        }
        if (DataContext is not ChartLayoutViewModel vm) return;
        var startTime = logData.Select(d => d.TimeStamp).Min();
        
        //convert data to a dictionary with the key as the name of the port and the value as a tuple of x and y values
        var data = logData
            .SelectMany(d => d.Data.Select(kv => new
            {
                Key = kv.Key,
                Time = (d.TimeStamp - startTime).TotalHours,
                Value = kv.Value
            })).Where(k => k.Key.Contains(measurePortKey))
            .GroupBy(d => d.Key)
            .ToDictionary(
                g => g.Key,
                g => (
                    x: g.Select(d => d.Time).ToList(),
                    y: g.Select(d => d.Value).ToList()
                ));

        /*var keys = logData.First().Data.Keys.Where(d => d.Contains(measurePortKey));
        
        foreach (var key in keys)
        {
            List<double> x = logData.Select(d => (StartTime - d.TimeStamp).TotalHours).ToList();
            List<double> y = logData.Select(d => d.Data[key]).ToList();
            
        }*/
        

        while (data.Count < 1)
        {
            //wait until Data is initialized
            Thread.Sleep(10);
        }
        foreach (var (key, value) in data)
        {
            vm.AddLine(key, value.x, value.y, stroke);
        }

        foreach (var graph in vm.LineGraphs)
        {
            lines.Children.Add(graph);
        }
    }


    /// <summary>
    ///     Creates a Chart, that displays the given Values from The Device
    /// </summary>
    /// <param name="device">the measured Device</param>
    /// <param name="measurePortKey">if you want to measure just a Port like (Voltage) with (MaxVoltage) and (MinVoltage) add just the (Voltage) key!</param>
    public ChartItem(MeasuredDevice device, SolidColorBrush stroke, string? measurePortKey = null)
    {
        InitializeComponent();

        if (plotter.Title is TextBlock titleTextBlock)
        {
            titleTextBlock.Text = measurePortKey ?? device.Name;
        }
        if(DataContext is not ChartLayoutViewModel vm) return;
        
         
        Dictionary<string, double> GetDataWithContainingKey(Dictionary<string, double> data, string key)
        {
            return measurePortKey is null 
                ? device.Data 
                : device.Data.Where(d => d.Key.Contains(measurePortKey))
                    .ToDictionary(kv => kv.Key,kv => kv.Value);
        }
        
        while (GetDataWithContainingKey(device.Data,measurePortKey).Count < 1)
        {
            //wait until Data is initialized
            Thread.Sleep(10);
        }
        
        foreach (var (key,_) in GetDataWithContainingKey(device.Data,measurePortKey))
        {
            vm.AddLine(key, stroke);
        }

        foreach (var graph in vm.LineGraphs)
        {
            lines.Children.Add(graph);
        }

        _timer.Tick += (sender, args) =>
        {
            vm.UpdateData(GetDataWithContainingKey(device.Data,measurePortKey));
            plotter.PlotHeight = vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.Y) - vm.Lines.Min(p => p.Value.LineChart.Points.Min(d => d.Y)));
            if(vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.X)) < 0.02)
            {
                plotter.PlotWidth = vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.X));
            }
            else if(vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.X)) >= plotter.PlotWidth )
            {
                plotter.PlotOriginX = vm.Lines.Max(p => p.Value.LineChart.Points.Max(d => d.X)) -plotter.PlotWidth;
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

