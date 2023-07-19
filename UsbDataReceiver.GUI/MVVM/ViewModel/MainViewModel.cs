using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.Log;
using UsbDataReceiver;
namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{

    public static List<MeasuredDevice> Devices { get; } = new();

    public static List<IODevice> IoDevices { get; } 
        = IODevice.GetConnectedDevices()?.Select(d => new IODevice(d.DeviceID, d.AIPhysicalChannels.Length)).ToList() ?? new List<IODevice>();

    public RelayCommand StartLoggingDeviceCommand { get; set; }
    public RelayCommand DataDisplayCommand { get; set; }
    public RelayCommand AddDeviceCommand { get; set; }
    public AddDeviceViewModel AddDeviceVM { get; set; }
    public DataDisplayViewModel DataDisplayVM { get; set; }
    
    private object _currentView = null!;
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
        foreach (var currentDevice in currentDevices)
        {
            // Find the matching device in the oldDeviceList based on Name
            var existingDevice = IoDevices.FirstOrDefault(d => d.Name == currentDevice.DeviceID);

            if (existingDevice != null)
            {
                // Update the Name property of the existing device with the newer name
                //existingDevice.Name = newDevice.Name;
                // You can also update other properties of the existing device here if needed
                // For example: existingDevice.Property = newDevice.Property;
            }
            else
            {
                // If the device does not exist in oldDeviceList, add it to the list
                IoDevices.Add(new IODevice(currentDevice.DeviceID,currentDevice.AIPhysicalChannels.Length));
            }
        }

        var devicesToRemove = from oldDevice in IoDevices 
            join newDevice in currentDevices 
            on oldDevice.Name equals newDevice.DeviceID into matchingDevices
            from match in matchingDevices.DefaultIfEmpty()
            where match == null
            select oldDevice;

        // Remove devices from oldDeviceList that don't exist in newDeviceList.
        foreach (var deviceToRemove in devicesToRemove.ToList())
        {
            IoDevices.Remove(deviceToRemove);
        }

    }
    public MainViewModel()
    {
        AddDeviceVM = new AddDeviceViewModel();
        DataDisplayVM = new DataDisplayViewModel();
        CurrentView = DataDisplayVM;

        DataDisplayCommand = new RelayCommand(o =>
        {
            CurrentView = DataDisplayVM;
        });
        
        AddDeviceCommand = new RelayCommand(o =>
        {
            CurrentView = AddDeviceVM;
        });
        
        StartLoggingDeviceCommand = new RelayCommand(o =>
        {
            if (o is MeasuredDevice device)
            {
                LogManager.StartLoggingDevice(device);
            }
        });
        
    }
}