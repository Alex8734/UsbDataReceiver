using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDataReceiver.GUI.Core;
using UsbDataReceiver.GUI.MVVM.View;

namespace UsbDataReceiver.GUI.MVVM.ViewModel
{
    public class AllDataDisplayViewModel : ObservableObject
    {

        public List<ChartLayout> Views { get; }
        public AllDataDisplayViewModel()
        {
            Views = new List<ChartLayout>();
        }

        public void AddDevice(MeasuredDevice device)
        {
            var chartLayout = new ChartLayout(device);
            Views.Add(chartLayout);

            OnPropertyChanged(nameof(Views));
        }
    }
}
