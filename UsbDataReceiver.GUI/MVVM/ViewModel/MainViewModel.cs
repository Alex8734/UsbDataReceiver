using UsbDataReceiver.GUI.Core;

namespace UsbDataReceiver.GUI.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{

    public RelayCommand DataDisplayCommand { get; set; }
    public RelayCommand AddDeviceCommand { get; set; }
    public AddDeviceViewModel AddDeviceVM { get; set; }
    public DataDisplayViewModel DataDisplayVM { get; set; }
    
    private object _currentView = null!;
    public object CurrentView
    {

        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }
    public MainViewModel()
    {
        AddDeviceVM = new AddDeviceViewModel();
        DataDisplayVM = new DataDisplayViewModel();
        
        CurrentView = DataDisplayVM;
        
        DataDisplayCommand = new RelayCommand(o =>
        {
            CurrentView = DataDisplayVM;
        });
        
        AddDeviceCommand = new RelayCommand(o =>
        {
            CurrentView = AddDeviceVM;
        });
    }
}