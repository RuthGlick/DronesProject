   M S B A M L   `   `   `  ����  PL    PL.BaseStationListView   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      localclr-PL     5      -   5        
�������  � Title$  Base stations list��5	       � Icon$= 7https://cdn-icons-png.flaticon.com/128/2526/2526496.png��5
      .��5      � -   5        $��MainGrid��5       � RowDefinitions 5   
   �� 5      $	��50* �6   �� 5      $
��400* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $��* �6#   6/   �� 5      $��* �6#   6/   .+�5      �� -   5        $��clear��5      $��0��6"   $��clear��6F      styleButton#��[  6W   �� 5      -   5        $��1��5      $,��&group by sum of available charge slots��6%   #��[  6f   $��25(�6�   6�   5      � 5   
   -   5        $��ListGrid��5      $��1��6    5      �� 5      $��7* �6    6.   �� 5       $��* �6    6-   .+�5"      �� -   5         ��Name$ BaseStationsListView��5#      
�������
�������$��Center=�5"   n   $��0��5#   7    �� SelectionMode$ Single=�6D   ��5"      �� *BaseStationView+��5$      �� 5%       �� 
TargetType" ��6   .�5&      ��  �� Property � �� �� Value$ 15(�65   �� 5'       � ��$ 35��63   �� 5(       � ��$ 5q�63   �� 5)       � ��
 �����67   �� 5*       � �� 5+      �� 5,   "   	 �� 
TargetType"	 ��62   .R�5-   &   �� #��z��6-   ._�5.   *   �� $��Center=�6;   $��Center=�6X   5/   '   50   #   52      
 �� Triggers
 53      P� 54       P� Property �@J�IsMouseOver P� Value$
 True��6=   . �55   "   ��  � ��
 �   �6?   �� 56   "    � ��
 �"�"�6?   57      59       �� 
GroupStyle 5<      (  "System.Windows.Controls.GroupStyle  5=         HeaderTemplate 5>      �� 5?      .R�5@   "   �� $
��Bold"�6,   $��14(�6>   ��6L   �� *Name+5A      5C      5F      � 5G      -   5        $��bottom��5G   !   $��1��6   $��Stretch=�6/   $
��auto��6M   $��Stretch=�6[   $
��auto��6w    5H      �� 5I      $��* �6'   �� 5J      $��* �6'   .+�5L      �� -   5        $��0��5L      $��add base station��6)   #��[  6X   6   �� 5M      -	   5        $��1��5M      $��close��6)   #��[  6O   6v   5N      5O      5P      5Q      