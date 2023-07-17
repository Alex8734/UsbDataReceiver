using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        ((TextBlock)plotter.Title).Text = device.Name;
        int i = 0;
        var started = DateTime.Now;
        foreach (var (key,value) in device.Data)
        {
            var line = new LineGraph
            {
                Stroke = Brushes.Red,
                Description = key,
                StrokeThickness = 2,
            };
            line.Plot(new[]{3,4},new[]{2,3});
            lines.Children.Add(line);
            line.Points.Add(new Point(10,10));
            if(!key.Contains("Max") && !key.Contains("Min"))
                i++;
        }
    }
    
    
}