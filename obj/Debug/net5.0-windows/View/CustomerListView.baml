   M S B A M L   `   `   `  ����  PL    PL.CustomerListView   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      localclr-PL     5      -   5        
�������  � Title$  Customers list��5	       � Icon$= 7https://cdn-icons-png.flaticon.com/128/2526/2526496.png��5
      .��5      � -   5        $��MainGrid��5       � RowDefinitions 5   
   �� 5      $	��50* �6   �� 5      $
��400* �6   .+�5   
   �� $��0��6   $��Our Customers��6!   $��Center=�66   $��Center=�6Q   $��40(�5      6$   � 5   
   -   5        $��ListGrid��5      $��1��6    5      �� 5      $��7* �6    6.   �� 5      $��* �6    6-   .+�5      �� -   5         ��Name$ CustomersListView��5      
�������
�������$��Center=�5   h   $��0��5   2    �� SelectionMode$ Single=�6?   ��5      �� *	Customers+��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value$ 15(�65   �� 5       � ��$ 35��63   �� 5        � ��$ 5q�63   �� 5!       � ��
 �����67   �� 5"       � �� 5#      �� 5$   "    �� 
TargetType" ��62   .R�5%   &   �� #��z��6-   ._�5&   *   �� $��Center=�6;   $��Center=�6X   5'   '   5(   #   5*      	 �� Triggers	 5+      P� 5,      
 P� Property
 �@J�IsMouseOver P� Value$
 True��6=   . �5-   "   ��  � ��
 �   �6?   �� 5.   "    � ��
 �"�"�6?   5/      51      54      � 55      -   5        $��bottom��55   !   $��1��6   $��Stretch=�6/   $
��auto��6M   $��Stretch=�6[   $
��auto��6w    � ColumnDefinitions 56      �� 57      $��* �6'   �� 58      $��* �6'   .+�5:      �� -   5        $��0��5:      $��add customer��6)      styleButton#��[  6V   6}   �� 5;      -   5        $��1��5;      $��close��6)   #��[  6N   6u   5<      5=      5>      5?      