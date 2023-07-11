namespace UsbDataReceiver.Log;

public class Logger
{
    public string LogFilePath { get; }
    public MeasuredDevice Device { get; }
    private StreamWriter? _writer;
    public string LogName { get; }
    public Logger(MeasuredDevice device, string logPath, string logName)
    {
        Device = device;
        var currentTime = DateTime.Now;
        var currentDate = DateOnly.FromDateTime(currentTime);
        LogName = logName;
        LogFilePath = logPath + $"/{Device.Name}/{currentDate:yyyy-MM-dd}-{LogName}.csv";
        if(!Path.Exists(logPath + $"/{Device.Name}"))
        {
            Directory.CreateDirectory(logPath + $"/{Device.Name}");
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
        Console.WriteLine($"wrote --> {DateTime.UtcNow}: {Device.GetDataAsString()} ---> in File: {LogFilePath.Split("net7.0/")[^1]}");
    }
}