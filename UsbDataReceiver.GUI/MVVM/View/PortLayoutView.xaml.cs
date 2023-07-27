using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.GUI.MVVM.ViewModel;

namespace UsbDataReceiver.GUI.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für PortLayoutView.xaml
    /// </summary>
    public partial class PortLayoutView : UserControl
    {
        
        public PortLayoutView()
        {
             InitializeComponent();
            if (DataContext is PortLayoutViewModel vm)
            {
                vm.SelectedIoDevice = NiDeviceSelector.SelectedValue?.ToString() ?? "";
                PortSelector.SelectedIndex = 0;
            }
        }

        private void NiDeviceSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is PortLayoutViewModel vm)
            {
                vm.SelectedIoDevice = NiDeviceSelector.SelectedValue.ToString();
                if(PortSelector is not null)
                {
                    PortSelector.SelectedIndex = 0;
                }
            }
        }

        private void RemoveItem_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (sender is Button button &&
                button.FindParent<AddDeviceView>() is
                {
                    DataContext: AddDeviceViewModel vmParent
                } && button.FindParent<PortLayoutView>() is
                {
                    DataContext: PortLayoutViewModel vm
                })
            {
                MainViewModel.IoDevices?.FirstOrDefault(d => d.Name == vm.SelectedIoDevice)?.AvailablePorts.Add(vm.SelectedPort ?? 0);
                vmParent.RemovePort(this);
            }
        }

        private void PortSelector_OnSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (DataContext is PortLayoutViewModel vm && PortSelector.SelectedValue is not null && int.TryParse(PortSelector.SelectedValue.ToString(), out var selectedPort))
            {
                vm.SelectedPort = selectedPort;
            }
        }

        private void PortSelector_OnDropDownOpened(object? sender, EventArgs e)
        {
            if (DataContext is PortLayoutViewModel vm && sender is ComboBox comboBox)
            {
                
                vm.OnPropertyChanged(nameof(vm.PortsOfSelectedDevice));
            }
        }

        private void TypeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is PortLayoutViewModel vm && TypeSelector.SelectedValue is not null && Enum.TryParse<MeasurementType>(TypeSelector.SelectedValue.ToString(), out MeasurementType selectedPortType))
            {
                vm.SelectedPortType = selectedPortType ;
            }
        }
    }
}
