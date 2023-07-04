namespace UsbDataReceiver.Log;

public class LoggerManager
{
    public string LogPath { get; set; }
    public string LogName { get; }
    private Dictionary<string,StreamWriter> _writers = new();
    
    public LoggerManager(string logPath, string logName)
    {
        LogPath = logPath;
        LogName = logName;
    }
    public void StartDevice(string deviceName)
    {
        var currentTime = DateTime.Now;
        var currentDate = DateOnly.FromDateTime(currentTime);
        var filePath = LogPath + $"/{deviceName}/{LogName}{currentDate:yyyy-MM-dd}";
        if(Path.Exists(LogPath + $"/{deviceName}"))
            Directory.CreateDirectory(LogPath + $"/{deviceName}");
        var writer = new StreamWriter(filePath, true);
        _writers.Add(deviceName,writer);

    }
    public void StopDevice(string deviceName)
    {
        if (_writers.ContainsKey(deviceName))
        {
            _writers[deviceName].Close();
            _writers.Remove(deviceName);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="deviceName"></param>
    private void LogLoop(string data, string deviceName)
    {
        if(_writers.TryGetValue(deviceName, out var writer))
        {
            writer.WriteLine(data);
        }
    }
}
