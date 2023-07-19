using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using NationalInstruments.DAQmx;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver;
using UsbDataReceiver.GUI.MVVM.View;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class AddDeviceViewModel
{
    
    public List<PortLayoutView> PortLayoutViews { get; set; }
    private string _deviceName = string.Empty;
    public string DeviceName
    {
        get => _deviceName;
        set
        {
            if(MainViewModel.Devices.Any(d => d.Name == value)) return;
            _deviceName = value;
        }
    }
    public RelayCommand NameCommand { get; }

    public AddDeviceViewModel()
    {
        PortLayoutViews = new List<PortLayoutView>
        {
            new()
        };
        NameCommand = new RelayCommand(o =>
        {
            Keyboard.ClearFocus();
        });
    }
}