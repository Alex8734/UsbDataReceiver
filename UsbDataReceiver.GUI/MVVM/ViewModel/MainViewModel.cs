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
            UpdateDeviceList();
            return _ioDevices;
        }
    }

    public ObservableCollection<object> DisplayDevices 
        = new (Devices.Count != 0 
            ?  Devices.Select(d => new DeviceDisplayModel(d)) 
            : new List<Label>{NoDevicesLabel} );
    
    public RelayCommand DataDisplayCommand { get; set; }
    public RelayCommand AddDeviceCommand { get; set; }
    public RelayCommand BackButtonCommand { get; set; }

    public AddDeviceViewModel AddDeviceVM { get; set; }
    public AllDataDisplayViewModel AllDataDisplayVM { get; set; }
    
    private object _currentView = null!;
    private Visibility _backButtonVisibility;

    public object CurrentView
    {

        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public static void UpdateDeviceList()
    {
        var currentDevices = IODevice.GetConnectedDevices();
        if (currentDevices == null)
        {
            return;
        }

        // Find names of devices that exist in the old list but not in the new list
        var existingDeviceNames = _ioDevices.Select(device => device.Name);
        var newDeviceNames = currentDevices.Select(device => device.DeviceID);

        var namesToRemove = existingDeviceNames.Except(newDeviceNames).ToList();
        var namesToAdd = newDeviceNames.Except(existingDeviceNames).ToList();

        // Remove devices from the old list that don't exist in the new list.
        _ioDevices.RemoveAll(device => namesToRemove.Contains(device.Name));

        // Add new devices to the old list.
        _ioDevices.AddRange(currentDevices
            .Where(device => namesToAdd.Contains(device.DeviceID))
            .Select(d => new IODevice(d.DeviceID,d.AIPhysicalChannels.Length)));
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
        AddDeviceVM = new AddDeviceViewModel();
        //AllDataDisplayVM = new AllDataDisplayViewModel();
        CurrentView = AddDeviceVM;
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
            //CurrentView = AllDataDisplayVM;
            BackButtonVisibility = Visibility.Hidden;
        });

    }
}