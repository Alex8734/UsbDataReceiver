using System.Reflection;

namespace UsbDataReceiver;

public record PortDescription (int Id, MeasurementType Type, IODevice Device, string Name);

public enum MeasurementType
{
    Voltage,
    Ampere,
    Temperature
}