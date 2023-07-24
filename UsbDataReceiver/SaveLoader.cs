using System.Text.Json;
using System.Text.Json.Nodes;

namespace UsbDataReceiver;

public static class SaveLoader
{
    public static string Dir = "config/";
    public static string DeviceFile = $"{Dir}devices.json";
    public static string IoDeviceFile = $"{Dir}io-devices.json";
    
    public static JsonSerializerOptions Options = new()
    {
        WriteIndented = true // Set this to true for human-readable formatting
    };
    
    public static List<MeasuredDevice> LoadMeasuredDevices()
    {
        if (!File.Exists(DeviceFile)) return new List<MeasuredDevice>();
        var file = File.ReadAllText(DeviceFile);
        var devices = JsonSerializer.Deserialize<List<MeasuredDevice>>(file, Options);
        return devices ?? new List<MeasuredDevice>();
    }
    
    public static void SaveMeasuredDevices(List<MeasuredDevice> devices)
    {
        var json = JsonSerializer.Serialize(devices, Options);
        File.WriteAllText(DeviceFile, json);
    } 
    
    public static void SaveIoDevices(List<IODevice> devices)
    {
        var json = JsonSerializer.Serialize(devices, Options);
        File.WriteAllText(IoDeviceFile, json);
    }
    
    public static List<IODevice> LoadIoDevices()
    {
        if (!File.Exists(IoDeviceFile)) return new List<IODevice>();
        var file = File.ReadAllText(IoDeviceFile);
        var devices = JsonSerializer.Deserialize<List<IODevice>>(file);
        return devices ?? new List<IODevice>();
    }

}