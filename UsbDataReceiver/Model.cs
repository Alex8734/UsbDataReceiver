using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsbDataReceiver;


public record PortDescription (int Id, MeasurementType Type, IODevice Device, string Name);

public record MeasurementData(DateTime TimeStamp, Dictionary<string,double> Data);

public record DeviceLogs(string DeviceName, List<(string LogName, List<MeasurementData>)> Logs);

public enum MeasurementType
{
    Voltage,
    Ampere,
    Temperature
}