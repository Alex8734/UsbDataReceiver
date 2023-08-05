using System.Collections.ObjectModel;
using System.Data;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using NationalInstruments.DAQmx;
using UsbDataReceiver.Extensions;
using Task = NationalInstruments.DAQmx.Task;

namespace UsbDataReceiver;

public class MeasuredDevice
{
    private DataManager _dataManager;
    public string Name { get; set; }
    /// <summary>
    ///  ad bool if all ports are connected
    /// </summary>
    [JsonIgnore]
    public bool IsConnected => Ports.All(p => IODevice.GetConnectedDevices()?.Select(d => d.DeviceID).Contains(p.Name) ?? false);
    public  IReadOnlyCollection<PortDescription> Ports { get; }
    private readonly Task _measureTask = new();
    private AnalogMultiChannelReader Reader { get; }
    /// <summary>
    ///  returns the current data of the device
    ///  it is updated with <see cref="DataManager"/> every <see cref="DataManager.UpdateInterval"/>
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, double> Data { get; } = new();
    /// <summary>
    ///   ad dictionary of queues for the floating mean ("gleitender Mittelwert") of each data
    /// </summary>
    public Dictionary<string, Queue<double>> FloatingMeanQueues;
    /// <summary>
    ///  the time when the queue was filled the first time
    /// </summary>
    private DateTime StartedFillingQueueAt { get; set; }
    /// <summary>
    ///  the time how log the queue should be filled
    ///  because if the queue is filled with constant 10 values it could be 10 values of the last 10 seconds or 10 values of the last 10 minutes
    /// </summary>
    public double MillisecondsToFillQueue { get; set; } = 400;
    /// <summary>
    ///  true if queues are filled for <see cref="MillisecondsToFillQueue"/>
    /// </summary>
    public bool QueueIsFull => (DateTime.Now - StartedFillingQueueAt) >= TimeSpan.FromMilliseconds(MillisecondsToFillQueue);
    /// <summary>
    /// creates a new MeasuredDevice like a Transponder with a list of ports
    /// </summary>
    /// <param name="name">the name of the device</param>
    /// <param name="ports">a list of ports of the device</param>
    [JsonConstructor]
    public MeasuredDevice(string name, IEnumerable<PortDescription> ports)
    {
        FloatingMeanQueues = new Dictionary<string, Queue<double>>();
        Name = name;
        _dataManager = new DataManager(this);
        StartedFillingQueueAt = DateTime.Now;
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
    public string GetDataAsString(int decimalPlaces = 10)
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
    /// <summary>
    ///   return the port of the device with the given name
    /// </summary>
    /// <param name="portName">the name of the port</param>
    /// <exception cref="DataException">if port not in list</exception>
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