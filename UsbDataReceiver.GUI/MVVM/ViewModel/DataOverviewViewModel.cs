using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.GUI.MVVM.View;

namespace UsbDataReceiver.GUI.MVVM.ViewModel
{
    public class DataOverviewViewModel : ObservableObject
    {

        public List<ChartItem> Views { get; }
        public DataOverviewViewModel()
        {
            Views = new List<ChartItem>();
        }

        public void AddDevice(MeasuredDevice device)
        {
            var chartLayout = new ChartItem(device);
            Views.Add(chartLayout);

            OnPropertyChanged(nameof(Views));
        }
    }
}
