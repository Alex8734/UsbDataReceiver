namespace UsbDataReceiver.Log;

public class Logger
{
    public string LogFilePath { get; }
    public int LogInterval { get; set; }
    public MeasuredDevice Device { get; }
    public StreamWriter? Writer { get; private set; } = null;
    private Task? _logTask = null;
    public Logger(MeasuredDevice device, string logPath, int interval)
    {
        Device = device;
        LogInterval = interval;
        var currentTime = DateTime.Now;
        var currentDate = DateOnly.FromDateTime(currentTime);
        LogFilePath = logPath + $"/{Device.Name}/{currentDate:yyyy-MM-dd}";
        if(Path.Exists(logPath + $"/{Device.Name}"))
            Directory.CreateDirectory(logPath + $"/{Device.Name}");
    }
    
    public void Start()
    {
        Writer = new StreamWriter(LogFilePath, true);
        _logTask = new Task(LogLoop);
    }
    private void LogLoop()
    {
        while (Writer != null)
        {
            Writer.WriteLine(Device.ReadDataString());
            Thread.Sleep(LogInterval);
        }   
    }
}