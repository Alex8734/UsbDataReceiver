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
        

        public string? SelectedIoDevice
        {
            get => _selectedIoDevice;
            set
            {
                var ioDev = MainViewModel.IoDevices.FirstOrDefault(d => d.Name == _selectedIoDevice);
                
                if (ioDev is not null && _selectedPort is not null)
                {
                    ioDev.AvailablePorts.Add((int)_selectedPort);
                    _selectedPort = null;
                }
                _selectedIoDevice = value ;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PortsOfSelectedDevice));
                OnPropertyChanged(nameof(SelectedPort));
            }
        }

        private int? _selectedPort; 

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
                ioDev.AvailablePorts.Remove((int)value);
            }
        }
        public List<Device>? AvailableNiDevices => IODevice.GetConnectedDevices();
        public List<string> AvailableNiDeviceNames =>
            MainViewModel.IoDevices
                .Where(d => d.AvailablePorts.Count > 0)
                .Select(x => x.Name).ToList();

        public List<string> MeasurementTypes => Enum.GetNames(typeof(MeasurementType)).ToList();
        
        public List<string> PortsOfSelectedDevice
        {
            get
            {
                var ports = MainViewModel.IoDevices
                    .FirstOrDefault(d => d.Name == SelectedIoDevice)?.AvailablePorts
                    .Select(p => p.ToString()).ToList() ?? new List<string>();
                if(SelectedPort is not null)
                {
                    ports.Add(SelectedPort.ToString()!);
                    ports.Sort();
                }
                return ports;
            }
        }

        public PortLayoutViewModel()
        {
            PortName = string.Empty;
            SelectedIoDevice = AvailableNiDeviceNames.FirstOrDefault();
            
        }
    }
}
