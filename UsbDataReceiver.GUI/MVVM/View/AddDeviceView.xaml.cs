using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using UsbDataReceiver.GUI.Core;
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
        if (DataContext is not AddDeviceViewModel vm) return;
        if (outerNameBox.Template.FindName("NameBox",outerNameBox) is not TextBox innerTbx) return;

        var devName = innerTbx.Text;
        var ports = new List<PortDescription>();
        foreach (var view in vm.PortLayoutViews)
        {
            if (view.DataContext is not PortLayoutViewModel portVm) continue;
            var selectedIoDev = MainViewModel.IoDevices.FirstOrDefault(d => d.Name == portVm.SelectedIoDevice);
            if(selectedIoDev == null || portVm.SelectedPort is null) continue;
            ports.Add(new PortDescription(
                    (int)portVm.SelectedPort,
                    portVm.SelectedPortType, 
                    selectedIoDev,
                    portVm.PortName));
        }

        var newDev = new MeasuredDevice(devName, ports);
        var main = this.FindParent<MainWindow>();
        if(main?.DataContext is not MainViewModel mainVm) return;
        mainVm.AddDevice(newDev);
        mainVm.CurrentView = mainVm.AllDataDisplayVM;
        SaveLoader.SaveMeasuredDevices(MainViewModel.Devices);
    }

    private void Placeholder_OnGotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox {Text: "New Device"} tbx)
        {
            tbx.Text = "";
        }
    }

    private void PlaceHolderAdd_OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox tbx) return;
        if (tbx.Template.FindName("NameBox", tbx) is not TextBox innerTbx) return;
        
        if (innerTbx.Text.Length <1)
        {
            tbx.Text = "New Device";
        }

    }

    private void AddPort_Event(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AddDeviceViewModel vm) return;
        vm.AddPort();
    }
    
    private void FocusAddPort_Event(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AddDeviceViewModel vm) return;
        var newPort = vm.AddPort();
        
        switch(e.Source)
        {
            case TextBox{Name:"PortNameDummy"}:
                newPort.PortName.Focus();
                Keyboard.Focus(newPort.PortName);
                break;
            case ComboBox{Name:"MeasurementTypeDummy"}:
                newPort.TypeSelector.Focus();
                Keyboard.Focus(newPort.TypeSelector);
                break;
            case ComboBox{Name:"NiDeviceDummy"}:
                newPort.NiDeviceSelector.Focus();
                Keyboard.Focus(newPort.NiDeviceSelector);
                break;
            case ComboBox{Name:"PortSelectorDummy"}:
                newPort.PortSelector.Focus();
                Keyboard.Focus(newPort.PortSelector);
                break;
            
        }
    }   
}