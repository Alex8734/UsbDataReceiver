using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NationalInstruments.DAQmx;
using UsbDataReceiver.GUI.MVVM.Model;
using UsbDataReceiver.GUI.MVVM.View;
using UsbDataReceiver.GUI.MVVM.ViewModel;
using UsbDataReceiver.Log;

namespace UsbDataReceiver.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine();
            var devices = SaveLoader.LoadMeasuredDevices();
            
            foreach (var dev in devices)
            {
                var ports = ConvertFromJson(dev.Ports);
                if (ports == null) continue;
                var device = new MeasuredDevice(dev.Name, ports);
                MainViewModel.Devices.Add(device);
                foreach (var port in ports)
                {
                    port.Device.AvailablePorts.Remove(port.Id);
                }
            }
            if( DataContext is not MainViewModel vm) return;
            vm.OnPropertyChanged(nameof(vm.DisplayDevices));
            
            
            List<PortDescription>? ConvertFromJson(List<JsonPortDescription> ports)
            {
                var measuredPorts = new List<PortDescription>();

                foreach (var port in ports)
                {
                    var ioDevice = MainViewModel.IoDevices.FirstOrDefault(io => io.Name == port.Device);
                    if (ioDevice == null) return null;
                    measuredPorts.Add(new PortDescription(port.Id, port.Type, ioDevice, port.Name));
                }

                return measuredPorts;
            }
            
        }

        private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow != null && Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                if (Application.Current.MainWindow != null)
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            DataManager.IsUpdating = false;
            LogManager.StopAll();
            
        }


        private void DisplayIoDevices()
        {
            var devices = IODevice.GetConnectedDevices();
            IoDeviceList.Children.Clear();
            if(devices is null || devices.Count == 0)
            {
                IoDeviceList.Children.Add(new Label()
                {
                    Content = "No IO Devices"
                });
                return;
            }

            foreach (var dev in devices)
            {
                var simulatedInfo = dev.IsSimulated ? "  (virtual)" : "";
                IoDeviceList.Children.Add(new Label
                {
                    Foreground  = Brushes.Gray,
                    Content = $"{dev.DeviceID} - {dev.ProductType}{simulatedInfo}"
                });
                
            }
        }
        private void Sidebar_OnLoaded(object sender, RoutedEventArgs e)
        {
            DisplayIoDevices();
        }

        private void RefreshIODevices_Click(object sender, RoutedEventArgs e)
        {
            DisplayIoDevices();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm) return;
            vm.BackButtonVisibility = Visibility.Visible;
        }

    }
}