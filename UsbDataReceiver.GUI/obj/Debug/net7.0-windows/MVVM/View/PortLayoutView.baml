   M S B A M L   `   `   `  ����N  JUsbDataReceiver.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:UsbDataReceiver.GUI.MVVM.ViewModel"UsbDataReceiver.GUI.MVVM.ViewModel  5      2    ,UsbDataReceiver.GUI.MVVM.View.PortLayoutView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      F	viewModel0clr-namespace:UsbDataReceiver.GUI.MVVM.ViewModel     5        � DataContext  5      <  6UsbDataReceiver.GUI.MVVM.ViewModel.PortLayoutViewModel  5	   
   .��5      � .+�5   
   ��    PortForm#��[  6   $��0,0,70,0q�69   .@�5      �� -   5        $��30��5      
������ $
��🞩��5      
���  ��$��20(�5      $��Right=�5      $��0,0,15,0q�5      5      �� 5      
�������$��14(�6&   .��64   Name:6;   �� 5      -   5         ��Name$ PortName��5      
�������$��90��5      $	��115��5      $��30��5      $��14(�5      ���	#FF3F3F3F$��SemiBold"�5      $��Center=�5!      $��Center=�5"      ��5       �� *
PortName+5"      �� 5#      
�������$��14(�6&   .��64   MeasurementType:6F   �� 5$      -   5         ��Name$ TypeSelector��5$      $��70��5%      $��Center=�5&      $��Center=�5'      $��SemiBold"�5(      
������� �� SelectedIndex$ 0��5+      $ �� IsSynchronizedWithCurrentItem$
 TrueY�5-      ��5*      �� *MeasurementTypes+5.      �� 5/      
�������$��14(�6&   .��64   
NI Device:6@   �� 50      -   5        $ NiDeviceSelector��50      $��Center=�51      $��Center=�52      $��70��53      $��SemiBold"�54      
�������$
 TrueY�58      $ 0��59      ��56      �� *AvailableNiDeviceNames+5:      �� 5;      
�������$��14(�6&   .��64   Port:6;   �� 5<      -   5        $ PortSelector��5<      $��70��6-   $��Center=�5=      $��Center=�5>      $��SemiBold"�5?      ��5@      �� *PortsOfSelectedDevice+ �� SelectedValue 5A      �� *SelectedPort+ �� Mode$ TwoWay=�5B      5C      5D      5E      