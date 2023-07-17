using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using InteractiveDataDisplay.WPF;
using UsbDataReceiver.GUI.MVVM.ViewModel;
using UsbDataReceiver.Log;

namespace UsbDataReceiver.GUI.MVVM.View;

public partial class DataDisplayView : UserControl
{
    
    public DataDisplayView()
    {
        InitializeComponent();
        
        var dev = new IODevice("Dev1", 8);

        var device1 = new MeasuredDevice("device1", new[]
        {
            new PortDescription(dev.GetNextAvailablePort(), MeasurementType.Voltage, dev, "Voltage"),
            new PortDescription(dev.GetNextAvailablePort(), MeasurementType.Ampere, dev, "Ampere"),
            new PortDescription(dev.GetNextAvailablePort(), MeasurementType.Temperature, dev, "Temperature")
        });
        var device2 = new MeasuredDevice("device2", new[]
        {
            new PortDescription(dev.GetNextAvailablePort(), MeasurementType.Voltage, dev, "Voltage"),
            new PortDescription(dev.GetNextAvailablePort(), MeasurementType.Ampere, dev, "Ampere"),
            new PortDescription(dev.GetNextAvailablePort(), MeasurementType.Temperature, dev, "Temperature")
        });


        var logManager = new LoggerManager(Environment.CurrentDirectory + "/Data", "v1",2000);

        logManager.StartLoggingDevice(device1);
        //logManager.StartLoggingDevice(device2);
        var vm = (DataDisplayViewModel) this.DataContext;
        vm.AddDevice(device1);
        //vm.AddDevice(device2);
        Console.WriteLine();
    }
}