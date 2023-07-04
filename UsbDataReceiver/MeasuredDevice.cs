using System.Collections.ObjectModel;
using System.Data;
using NationalInstruments.DAQmx;
using UsbDataReceiver.Extensions;
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
    /// <summary>
    ///     Read data from Devices ports
    /// </summary>
    /// <returns>Dictionary with Key = name of Data-Port</returns>
    /// <exception cref="DataException">if two ore more Data-Ports have same Name</exception>
    public Dictionary<string,double>? ReadData()
    {
        var rareData = Reader.ReadSingleSample();
        if (rareData is null) return null;
        var data = new Dictionary<string, double>();
        for (var i = 0; i < Ports.Count; i++)
        {
            if(data.ContainsKey(Ports[i].Name)) throw new DataException($"data already contains key name {Ports[i].Name}");
            data.Add(Ports[i].Name, rareData[i]);
        }

        return data;
    }
    /// <summary>
    ///     return data as string
    /// </summary>
    /// <param name="decimalPlaces">digits after comma</param>
    /// <returns>data as string: "<see cref="MeasurementType"/>:xxx,xx;<see cref="MeasurementType"/>:xxx,xx"</returns>
    /// <exception cref="DataException">if ReadSingleSample failed</exception>
    public string ReadDataString(int decimalPlaces = 2)
    {
        var data = ReadData();
        if (data is null) throw new DataException("error by reading data");
        return string.Join(";", data.Select(d => $"{d.Key}:{d.Value.Round(decimalPlaces)}"));
    }
    
    private AIChannel? SetPortTask(PortDescription port)
    {
        var aiChannel = _measureTask.AIChannels.CreateVoltageChannel(
            $"{port.Device.Name}/ai{port.Id}",
            $"{port.Type}Channel{port.Id}",
            AITerminalConfiguration.Rse,
            0,
            10,
            AIVoltageUnits.Volts);
        return aiChannel;
    } 
    
    public PortDescription this[string portName]
    {
        get
        {
            var port = Ports.FirstOrDefault(p => p.Name == portName);
            if (port is null) throw new DataException($"port with name {portName} not found");
            return port;
        }    
    }
}