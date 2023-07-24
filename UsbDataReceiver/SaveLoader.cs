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
        var ioDevs = new List<IODevice>();
        var normalDevices = devices.Select(d => new MeasuredDevice(
                d.Name,
                d.Ports.Select(p =>
                    new PortDescription(p.Id, p.Type, ioDevs.FirstOrDefault(i => i.Name == p.Device), p.Name))
                .ToList()))
            .ToList();
        return devices?.ToList() ?? new List<JsonMeasuredDevice>();
    }
    private static List<IODevice> ConfigureIoDevicesFromOutput(IEnumerable<JsonMeasuredDevice> devices)
    {
        var ioDevices = new List<IODevice>();
        foreach (var device in devices)
        {
            foreach (var port in device.Ports)
            {
                if (ioDevices.Any(d => d.Name == port.Device)) continue;
                ioDevices.Add(new IODevice(port.Device, new List<int>()));
            }
        }

        throw new NotImplementedException();
    }
    public static void SaveMeasuredDevices(List<MeasuredDevice> devices)
    {
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        var json = JsonSerializer.Serialize(devices, Options);
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
        var devices = JsonSerializer.Deserialize<IODevice[]>(file.ToString(),Options);
        return devices?.ToList() ?? new List<IODevice>();
    }

}

public class JsonMeasuredDevice
{
    public string Name { get; set; }
    public List<JsonPortDescription> Ports { get; set; }
}
public record JsonPortDescription (int Id, MeasurementType Type, string Device, string Name);