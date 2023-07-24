using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.Log;
using UsbDataReceiver;
using UsbDataReceiver.GUI.MVVM.Model;
using System.Windows.Media;
using System.Windows;
using UsbDataReceiver.GUI.MVVM.View;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public static readonly Label NoDevicesLabel = new()
    {
        Content = "No Transponders"
    };

    public Visibility BackButtonVisibility
    {
        get => _backButtonVisibility;
        set
        {
            _backButtonVisibility = value;
            OnPropertyChanged();
        }
    }

    public static List<MeasuredDevice> Devices { get; } = new();

    private static List<IODevice> _ioDevices = IODevice.GetConnectedDevices()?
                                                   .Select(d => new IODevice(d.DeviceID, d.AIPhysicalChannels.Length))
                                                   .ToList()
                                               ?? new List<IODevice>();

    public static List<IODevice> IoDevices
    {
        get
        {
            var connectedDevices = IODevice.GetConnectedDevices();
            if (connectedDevices == null) return _ioDevices;
            _ioDevices.UpdateList(connectedDevices, 
                old => old.Name, 
                newItem => newItem.DeviceID,
                newItem => new IODevice(newItem.DeviceID, newItem.AIPhysicalChannels.Length));
            return _ioDevices;
        }
    }

    public ObservableCollection<object> DisplayDevices
    {
        get
        {
            if(Devices.Count == 0) return new (new List<Label>{NoDevicesLabel});
            var devices = Devices.Select(d => d.Name).ToList();
            var newDevices = _ioDevices.Select(d => d.Name).ToList();
            if(devices.Equals(newDevices)) return _displayDevices;
            _displayDevices.UpdateList(Devices, 
                old =>
                {
                    var dev = old as DeviceDisplayModel;
                    return dev?.Name ?? "";
                }, 
                newItem => newItem.Name,
                newItem => new DeviceDisplayModel(newItem));
            return _displayDevices;
        }
    }

    public RelayCommand DataDisplayCommand { get; set; }
    public RelayCommand AddDeviceCommand { get; set; }
    public RelayCommand BackButtonCommand { get; set; }

    public AddDeviceViewModel AddDeviceVM { get; set; }
    public AllDataDisplayViewModel AllDataDisplayVM { get; set; }
    
    private object _currentView = null!;
    private Visibility _backButtonVisibility;
    private readonly ObservableCollection<object> _displayDevices = new (Devices.Count != 0 
        ?  Devices.Select(d => new DeviceDisplayModel(d)) 
        : new List<Label>{NoDevicesLabel} );

    public object CurrentView
    {

        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void AddDevice(MeasuredDevice device)
    {
        Devices.Add(device);
        //AllDataDisplayVM.AddDevice(device);
        OnPropertyChanged(nameof(DisplayDevices));
    }
    public MainViewModel()
    {
        
        //init Views
        AllDataDisplayVM = new AllDataDisplayViewModel();
        AddDeviceVM = new AddDeviceViewModel();
        //AllDataDisplayVM = new AllDataDisplayViewModel();
        
        CurrentView = AllDataDisplayVM;
        BackButtonVisibility = Visibility.Hidden;
        
        //init Commands
        DataDisplayCommand = new RelayCommand(o =>
        {
            //CurrentView = AllDataDisplayVM;
        });
        AddDeviceCommand = new RelayCommand(o =>
        {
            CurrentView = AddDeviceVM;
        });
        
        BackButtonCommand = new RelayCommand(o =>
        {
            CurrentView = AllDataDisplayVM;
            BackButtonVisibility = Visibility.Hidden;
        });

    }
}