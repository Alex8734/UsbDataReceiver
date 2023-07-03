using System.Collections.ObjectModel;
using NationalInstruments.DAQmx;
using NationalInstruments.Restricted;
using Task = NationalInstruments.DAQmx.Task;


namespace UsbDataReceiver;

public class MeasuredDevice
{
    public string Name { get; set; }
    public ReadOnlyCollection<PortDescription> Ports { get; }
    private readonly Task _measureTask = new();
    public AnalogMultiChannelReader Reader { get; }
    public MeasuredDevice(string name, IEnumerable<PortDescription> ports)
    {
        Name = name;
        Ports = ports.ToList().AsReadOnly();
        foreach (var port in Ports)
        {
            SetPortTask(port);
        }
        Reader = new AnalogMultiChannelReader(_measureTask.Stream);
    }

    private AIChannel? SetPortTask(PortDescription port)
    {
        if(!port.Device.AvailablePorts.Contains(port.Id)) return null;
        var aiChannel = _measureTask.AIChannels.CreateVoltageChannel(
            $"{Name}/ai{port.Id}",
            $"{port.Type}Channel{port.Id}",
            AITerminalConfiguration.Rse,
            0,
            10,
            AIVoltageUnits.Volts);
        return aiChannel;
    } 
}