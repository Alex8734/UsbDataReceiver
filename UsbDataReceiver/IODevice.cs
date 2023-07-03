
namespace UsbDataReceiver;

// ReSharper disable once InconsistentNaming
public abstract class IODevice
{
    public List<int> AvailablePorts { get; }
    public string Name { get; set; }
    
    public IODevice(string name,int portsCount)
    {
        AvailablePorts = Enumerable.Range(0, portsCount).ToList();
        Name = name;
    }
    
    public int GetNextAvailablePort()
    {
        var port = AvailablePorts.First();
        AvailablePorts.Remove(port);
        return port;
    }
}