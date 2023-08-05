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
    /// <summary>
    ///   creates a new IODevice with the given name and the given amount of ports
    ///   the ports are numbered from 0 to portsCount
    ///   it represents a National Instruments device
    /// </summary>
    /// <param name="name">the device name on your computer default its "Dev1-.."</param>
    /// <param name="portsCount">how many input ports the device has</param>
    public IODevice(string name, int portsCount)
    {
        _availablePorts = Enumerable.Range(0, portsCount).ToList();
        Name = name;
    }
    /// <summary>
    ///  creates a new IODevice with the given name and the given available ports
    /// </summary>
    /// <param name="name">the device name on your computer default its "Dev1-.."</param>
    /// <param name="availablePorts">a list of all available ports on the device</param>
    public IODevice(string name, List<int> availablePorts)
    {
        _availablePorts = availablePorts;
        Name = name;
    } 
    /// <summary>
    ///  gets you the next available port and removes it from the list
    ///  if still available add to the list manually
    /// </summary>
    /// <returns>null if not available otherwise the next available port </returns>
    public int? GetNextAvailablePort()
    {
        if (AvailablePorts.Count == 0) return null;
        var port = AvailablePorts.First();
        AvailablePorts.Remove(port);
        return port;
    }
    /// <summary>
    ///  returns all connected devices to your computer also the virtual ones
    /// </summary>
    /// <returns> a list of <see cref="Device"/> from the DAQmx library </returns>
    public static List<Device>? GetConnectedDevices()
    {
        var devices = DaqSystem.Local.Devices;
        return devices?.Select(d => DaqSystem.Local.LoadDevice(d)).ToList();
    }
}