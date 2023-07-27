using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.GUI.MVVM.Model;
using UsbDataReceiver.GUI.MVVM.View;

namespace UsbDataReceiver.GUI.MVVM.ViewModel
{
    internal class LogViewModel : ObservableObject
    {
        private DataDisplayView _currentView;
        private ObservableCollection<LogItemDisplayModel> _displayLogs;
        public DeviceLogs Logs { get; set; }

        public ObservableCollection<LogItemDisplayModel> DisplayLogs
        {
            get => _displayLogs;
            set
            {
                _displayLogs = value;
                OnPropertyChanged();
            }
        }
            

        public DataDisplayView CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public void InitDevicesLogs(DeviceLogs logs)
        {
            Logs = logs;
            DisplayLogs = new(Logs.Logs.Select(l => new LogItemDisplayModel(l.LogName, l.Data)));
        }

    }
}
