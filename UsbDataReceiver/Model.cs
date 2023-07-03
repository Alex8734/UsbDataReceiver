namespace UsbDataReceiver;

public abstract record PortDescription (int Id, MeasurementType Type, IODevice Device);

public enum MeasurementType
{
    Voltage,
    Ampere,
    Temperature
}