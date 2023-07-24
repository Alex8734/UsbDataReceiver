using NationalInstruments.DAQmx;
using UsbDataReceiver;
using UsbDataReceiver.Log;
using Task = NationalInstruments.DAQmx.Task;
using UsbDataReceiver.Extensions;


var dev = new IODevice("Dev2", 8);

var device1 = new MeasuredDevice("AmpereTest", new[]
{
    new PortDescription(dev.GetNextAvailablePort(), MeasurementType.Ampere, dev, "Ampere")
});

LogManager.LogName = "Test1";
DataManager.UpdateInterval = 20;
LogManager.StartLoggingDevice(device1);
Thread.Sleep(1000);

while(true)
{
    Console.WriteLine(string.Join("   ",device1.Data.Select(d => $"{d.Key}: {d.Value:F3}")));
}

//logManager.StartLoggingDevice(device2);

  //__Hardcoded_Variant__//

/*Task measureTask = new();

//setup channels

var voltageAiChannel = measureTask.AIChannels.CreateVoltageChannel(
    "Dev2/ai0",
    "VoltageChannel",
    AITerminalConfiguration.Rse,
    0,
    10,
    AIVoltageUnits.Volts);
var ampereAiChannel = measureTask.AIChannels.CreateVoltageChannel(
    "Dev2/ai1",
    "AmpereChannel",
    AITerminalConfiguration.Rse,
    0,
    10,
    AIVoltageUnits.Volts);
var temperatureAiChannel = measureTask.AIChannels.CreateVoltageChannel(
    "Dev2/ai2",
    "TemperatureChannel",
    AITerminalConfiguration.Rse,
    0,
    10,
    AIVoltageUnits.Volts);

AnalogMultiChannelReader reader = new(measureTask.Stream);
while (true)
{
    var data = reader.ReadSingleSample();
    if(data is null)
    {
        Console.WriteLine("data is null");
        continue;
    }
    Console.WriteLine($"volts: {data[0].Round(2),5:F2} ampere: {data[1].Round(2),5:F2} temperature: {data[2],5}");
    
}
*/