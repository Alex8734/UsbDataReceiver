using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace UsbDataReceiver;
/// <summary>
///  to load and save the devices
///  dont need iodevices anymore because they are updated manually
/// </summary>
public static class SaveLoader
{
    public static string Dir = "config/";
    public static string DeviceFile = $"{Dir}devices.json";
    /// <summary>
    ///  options for the json serializer
    /// </summary>
    public static JsonSerializerOptions Options = new()
    {
        WriteIndented = true, // Set this to true for human-readable formatting
        IncludeFields = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues:true)
        }
    };
    /// <summary>
    ///  loads all devices from the config folder
    /// </summary>
    /// <returns></returns>
    public static List<JsonMeasuredDevice> LoadMeasuredDevices()
    {
        if (!File.Exists(DeviceFile)) return new List<JsonMeasuredDevice>();
        var file = File.ReadAllText(DeviceFile);
        var devices = JsonSerializer.Deserialize<JsonMeasuredDevice[]>(file, Options);
       
        return devices?.ToList() ?? new List<JsonMeasuredDevice>();
    }
    /// <summary>
    ///  saves all devices to the config folder
    /// </summary>
    /// <param name="devices"></param>
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
    

}

public record JsonMeasuredDevice(string Name, List<JsonPortDescription> Ports);
public record JsonPortDescription (int Id, MeasurementType Type, string Device, string Name);