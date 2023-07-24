using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using NationalInstruments.DAQmx;

public class IODevice
{
    private List<int> _availablePorts;
    
    public List<int> AvailablePorts
    {
        get
        {
            _availablePorts.Sort();
            return _availablePorts;
        }
        private init => _availablePorts = value;
    }

    /// <summary>
    ///     Device display-name of the IODevice
    /// </summary>
    public string Name { get; }

    public IODevice(string name, int portsCount)
    {
        _availablePorts = Enumerable.Range(0, portsCount).ToList();
        Name = name;
    }
    public IODevice(string name, List<int> availablePorts)
    {
        _availablePorts = availablePorts;
        Name = name;
    } 
    
    public int? GetNextAvailablePort()
    {
        if (AvailablePorts.Count == 0) return null;
        var port = AvailablePorts.First();
        AvailablePorts.Remove(port);
        return port;
    }
    
    public static List<Device>? GetConnectedDevices()
    {
        var devices = DaqSystem.Local.Devices;
        return devices?.Select(d => DaqSystem.Local.LoadDevice(d)).ToList();
    }
}