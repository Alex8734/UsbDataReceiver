using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UsbDataReceiver.GUI.Core;

namespace UsbDataReceiver.GUI.MVVM.ViewModel
{

    class PortLayoutViewModel : ObservableObject
    {
        //public List<PortModel> Ports { get; set; } = new();
        private string? _selectedIoDevice;

        public string? SelectedIoDevice
        {
            get => _selectedIoDevice;
            set
            {
                _selectedIoDevice = value;
                OnPropertyChanged(nameof(PortsOfSelectedDevice));
            }
        }
        public List<Device>? AvailableNiDevices => IODevice.GetConnectedDevices();
        public List<string> AvailableNiDeviceNames => AvailableNiDevices?.Select(d => d.DeviceID.ToString()).ToList() ?? new List<string>();
        public List<string> MeasurementTypes => Enum.GetNames(typeof(MeasurementType)).ToList();

        public List<string> PortsOfSelectedDevice => MainViewModel.IoDevices
            .FirstOrDefault(d => d.Name == SelectedIoDevice)?.AvailablePorts
            .Select(p => p.ToString()).ToList() ?? new List<string>();

        
    }
}
