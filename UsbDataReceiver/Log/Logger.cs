namespace UsbDataReceiver.Log;


public class Logger
{
    public string LogFilePath { get; }
    public string LogFileDir { get; }
    public MeasuredDevice Device { get; }
    private StreamWriter? _writer;
    public string LogDetails { get; }
    /// <summary>
    ///     creates a new Logger for a device but does not start logging
    ///     it needs the logger manager to start logging
    /// </summary>
    /// <param name="device">the device that should be logged</param>
    /// <param name="logPath">the path, where its gonna be logged</param>
    /// <param name="logDetails">you can add some details for the log</param>
    public Logger(MeasuredDevice device, string logPath, string logDetails)
    {
        Device = device;
        var currentTime = DateTime.Now;
        var currentDate = DateOnly.FromDateTime(currentTime);
        
        LogDetails = logDetails;
        LogFileDir = $"{logPath}/{Device.Name.Trim(' ')}";

        LogFilePath = $"{LogFileDir}/{currentTime:yy-MMM-dd--H-mm}-{LogDetails}.csv";
        
        if(!Path.Exists(LogFileDir))
        {
            Directory.CreateDirectory(LogFileDir);
        }

        if (!File.Exists(LogFilePath))
        {
            
            while (device.Data.Count == 0)
            {
                //wait till device has data
            }
            
            var header = new[]
            {
                $"#Date_Time;{string.Join(";",device.Data.Select(p => p.Key.ToString()))}"
            };
            File.WriteAllLines(LogFilePath,header);
        }
        _writer = new StreamWriter(LogFilePath, true);
        Console.WriteLine($"Start Logging Device {Device.Name} in File {LogFilePath}");
    }
    /// <summary>
    ///  stop logging just to close the writer
    ///     call it at the end of the program
    /// </summary>
    /// <returns>true if logger loged</returns>
    public bool Stop()
    {
        if(_writer is null) return false;
        _writer.Close();
        _writer = null;
        return true;
    }
    /// <summary>
    ///     logs the current data of the device
    /// </summary>
    public async Task Log()
    {
        if(_writer == null)
        {
            Console.WriteLine("_writer is null");
            return;
        }
        await _writer.WriteLineAsync($"{DateTime.Now};{Device.GetDataAsString()}");
        
        //Console.WriteLine($"wrote --> {DateTime.UtcNow}: {Device.GetDataAsString()} ---> in File: {LogFilePath.Split("net7.0/")[^1]}");
    }
}