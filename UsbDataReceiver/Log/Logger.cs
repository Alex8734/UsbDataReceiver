namespace UsbDataReceiver.Log;

public class Logger
{
    public string LogFilePath { get; }
    public string LogFileDir { get; }
    public MeasuredDevice Device { get; }
    private StreamWriter? _writer;
    public string LogDetails { get; }
    public Logger(MeasuredDevice device, string logPath, string logDetails)
    {
        Device = device;
        var currentTime = DateTime.Now;
        var currentDate = DateOnly.FromDateTime(currentTime);
        
        LogDetails = logDetails;
        LogFileDir = $"{logPath}/{Device.Name}";

        LogFilePath = $"{LogFileDir}/{currentDate:yyyy-MM-dd}-{LogDetails}.csv";
        
        if(!Path.Exists(LogFileDir))
        {
            Directory.CreateDirectory(LogFileDir);
        }

        if (!File.Exists(LogFilePath))
        {
            var header = new[]
            {
                $"#{string.Join(";",device.Ports.Select(p => p.Type.ToString()))}"
            };
            File.WriteAllLines(LogFilePath,header);
        }
        _writer = new StreamWriter(LogFilePath, true);
        Console.WriteLine($"Start Logging Device {Device.Name} in File {LogFilePath}");
    }

    public bool Stop()
    {
        if(_writer is null) return false;
        _writer.Close();
        _writer = null;
        return true;
    }

    public void Log()
    {
        if(_writer == null)
        {
            Console.WriteLine("_writer is null");
            return;
        }
        _writer.WriteLine($"{DateTime.UtcNow}: {Device.GetDataAsString()}");
        //Console.WriteLine($"wrote --> {DateTime.UtcNow}: {Device.GetDataAsString()} ---> in File: {LogFilePath.Split("net7.0/")[^1]}");
    }
}