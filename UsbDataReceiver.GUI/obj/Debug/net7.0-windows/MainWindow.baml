   M S B A M L   `   `   `  ����N  JUsbDataReceiver.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:UsbDataReceiver.GUI.MVVM.ViewModel"UsbDataReceiver.GUI.MVVM.ViewModel  5   	   $    UsbDataReceiver.GUI.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   F	viewModel0clr-namespace:UsbDataReceiver.GUI.MVVM.ViewModel     5   	   ���#36393F  � WindowStyle$
  None=�5	   	    � AllowsTransparency$
 True��5
   	    � 
ResizeMode$ CanResizeWithGrip=�5   	    � Title$ NI Data Logger��5   	   $	��650��6    $
��1200��6-    � DataContext 5      6   0UsbDataReceiver.GUI.MVVM.ViewModel.MainViewModel  5   
   .��5      � -   5         �Name$ WindowLayout��5       � RowDefinitions 5   
   �� 5      $��30 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $	��200 �6   �� 5      .+�5   
   �� -   5        $��2��5      ���#252525._�5       � $��Stretch=�6   $��2,2,2,2q�5!      .+�5"      �� $��NI Data Receiver��6   
�������$��SemiBold"�5$      5&      �� 5'      $��Right=�6    �� Orientation$ 
Horizontal=�5(      .+�5*      �� -   5        $��20��5*      $��20��6(   $��0,0,0,3q�5+      $
��🗕��5,      
������ $��0q�5.      
�������$
��Bold"�50      52      �� 53      -   5        $��20��53      $��20��6(   $	��▢��54      
������ $��0q�56      
�������$
��Bold"�58      5:      �� 5;      -   5        $��20��5;      $��20��6(   $	��✕��5<      
������ $��0q�5>      
�������$
��Bold"�5@      5B      5C      5D      5E      � 5F   
   -   5        ���#2F3136$��1��5G      $��0��6    5I      �� 5J      $��50��6    �� 5K      $��0��6    $
��Auto �6.   �� 5L      $��50 �6    �� 5M      $��0��6    $
��Auto �6.   �� 5N      $��50 �6    �� 5O      $��0��6    $
��Auto �6.   .+�5Q      �� $��0��6   $��0,0,0,.3q�5R      
�������$��8,0,8,0q�5T      $��40��5U      ._�5V      �� $
��Left=�6   $ 
Horizontal=�5W      .+�5X      �� $��
NI Devices��6   $	��160��5Y      $��Center=�5Z      $
��Left=�5[      $��Medium"�5\      
�������5]      �� 5^      -   5        
������ $
��🗘��5_      $��.6q�5`      
�������$��15(�5b      $��24��5c      $��24��5d      5e      5f      5g      �� 5h      -   5        	 ��Name$	 IoDeviceList��5h      $��1��6/   $��Center=�6<   $ Vertical=�5i      .+�5j      �� .��6   No Ni Devices6'   5k      �� 5l      $��2��6   $��0,0,0,.3q�5m      
�������$��8,0,8,0q�5o      $��40��5p      ._�5q      �� $
��Left=�6   $ 
Horizontal=�5r      .+�5s      �� $��Transponder��6   $	��160��5t      $��Center=�5u      $
��Left=�5v      $��Medium"�5w      
�������5x      �� 5y      -	   5        
������ $��.6q�5z      
�������$	��＋��5|      $��15(�5}      $
��Bold"�5~      $��24��5      $��24��5�      ��5�      �� *AddDeviceCommand+5�      5�      5�      �� 5�      -
   5        
 ��Name$
 
DeviceList��5�      $��3��6/   $��Center=�6<   ��5�      �� *DisplayDevices+5�      �� 5�      $��4��6   $��0,0,0,.3q�5�      
�������$��8,0,8,0q�5�      $��50��5�      ._�5�      �� $ Vertical=�6   .+�5�      �� $
��Left=�6!   $ 
Horizontal=�5�   !   .+�5�      �� $
��Logs��6    $	��160��5�       $��Center=�5�       $
��Left=�5�       $��Medium"�5�       
�������5�       �� 5�      -   5        
������ $��.6q�5�   !   
�������$��15(�5�   !   $
��Bold"�5�   !   $��24��5�   !   $��24��5�   !   ��5�   !   �� *SelectLogFolderCommand+��5�      �� 5�   "   $��18��6(   $#��Icons/icons-browse-folder.png��63   5�      5�      5�      5�      �� 5�      -   5        $
 LogList��5�      $��5��6,   $��Center=�5�      ��5�      �� *
DeviceLogs+5�      5�      �� 5�   
   $��1��6   $��1��6&   $��10q�5�      ��5�      �� *CurrentView+5�      �� 5�   
   -   5         ��Name$ 
BackButton��5�   .   $��1��6   $��1��6!   $
��Left=�5�      $	��Top=�5�      $��10q�5�      
������ $	��↩��5�      $	��0.6q�5�      
�������$��13(�5�      $��Light"�5�      $��40��5�      $��40��5�      y�5�      �� *BackButtonVisibility+��5�      �� *BackButtonCommand+5�      5�      