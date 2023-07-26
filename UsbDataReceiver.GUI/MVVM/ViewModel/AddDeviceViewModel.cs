using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using NationalInstruments.DAQmx;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver;
using UsbDataReceiver.GUI.MVVM.View;
using System.Collections.ObjectModel;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class AddDeviceViewModel : ObservableObject
{
    public RelayCommand NameCommand => new((o) =>
    {
        Keyboard.ClearFocus();
    });

    private ObservableCollection<PortLayoutView> _portLayoutViews;
    public ObservableCollection<PortLayoutView> PortLayoutViews
    {
        get => _portLayoutViews;
        set
        {
            _portLayoutViews = value;
            OnPropertyChanged();
        }
    }
    private string _deviceName = string.Empty;
    public string DeviceName
    {
        get => _deviceName;
        set
        {
            if(MainViewModel.Devices.Any(d => d.Name == value)) return;
            _deviceName = value;
        }
    }

    public void RemovePort(PortLayoutView port)
    {
        PortLayoutViews.Remove(port);
        OnPropertyChanged();
    }
    public AddDeviceViewModel()
    {
        _portLayoutViews = new ObservableCollection<PortLayoutView>();
    }

    public PortLayoutView AddPort()
    {
        var port = new PortLayoutView();
        PortLayoutViews.Add(port);
        OnPropertyChanged(nameof(PortLayoutViews));
        return port;
    }
}