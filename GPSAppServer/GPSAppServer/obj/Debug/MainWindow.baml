   M S B A M L   `   `   `  ����d  `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF  5   	   G CGPSAppServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:GPSAppServerGPSAppServer 5   	      GPSAppServer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   `mRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF     5   	   ,localclr-namespace:GPSAppServer     5   	   -   5          � Title$  
MainWindow��5	   	   $
��Auto��6   $
��Auto��6*    � WindowStyle$
 None=�67    � WindowState$ 	Maximized=�6J    � 
ResizeMode$ CanResizeWithGrip=�6b   .��5
      � -   5         � ColumnDefinitions 5   
   �� 5      -   5         ��Name$ MapGrid��5      $��3* �60   6=   �� 5      -   5        $ ClientListGrid��5      $��* �67   6C   .+�5   
   � $��0��6   .+�5      '   !Microsoft.Maps.MapControl.WPF.Map  -   5          Name$ MyMap��5      +   %Microsoft.Maps.MapControl.WPF.MapCore   CredentialsProviderT   NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$F @AgW8SdR6t8nw_5Wo3ZCNFlRrUHzw-YF6MxeAzecIiRAaQ454o1x_PsNpLuM3f0ll 6#      Mode;   5Microsoft.Maps.MapControl.WPF.Design.MapModeConverter$
 Road 6z   5      5      � 5   
   $��1��6   	 � RowDefinitions	 5      �� 5      -   5        
 ��Name$
 	DesignRow��5       $��* �63   6@   �� 5      -   5        $
 MainRow��5       $��4* �61   6?   .+�5      �� $��0��6   ��5      �� 5       �� ImageSource$  /Resources/background1.jpg��6!    �� Opacity$
 0.75Q�6J   6[   5      �� 5      $��Center=�6    �� Orientation$ Vertical=�64   .+�5      �� $��Center=�6   $��Center=�65   $��GPS APPLICATION SERVER��6P   $��Segoe UI)�6q   $��25(�6�   $
��Bold"�6�   
�������6�   �� 5      $��40��6   ��5       �� 5!      $ map.ico��6%    j� Stretch$ Uniform=�6;   5#      5$      &   System.Windows.Controls.DataGrid  5%      -   5          Name$ 	MainBoard��5%      $��1��6*   $��Stretch=�67   $��Stretch=�6V      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6{   5&      5'      5(      5)      