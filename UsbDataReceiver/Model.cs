using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsbDataReceiver;


public record PortDescription (int Id, MeasurementType Type, IODevice Device, string Name);



public enum MeasurementType
{
    Voltage,
    Ampere,
    Temperature
}