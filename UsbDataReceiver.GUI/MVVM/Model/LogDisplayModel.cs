using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UsbDataReceiver.GUI.MVVM.View;
using UsbDataReceiver.GUI.MVVM.ViewModel;
using UsbDataReceiver.Log;
using Label = System.Windows.Controls.Label;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace UsbDataReceiver.GUI.MVVM.Model;

public class LogItemDisplayModel : LogDisplayModel
{
    private List<MeasurementData> _logs;
    public LogItemDisplayModel(string logName, List<MeasurementData> logs) : base(logName)
    {
        _logs = logs;
    }

    public override void LogDisplay_Click(object sender, MouseButtonEventArgs e)
    {
        if(DataContext is not LogViewModel vm) return;
        var view = new DataDisplayView();
        if(view.DataContext is not DataDisplayViewModel dvm) return;
        dvm.SetData(_logs, TitleName);
        vm.CurrentView = view;

    }
}

public class LogDisplayModel : Border
{
    public string TitleName { get; set; }
    
    public LogDisplayModel(string deviceName)
    {
        
        TitleName = deviceName;
        var text = new Label();
        Child = text;
        text.Foreground = Brushes.Gray;
        text.Content = TitleName;
        MouseLeftButtonDown += LogDisplay_Click;
        MouseEnter += LogDisplay_MouseEnter;
        MouseLeave += LogDisplay_MouseLeave;
    }
    
    public virtual void LogDisplay_Click(object sender, MouseButtonEventArgs e)
    {
        if(DataContext is not MainViewModel vm) return;
        var devLogs = LogManager.ReadDeviceLogs($"{LogManager.LogPath}/{TitleName}");
        if(devLogs is null) return;
        var view = new LogView(devLogs);
        if(view.DataContext is not LogViewModel lvm) return;
        lvm.InitDevicesLogs(devLogs);
        vm.CurrentView = view;
        vm.BackButtonVisibility = Visibility.Visible;
        
    }
    private void LogDisplay_MouseEnter(object sender, MouseEventArgs e)
    {
        if (sender is not Border border) return;
        border.BorderBrush = Brushes.LightGray;
        border.BorderThickness = new Thickness(0.4);
    }
    private void LogDisplay_MouseLeave(object sender, MouseEventArgs e)
    {
        if (sender is not Border border) return;
        border.BorderBrush = Brushes.LightGray;
        border.BorderThickness = new Thickness(0);
    }
}