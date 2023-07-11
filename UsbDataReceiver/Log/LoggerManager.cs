namespace UsbDataReceiver.Log;

public class LoggerManager
{
    public string LogPath { get; set; }
    private readonly List<Logger> _loggerList = new();
    public int Interval { get; }
    public string LogName { get;  }
    public bool IsLogging => _loggerList.Count > 0;
    private Thread _logThread;
    public LoggerManager(string logPath, string logName, int interval = 2000)
    {
        _logThread = new Thread(LogLoop);
        LogPath = logPath;
        LogName = logName;
        Interval = interval;
    }
    public bool StartLoggingDevice(MeasuredDevice device)
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
    public bool StopLoggingDevice(string deviceName)
    {
        var logger = _loggerList.FirstOrDefault(l => l.Device.Name == deviceName);
        return logger != null && logger.Stop() && _loggerList.Remove(logger);
    }

    private void LogLoop()
    {
        Console.WriteLine("Starting Log Loop");
        while (IsLogging)
        {
            for (int i = 0; i < _loggerList.Count; i++)
            {
                _loggerList[i].Log();
                Thread.Sleep(Interval/ _loggerList.Count);
            }
        }
    }
    
}
