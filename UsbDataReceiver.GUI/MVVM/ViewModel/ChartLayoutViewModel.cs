using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Ink;
using System.Windows.Media;
using InteractiveDataDisplay.WPF;
using UsbDataReceiver.GUI.Core;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class ChartLayoutViewModel : ObservableObject
{
    public static Stack<SolidColorBrush> Strokes = new();

    public Dictionary<string, Line> Lines { get; }

    public IEnumerable<LineGraph> LineGraphs => Lines.Values
        .Select(l => l.LineChart);

    public ChartLayoutViewModel()
    {
        Lines = new Dictionary<string, Line>();
        foreach (var stroke in Line.Strokes.Reverse())
        {
            Strokes.Push(stroke);
        }
        Strokes.Push(Brushes.Black);
    }
    public void AddLine(string name)
    {
        if(!name.Contains("Max") && !name.Contains("Min"))
        {
            Strokes.Pop();
        }
        Lines.Add(name,new(name, Strokes.Peek()));
        OnPropertyChanged();
    }
    public void UpdateData(Dictionary<string,double> data)
    {
        foreach (var (key,value) in data)
        {
            if(!Lines.TryGetValue(key, out var line)) continue;
            line.LineChart.Points.Add(new(line.HoursTicked, value));
        }
    }
}

public class Line
{
    public static SolidColorBrush[] Strokes = new[]
    {
        Brushes.Red,
        Brushes.Green,
        Brushes.Blue,
        Brushes.BlueViolet,
        Brushes.Cyan
    };
    public Line(string key, SolidColorBrush stroke)
    {
        LineChart = new LineGraph
        {
            Stroke = key.Contains("Max") 
                ? GetDarkerColor(stroke, 0.5) 
                : key.Contains("Min") 
                    ? GetLighterColor(stroke, 0.5) 
                    : stroke,
            Description = key,
            StrokeThickness = 2,
            IsManipulationEnabled = true
        };

        StartedAt = DateTime.Now;
        LegendVisibility = !key.Contains("Max") && !key.Contains("Min");
    }

    public LineGraph LineChart { get; set; }
    public DateTime StartedAt { get; set; }
    public bool LegendVisibility { get; set; }
    public double HoursTicked => (StartedAt-DateTime.Now).Hours;
    
    public SolidColorBrush GetDarkerColor(SolidColorBrush brush, double factor)
    {
        // Get the original color
        Color originalColor = brush.Color;

        // Adjust the brightness factor (0.0 to 1.0, where 0.0 is darkest and 1.0 is original color)
        double brightnessFactor = 1.0 - factor;

        // Create a new darker color
        Color darkerColor = Color.FromScRgb(originalColor.ScA,
            (float)(originalColor.ScR * brightnessFactor),
            (float)(originalColor.ScG * brightnessFactor),
            (float)(originalColor.ScB * brightnessFactor));

        // Create and return a new SolidColorBrush with the darker color
        return new SolidColorBrush(darkerColor);
    }
    
    public SolidColorBrush GetLighterColor(SolidColorBrush brush, double factor)
    {
        // Get the original color
        Color originalColor = brush.Color;

        // Adjust the brightness factor (0.0 to 1.0, where 0.0 is lightest and 1.0 is original color)
        double brightnessFactor = 1.0 + factor;

        // Create a new lighter color
        Color lighterColor = Color.FromScRgb(originalColor.ScA,
            (float)Math.Min(originalColor.ScR * brightnessFactor, 1.0),
            (float)Math.Min(originalColor.ScG * brightnessFactor, 1.0),
            (float)Math.Min(originalColor.ScB * brightnessFactor, 1.0));

        // Create and return a new SolidColorBrush with the lighter color
        return new SolidColorBrush(lighterColor);
    }
}

