using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NI.Data;
using UsbDataReceiver.GUI.MVVM.View;
using UsbDataReceiver.GUI.MVVM.ViewModel;
using UsbDataReceiver.Log;

namespace UsbDataReceiver.GUI.MVVM.Model;

public sealed class DeviceDisplayModel : Border
{
    private readonly Grid _layoutGrid;
    private Label _deviceLabel;
    private Button _loggingButton;
    public MeasuredDevice RepresentedDevice { get; }
    public DataDisplayView DataDisplay { get; set; }
    public DataDisplayViewModel? DataDisplayVM => DataDisplay.DataContext as DataDisplayViewModel;
    public DeviceDisplayModel(MeasuredDevice device)
    {
        RepresentedDevice = device;
        DataDisplay = new DataDisplayView();

        var vm = DataContext as MainViewModel;
        BorderBrush = Brushes.Gray;
        BorderThickness = new Thickness(0.2);
        MouseEnter += Device_MouseEnter;
        MouseLeave += Device_MouseLeave;
        
        _layoutGrid = new Grid
        {
            Margin = new Thickness(3)
        };

        _layoutGrid.MouseLeftButtonDown += Device_Click;

        _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(130) });
        _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition());
    
        _layoutGrid.RowDefinitions.Add(new RowDefinition());
        _layoutGrid.RowDefinitions.Add(new RowDefinition());
    
        _deviceLabel = new Label
        {
            Foreground = Brushes.Gray,
            Padding = new Thickness(2, 0, 2, 0),
            FontWeight = FontWeights.SemiBold,
            Content = device.Name,
        };
        Grid.SetColumn(_deviceLabel, 0);
    
        var deviceInfoLabel = new Label
        {
            Foreground = Brushes.Gray,
            Padding = new Thickness(10, 0, 0, 0),
            FontSize = 10,
            Content = string.Join(" / ", device.Ports.Select(p => p.Id))
        };
        Grid.SetRow(deviceInfoLabel, 1);
        Grid.SetColumn(deviceInfoLabel, 0);
    
        _loggingButton = new Button
        {
            Background = Brushes.Transparent,
            Content = "▶",
            BorderThickness = new Thickness(0.6),
            Foreground = Brushes.Lime,
            FontSize = 10,
            FontWeight = FontWeights.Bold,
            Height = 18,
            Width = 18,

        };
        _loggingButton.Click += StartLoggingDevice_Click;
        Grid.SetRow(_loggingButton, 0);
        Grid.SetColumn(_loggingButton, 1);
    
        _layoutGrid.Children.Add(_deviceLabel);
        _layoutGrid.Children.Add(deviceInfoLabel);
        _layoutGrid.Children.Add(_loggingButton);
    
        Child = _layoutGrid;
        Thread.Sleep(1000);
        DataDisplayVM?.SetDevice(device);
    }

    private void StartLoggingDevice_Click(object sender, RoutedEventArgs e)
    {
        if(sender is not Button btn) return;
        switch (btn.Content)
        {
            case "▶":
                btn.Content = "■";
                btn.Foreground = Brushes.Red;
                LogManager.StartLoggingDevice(RepresentedDevice);
                break;
            case "■":
                btn.Content = "▶";
                btn.Foreground = Brushes.Lime;
                LogManager.StopLoggingDevice(RepresentedDevice.Name);
                break;
        }
        
        
    }
    private void Device_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        vm.BackButtonVisibility = Visibility.Visible;
        vm.CurrentView = DataDisplay;

    }
    private void Device_MouseEnter(object sender, MouseEventArgs e)
    {
        if (sender is not Border border) return;
        border.BorderBrush = Brushes.WhiteSmoke;
    }

    private void Device_MouseLeave(object sender, MouseEventArgs e)
    {
        if (sender is not Border border) return;
        border.BorderBrush = Brushes.Gray;
    }
}