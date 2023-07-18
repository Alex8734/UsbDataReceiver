using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UsbDataReceiver.GUI.MVVM.ViewModel;

namespace UsbDataReceiver.GUI.MVVM.Model;

public sealed class DeviceDisplayModel : Border
{
    private readonly Grid _layoutGrid;
    private Label _deviceLabel;
    private Button _loggingButton;
    public DeviceDisplayModel(MeasuredDevice device)
    {
        var vm = DataContext as MainViewModel;
        BorderBrush = Brushes.Gray;
        BorderThickness = new Thickness(0.2);
        MouseEnter += Device_MouseEnter;
        MouseLeave += Device_MouseLeave;
    
        _layoutGrid = new Grid
        {
            Margin = new Thickness(3)
        };

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
            // Assuming that the AddDeviceCommand property is defined in the class that uses this control.
                
        };
        if (vm is not null)
            _loggingButton.Command = vm.StartLoggingDeviceCommand;
        Grid.SetRow(_loggingButton, 0);
        Grid.SetColumn(_loggingButton, 1);
    
        _layoutGrid.Children.Add(_deviceLabel);
        _layoutGrid.Children.Add(deviceInfoLabel);
        _layoutGrid.Children.Add(_loggingButton);
    
        Child = _layoutGrid;
    }
    private void Device_MouseEnter(object sender, MouseEventArgs e)
    {
        var border = sender as Border;
        if (border is null) return;
        border.BorderBrush = Brushes.WhiteSmoke;
    }

    private void Device_MouseLeave(object sender, MouseEventArgs e)
    {
        var border = sender as Border;
        if (border is null) return;
        border.BorderBrush = Brushes.Gray;
    }
}