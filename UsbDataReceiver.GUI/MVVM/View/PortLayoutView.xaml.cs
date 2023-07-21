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
            }
        }

        private void NiDeviceSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is PortLayoutViewModel vm)
            {
                vm.SelectedIoDevice = NiDeviceSelector.SelectedValue.ToString();
            }
        }

        private void RemoveItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.FindParent<AddDeviceView>() is
                {
                    DataContext: AddDeviceViewModel vmParent
                })
            {
                vmParent.RemovePort(this);
            }
        }

        private void PortSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is PortLayoutViewModel vm && PortSelector.SelectedValue is not null && int.TryParse(PortSelector.SelectedValue.ToString(), out var selectedPort))
            {
                vm.SelectedPort = selectedPort;
            }
        }
    }
}
