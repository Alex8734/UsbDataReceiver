using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.Log;
using UsbDataReceiver;
using UsbDataReceiver.GUI.MVVM.Model;
using System.Windows.Media;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using UsbDataReceiver.GUI.MVVM.View;
using Label = System.Windows.Controls.Label;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public static readonly Label NoDevicesLabel = new()
    {
        Content = "No Transponders"
    };
    
    public static readonly Label NoLogsLabel = new()
    {
        Content = "No Logs"
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

    public static List<MeasuredDevice> Devices { get; private set; } = new();

    private static List<IODevice> _ioDevices = new();
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

    public ObservableCollection<object> DeviceLogs
    {
        get
        {
            var devices = Directory.GetDirectories(LogManager.LogPath).Where(d => Directory.GetFiles(d).Any(f => f.EndsWith(".csv"))).ToList();
            return new ObservableCollection<object>(
                devices.Count > 0 
                    ? devices.Select(d => new LogDisplayModel(d.Split("\\").Last())) 
                    : new[]{NoLogsLabel});
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
    public RelayCommand SelectLogFolderCommand { get; set; }
    public RelayCommand DataDisplayCommand { get; set; }
    public RelayCommand AddDeviceCommand { get; set; }
    public RelayCommand BackButtonCommand { get; set; }

    public AddDeviceViewModel AddDeviceVM { get; set; }
    public DataOverviewViewModel AllDataDisplayVM { get; set; }
    
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
        AllDataDisplayVM = new DataOverviewViewModel();
        AddDeviceVM = new AddDeviceViewModel();
        
        CurrentView = AllDataDisplayVM;
        BackButtonVisibility = Visibility.Hidden;
        
        //init Commands
        SelectLogFolderCommand = new RelayCommand(o =>
        {
            var dirDialog = new FolderBrowserDialog();
            dirDialog.SelectedPath = LogManager.LogPath;
            var d = dirDialog.ShowDialog();
            if (d != DialogResult.OK) return;
            
            LogManager.LogPath = dirDialog.SelectedPath;
            OnPropertyChanged(nameof(DeviceLogs));
        });
        
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