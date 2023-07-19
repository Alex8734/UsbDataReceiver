using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UsbDataReceiver.GUI.MVVM.ViewModel;

namespace UsbDataReceiver.GUI.MVVM.View;

public partial class AddDeviceView : UserControl
{
    public AddDeviceView()
    {
        InitializeComponent();
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
    private void AddDevice_Click(object sender, RoutedEventArgs e)
    {
        
    }
}