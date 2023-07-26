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
    
    public static DeviceLogs? ReadDeviceLogs(string path)
    {
        var deviceName = path.Split("/").Last();
        var logs = new List<(string, List<MeasurementData>)>();
        foreach (var file in Directory.GetFiles(path))
        {
            var logName = file.Split("/").Last();
            var data = ReadLogFile(file);
            if(data != null)
            {
                logs.Add((logName, data));
            }
        }
        if(logs.Count == 0)
        {
            return null;
        }
        return new DeviceLogs(deviceName, logs);
    }
    
    public static List<MeasurementData>? ReadLogFile(string path)
    {
        string[] file;
        try
        {
            file = File.ReadAllLines(path);
        }
        catch (IOException)
        {
            return null;
        }
        
        
        if(file.Length == 0 || !file[0].StartsWith("#Date_Time;"))
        {
            return null;
        }
        var data = new List<MeasurementData>();
        var keys = file[0].Split("#")[1].Split(";");
        
        
        for(int i = 1; i< file.Length; i++)
        {
            var line = file[i].Split(';');
            if(!DateTime.TryParse(line[0],out var date)) continue;
            var values = new Dictionary<string, double>();
            for(int j = 1; j < line.Length; j++)
            {
                if(keys.Length != line.Length || 
                   !double.TryParse(line[j],out var value))
                {
                    continue;
                }
                var key = keys[j];
                values.Add(key, value);
            }
            data.Add(new MeasurementData(date, values));
        }

        return data;
    }
}
