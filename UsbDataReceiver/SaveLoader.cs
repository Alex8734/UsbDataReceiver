using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace UsbDataReceiver;

public static class SaveLoader
{
    public static string Dir = "config/";
    public static string DeviceFile = $"{Dir}devices.json";
    public static string IoDeviceFile = $"{Dir}io-devices.json";
    
    public static JsonSerializerOptions Options = new()
    {
        WriteIndented = true, // Set this to true for human-readable formatting
        IncludeFields = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues:true)
        }
    };
    
    public static List<JsonMeasuredDevice> LoadMeasuredDevices()
    {
        if (!File.Exists(DeviceFile)) return new List<JsonMeasuredDevice>();
        var file = File.ReadAllText(DeviceFile);
        var devices = JsonSerializer.Deserialize<JsonMeasuredDevice[]>(file, Options);
       
        return devices?.ToList() ?? new List<JsonMeasuredDevice>();
    }
    
    public static void SaveMeasuredDevices(List<MeasuredDevice> devices)
    {
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        var jsonDevices = 
            devices.Select(d => 
                new JsonMeasuredDevice(d.Name, d.Ports.Select(p => 
                    new JsonPortDescription(p.Id, p.Type, p.Device.Name, p.Name))
                    .ToList()))
                .ToList();
        
        var json = JsonSerializer.Serialize(jsonDevices, Options);
        File.WriteAllText(DeviceFile, json);
    } 
    
    public static void SaveIoDevices(List<IODevice> devices)
    {
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        var json = JsonSerializer.Serialize(devices, Options);
        File.WriteAllText(IoDeviceFile, json);
    }
    
    public static List<IODevice> LoadIoDevices()
    {
        if (!File.Exists(IoDeviceFile)) return new List<IODevice>();
        var file = File.ReadAllText(IoDeviceFile);
        var devices = JsonSerializer.Deserialize<IODevice[]>(file,Options);
        return devices?.ToList() ?? new List<IODevice>();
    }

}

public record JsonMeasuredDevice(string Name, List<JsonPortDescription> Ports);
public record JsonPortDescription (int Id, MeasurementType Type, string Device, string Name);