using NationalInstruments.DAQmx;
using Task = NationalInstruments.DAQmx.Task;
using UsbDataReceiver.Extensions;

Task voltageInTask = new();
Task ampereInTask = new();
Task temperatureInTask = new();

//setup channels
var voltageAiChannel = voltageInTask.AIChannels.CreateVoltageChannel(
    "Dev2/ai0",
    "VoltageChannel",
    AITerminalConfiguration.Rse,
    0,
    10,
    AIVoltageUnits.Volts);
var ampereAiChannel = ampereInTask.AIChannels.CreateVoltageChannel(
    "Dev2/ai1",
    "AmpereChannel",
    AITerminalConfiguration.Rse,
    0,
    10,
    AIVoltageUnits.Volts);
var temperatureAiChannel = temperatureInTask.AIChannels.CreateVoltageChannel(
    "Dev2/ai2",
    "TemperatureChannel",
    AITerminalConfiguration.Rse,
    0,
    10,
    AIVoltageUnits.Volts);

AnalogSingleChannelReader reader = new(voltageInTask.Stream);
AnalogSingleChannelReader reader2 = new(ampereInTask.Stream);
AnalogSingleChannelReader reader3 = new(temperatureInTask.Stream);
while (true)
{
    var volts = reader.ReadSingleSample().Round(2);
    var ampere = reader2.ReadSingleSample().Round(2);
    var temperature = reader3.ReadSingleSample().Round(2);
    Console.WriteLine($"volts: {volts}, ampere: {ampere}, temperature: {temperature}");
    
}

