   M S B A M L   `   `   `  ����N  JUsbDataReceiver.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:UsbDataReceiver.GUI.MVVM.ViewUsbDataReceiver.GUI.MVVM.View  5      W0clr-namespace:UsbDataReceiver.GUI.MVVM.ViewModel"UsbDataReceiver.GUI.MVVM.ViewModel  5      1    +UsbDataReceiver.GUI.MVVM.View.AddDeviceView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:UsbDataReceiver.GUI.MVVM.View     5      F	viewModel0clr-namespace:UsbDataReceiver.GUI.MVVM.ViewModel     5      
������ $��0q�5        � DataContext  5      ;   5UsbDataReceiver.GUI.MVVM.ViewModel.AddDeviceViewModel  5   
   .��5      � $
|�True��6    � RowDefinitions 5   
   �� 5      $	��105 �6   �� 5      �� 5      $	��105 �6    � ColumnDefinitions 5   
   �� 5      $��80 �6   �� 5      �� 5      $	��100 �6   .+�5   
   �� -   5         ��Name$ outerNameBox��5      $��0��5      $��1��6   $��0,20,0,0q�5       $��34��5!      $	��220��5"      $��
New Device��5#      $��Italic$�5$         NameBox#��[  5%      .��5'   A   5(      �� 5)   
   $��0��6   $��1��6   $��0,10,0,0q�6-   
�������$��20(�5+      $��Normal"�5,      $��Center=�5-      $��36��6-   $	��Top=�69   .��5.      �� $�Add new Transponder��6   5/      �� 50   
   -   5        $��2��50      $��2��6   
������ $��0,0,10,0q�51      $	��Add��52      $��.6q�53      
�������$��15(�55      $��Normal"�56      $��80��57      $��30��58      5;      �� 5<   
   $��1��6   $��1��6%    �� Orientation$ Vertical=�62   .+�5=      �� $��0,0,0,0q�6   ��6-   �� *PortLayoutViews+�� 5>      -   5          PortFormTransparency#��[ 5>   9   $��0,0,70,0q�6g   .@�5@      �� -   5        $��30��5@      $��30��5A      
������ $��Right=�5C      $��0,0,15,0q�5D       J� Opacity$ 1Q�5F      .��5G      �� 
��� � �$ 1Q�5H       $��20(�5I       $��45��5J       $
��Bold"�5K       $��0,-7,0,0q�5L       .��62   +65   5M      �� 5P      
�������$��14(�6*   .��68   Name:6?   �� 5Q      -   5        $ PortNameDummy��5Q      $��90��5R      $	��115��5S      $��30��5T      ${�False��5U      $��17(�5V      ���	#FF3F3F3F$��SemiBold"�5X      $��Center=�5Y      $��Center=�5Z      �� 5[      
�������$��14(�6*   .��68   MeasurementType:6J   �� 5\      -   5         ��Name$ MeasurementTypeDummy��5\      $��70��5]      ${�False��5^      $��Center=�5_      $��Center=�5`      $��SemiBold"�5a      
������� �� SelectedIndex$ 0��5c      $ �� IsSynchronizedWithCurrentItem$
 TrueY�5d      5e      �� 5f      
�������$��14(�6*   .��68   
NI Device:6D   �� 5g      -   5        $ NiDeviceDummy��5g      $��Center=�5h      $��Center=�5i      $��70��5j      ${�False��5k      $��SemiBold"�5l      
�������$
 TrueY�5n      $ 0��5o      5p      �� 5q      
�������$��14(�6*   .��68   Port:6?   �� 5r      -   5        $ PortSelectorDummy��5r      $��70��66   $��Center=�5s      $��Center=�5t      ${�False��5u      $��SemiBold"�5v      $
 TrueY�5w      $ 0��5x      5y      5z      5{      5|      