namespace UsbDataReceiver;

public class DataManager
{
    private static readonly List<MeasuredDevice> DataDevices = new();
    private static Thread _updateDataThread = new (UpdateDataLoop);
    private static bool _isUpdating = true;
    // hsould be the minimum value to update the mindate otherwise always 0 if one peek is 0
    public static double StartMinThreshold { get; set; } = 3;
    /// <summary>
    ///     check if the data is updating
    ///     if set to true it starts the update thread
    /// </summary>
    public static bool IsUpdating
    {
        get => _isUpdating;
        set
        {
            _isUpdating = value;
            if(_isUpdating)
            {
                _updateDataThread = new Thread(UpdateDataLoop);
                _updateDataThread.Start();
            }
        }
        
    }
    /// <summary>
    ///  the interval between one update in ms
    /// </summary>
    public static int UpdateInterval { get; set; } = 20;
    /// <summary>
    ///  to initialize the data manager to a device
    /// </summary>
    /// <param name="device">the device where you want to update the data</param>
    public DataManager(MeasuredDevice device)
    {
        if(!_updateDataThread.IsAlive)
        {
            _updateDataThread.Start();
        }

        DataDevices.Add(device);
    }
    /// <summary>
    ///  to stop updating the data from a device
    /// </summary>
    /// <param name="deviceName"> the name of the device</param>
    /// <returns>true if worked otherwise false</returns>
    public bool Remove(string deviceName)
    {
        var dev = DataDevices.FirstOrDefault(d => d.Name == deviceName);
        if (dev is null) return false;
        DataDevices.Remove(dev);
        return true;
    }
    /// <summary>
    ///  the loop to update the data from the devices and calculates the min, max and mean (gleitender Mittelwert)
    /// </summary>
    private static void UpdateDataLoop()
    {
        while (IsUpdating)
        {
            for (int i = 0; i < DataDevices.Count; i++)
            {
                var data = DataDevices[i].ReadData();
                if (data is null) continue;
                // data without min and max
                var dataWithoutMinMax = data.Where(d => !d.Key.StartsWith("Max") && !d.Key.StartsWith("Min"))
                    .ToDictionary(p => p.Key, p => p.Value);

                
                foreach( var (key,value) in dataWithoutMinMax)
                {
                    // flaoting mean calculation
                    if(!DataDevices[i].FloatingMeanQueues.ContainsKey(key))
                    {
                        DataDevices[i].FloatingMeanQueues.Add(key, new Queue<double>());
                    }
                    DataDevices[i].FloatingMeanQueues[key].Enqueue(value);
                    if(DataDevices[i].QueueIsFull)
                    {
                        DataDevices[i].FloatingMeanQueues[key].Dequeue();
                    }
                    var mean = CalculateFloatingMean(DataDevices[i].FloatingMeanQueues[key]);
                    
                    // update the value with the key and calculate min and max
                    UpdateValue(key, mean, DataDevices[i].Data );
                }
                
                Thread.Sleep( DataDevices.Count != 0 ? UpdateInterval/DataDevices.Count : UpdateInterval);   
            }
        }
        
        void UpdateValue(string key, double value, IDictionary<string, double> data)
        {
            string maxKey = "Max"+key;
            string minKey = "Min"+key;
            
            data[key] = value;
            
            if(!data.ContainsKey(maxKey))
            {
                data.Add(maxKey, value);
            }
            if(!data.ContainsKey(minKey))
            {
                data.Add(minKey, value);
                return;
            }
            
            data[maxKey] = Math.Max(data[maxKey], value);
            data[minKey] = Math.Min(data[minKey], value);
            
        }
        
        double CalculateFloatingMean(Queue<double> queue)
        {
            var values = new List<double>();
            foreach (var value in queue)
            {
                values.Add(value);
            }
            values.Sort();
            if (values.Count < 2) return values.FirstOrDefault();
            if(values.Count % 2 == 0)
            {
                return (values.ElementAt(values.Count / 2) + values.ElementAt(values.Count / 2) - 1 ) / 2;
            }
            return values.ElementAt(values.Count / 2);
        }
    }
    
}