using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UsbDataReceiver.GUI.Core;

namespace UsbDataReceiver.GUI.MVVM.ViewModel
{

    class PortLayoutViewModel : ObservableObject
    {
        //public List<PortModel> Ports { get; set; } = new();
        private string? _selectedIoDevice;
        private int? _selectedPort;

        public string? SelectedIoDevice
        {
            get => _selectedIoDevice;
            set
            {
                _selectedIoDevice = value ;
                OnPropertyChanged(nameof(PortsOfSelectedDevice));
            }
        }

        public string PortName { get; set; }
        public MeasurementType SelectedPortType { get; set; }

        public int? SelectedPort
        {
            get => _selectedPort;
            set
            {
                if (SelectedIoDevice is null || value is null) return;
                var ioDev = MainViewModel.IoDevices.FirstOrDefault(d => d.Name == SelectedIoDevice);
                if (ioDev == null) return;
                if (_selectedPort is not null)
                {
                    ioDev.AvailablePorts.Add((int)_selectedPort);
                }

                _selectedPort = value;
                OnPropertyChanged(nameof(PortsOfSelectedDevice));
                ioDev.AvailablePorts.Remove((int)value);
            }
        }
        public List<Device>? AvailableNiDevices => IODevice.GetConnectedDevices();
        public List<string> AvailableNiDeviceNames => MainViewModel.IoDevices
            .Where(d => d.AvailablePorts.Count > 0)
            .Select(x => x.Name).ToList();
        public List<string> MeasurementTypes => Enum.GetNames(typeof(MeasurementType)).ToList();

        public List<string> PortsOfSelectedDevice => MainViewModel.IoDevices
            .FirstOrDefault(d => d.Name == SelectedIoDevice)?.AvailablePorts
            .Select(p => p.ToString()).ToList() ?? new List<string>();

        public PortLayoutViewModel()
        {

        }
    }
}
