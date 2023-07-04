
namespace UsbDataReceiver;

// ReSharper disable once InconsistentNaming
public class IODevice
{
    public List<int> AvailablePorts { get; }
    /// <summary>
    ///     Device display-name of the IODevice
    /// </summary>
    public string Name { get;}
    
    public IODevice(string name,int portsCount)
    {
        AvailablePorts = Enumerable.Range(0, portsCount).ToList();
        Name = name;
    }
    
    public int GetNextAvailablePort()
    {
        if (AvailablePorts.Count == 0) return -1;
        var port = AvailablePorts.First();
        AvailablePorts.Remove(port);
        return port;
    }
}