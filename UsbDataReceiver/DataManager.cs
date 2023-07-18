namespace UsbDataReceiver;

public class DataManager
{
    private static readonly List<MeasuredDevice> DataDevices = new();
    private static Thread _updateDataThread = new (UpdateDataLoop);
    private static bool _isUpdating = true;
    public static double LowestMinValue { get; set; } = 3;
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
    
    public static int UpdateInterval { get; set; } = 20;
    public DataManager(MeasuredDevice device)
    {
        if(!_updateDataThread.IsAlive)
        {
            _updateDataThread.Start();
        }

        DataDevices.Add(device);
    }
    public bool Remove(string deviceName)
    {
        var dev = DataDevices.FirstOrDefault(d => d.Name == deviceName);
        if (dev is null) return false;
        DataDevices.Remove(dev);
        return true;
    }
    
    private static void UpdateDataLoop()
    {
        while (IsUpdating)
        {
            for (int i = 0; i < DataDevices.Count; i++)
            {
                var data = DataDevices[i].ReadData();
                if (data is null) continue;
                var dataWithoutMinMax = data.Where(d => !d.Key.StartsWith("Max") && !d.Key.StartsWith("Min"))
                    .ToDictionary(p => p.Key, p => p.Value);
                foreach( var (key,value) in dataWithoutMinMax)
                {
                    UpdateValue(key, value, DataDevices[i].Data, data);
                }
                Thread.Sleep( DataDevices.Count != 0 ? UpdateInterval/DataDevices.Count : UpdateInterval);   
            }
        }
        
        void UpdateValue(string key, double value, IDictionary<string, double> data, IDictionary<string,double> newData)
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
    }
    
}