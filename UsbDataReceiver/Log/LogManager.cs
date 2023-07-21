namespace UsbDataReceiver.Log;

public static class LogManager
{
    public static string LogPath { get; set; } = Environment.CurrentDirectory + "/Data";
    private static readonly List<Logger> _loggerList = new();
    public static int Interval { get; set; } = 2000;
    public static string LogName { get; set; }
    public static bool IsLogging
    {
        get => _loggerList.Count > 0;
    }

    private static Thread _logThread = new (LogLoop);
    public static bool StartLoggingDevice(MeasuredDevice device)
    {
        var logger = new Logger(device, $"{LogPath}", LogName);
        _loggerList.Add(logger);
        if(_loggerList.Count == 1)
        {
            _logThread = new Thread(LogLoop);
            _logThread.Start();
        }
        return true;
    }
    public static bool StopLoggingDevice(string deviceName)
    {
        var logger = _loggerList.FirstOrDefault(l => l.Device.Name == deviceName);
        return logger != null && logger.Stop() && _loggerList.Remove(logger);
    }

    public static void DisposeAll()
    {
        foreach (var logger in _loggerList)
        {
            StopLoggingDevice(logger.Device.Name);
        }
    }

    private static async void LogLoop()
    {
        Console.WriteLine("Starting Log Loop");
        while (IsLogging)
        {
            for (int i = 0; i < _loggerList.Count; i++)
            {
                await _loggerList[i].Log();
                Thread.Sleep(Interval/ _loggerList.Count);
            }
        }
    }
    
}
