using System.Collections.ObjectModel;
using System.Data;
using System.Linq.Expressions;
using NationalInstruments.DAQmx;
using UsbDataReceiver.Extensions;
using Task = NationalInstruments.DAQmx.Task;

namespace UsbDataReceiver;

public class MeasuredDevice
{
    private DataManager _dataManager;
    public string Name { get; set; }
    public  IReadOnlyCollection<PortDescription> Ports { get; }
    private readonly Task _measureTask = new();
    
    private AnalogMultiChannelReader Reader { get; }
    public Dictionary<string, double> Data { get; } = new();
    
    public MeasuredDevice(string name, IEnumerable<PortDescription> ports)
    {
        Name = name;
        _dataManager = new DataManager(this);
        Ports = ports.ToList().AsReadOnly();
        foreach (var port in Ports)
        {
            SetPortTask(port);
        }
        Reader = new AnalogMultiChannelReader(_measureTask.Stream);
        
    }


    /// <summary>
    ///     Read data from directly from Devices ports
    /// </summary>
    /// <returns>Dictionary with Key = name of Data-Port</returns>
    /// <exception cref="DataException">if two ore more Data-Ports have same Name</exception>
    public Dictionary<string,double>? ReadData()
    {
        if (Reader is null) return null;
        var rareData = Reader.ReadSingleSample();
        if (rareData is null) return null;
        var data = new Dictionary<string, double>();
        for (var i = 0; i < Ports.Count; i++)
        {
            if(data.ContainsKey(Ports.ElementAt(i).Name)) throw new DataException($"data already contains key name {Ports.ElementAt(i).Name}");
            
            data.Add(Ports.ElementAt(i).Name, rareData[i].CalculateToType(Ports.ElementAt(i).Type));
        }
        return data;
    }
    /// <summary>
    ///     return data as string
    /// </summary>
    /// <param name="decimalPlaces">digits after comma</param>
    /// <returns>data as string: "<see cref="MeasurementType"/>:xxx,xx;<see cref="MeasurementType"/>:xxx,xx"</returns>
    /// <exception cref="DataException">if ReadSingleSample failed</exception>
    public string GetDataAsString(int decimalPlaces = 2)
    {
        return string.Join(";", Data.Select(d => $"{d.Value.Round(decimalPlaces)}"));
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