namespace UsbDataReceiver.Log;
/// <summary>
///     manage the loggers and start logging
///     because can't access the data at the same time from different threads
/// </summary>
public static class LogManager
{
    /// <summary>
    ///    sets the path where the logs are saved
    ///     default : current directory + /Data
    /// </summary>
    public static string LogPath { get; set; } = Environment.CurrentDirectory + "/Data";
    private static readonly List<Logger> LoggerList = new();
    /// <summary>
    ///  sets the interval between one log
    /// </summary>
    public static int Interval { get; set; } = 2000;
    /// <summary>
    ///  sets the name of the log / whats gonna be logged
    /// </summary>
    public static string LogName { get; set; }
    /// <summary>
    ///   check if any device is logged
    /// </summary>
    public static bool IsLogging
    {
        get => LoggerList.Count > 0;
    }

    private static Thread _logThread = new (LogLoop);
    /// <summary>
    ///    start logging a device
    ///     it adds a neq logger to the logger list
    /// </summary>
    /// <param name="device">the device to log</param>
    /// <returns>true if device isn't logged yet and starting worked</returns>
    public static bool StartLoggingDevice(MeasuredDevice device)
    {
        var logger = new Logger(device, $"{LogPath}", LogName);
        if (LoggerList.All(l => l.Device.Name != device.Name)) return false;
        LoggerList.Add(logger);
        if (LoggerList.Count != 1) return false;
        _logThread = new Thread(LogLoop);
        _logThread.Start();
        return true;
    }
    /// <summary>
    ///     stop logging a device
    /// </summary>
    /// <param name="deviceName">the name of the device to stop</param>
    /// <returns>true if successfully stopped and device exists in LogList</returns>
    public static bool StopLoggingDevice(string deviceName)
    {
        var logger = LoggerList.FirstOrDefault(l => l.Device.Name == deviceName);
        return logger != null && logger.Stop() && LoggerList.Remove(logger);
    }
    /// <summary>
    ///   stop logging all devices    
    /// </summary>
    public static void StopAll()
    {
        foreach (var logger in LoggerList)
        {
            StopLoggingDevice(logger.Device.Name);
        }
    }
    
    private static async void LogLoop()
    {
        Console.WriteLine("Starting Log Loop");
        int logCount = 0;
        while (IsLogging)
        {
            for (int i = 0; i < LoggerList.Count; i++)
            {
                await LoggerList[i].Log();
                Console.WriteLine($"Logged {LoggerList[i].Device.Name}    {logCount}.");
                Thread.Sleep(Interval/ LoggerList.Count);
            }
            logCount++;
        }
    }
    /// <summary>
    ///   read all logs from a device ( directory )
    /// </summary>
    /// <param name="path">the path to the device ( directory ) </param>
    /// <returns>all logs of the device</returns>
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
    /// <summary>
    ///     read a log file
    /// </summary>
    /// <param name="path">the path to the lof file</param>
    /// <returns>a list of <see cref="MeasurementData"/></returns>
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
