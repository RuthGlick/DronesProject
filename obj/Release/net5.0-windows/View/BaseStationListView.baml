   M S B A M L   `   `   `  ����   PL    PL.BaseStationListView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     localclr-PL     -   
�������  � Title$  Base stations list�� � Icon$= 7https://cdn-icons-png.flaticon.com/128/2526/2526496.png��.��� -   $��MainGrid�� � RowDefinitions �� $	��50* ��� $
��400* �.+�� $��0�� � ColumnDefinitions �� $��* ��� $��* �.+��� -   $��clear��$��0��$��clear��   styleButton#��[  �� -   $��1��$,��&group by sum of available charge slots��#��[  $��25(�� -   $��ListGrid��$��1�� �� $��7* ��� $��* �.+��� -    ��Name$ BaseStationsListView��
�������
�������$��Center=�$��0�� �� SelectionMode$ Single=����� *BaseStationView+����  �� 
TargetType" ��.���  �� Property � �� �� Value$ 15(���  � ��$ 35����  � ��$ 5q���  � ��
 �������  � �� �� 	 �� 
TargetType"	 ��.R��� #��z��._��� $��Center=�$��Center=�
 �� Triggers
 P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  � ��
 �   ���  � ��
 �"�"� �� 
GroupStyle (  "System.Windows.Controls.GroupStyle     HeaderTemplate �� .R��� $
��Bold"�$��14(����� *Name+� -   $��bottom��$��1��$��Stretch=�$
��auto��$��Stretch=�$
��auto�� �� $��* ��� $��* �.+��� -   $��0��$��add base station��#��[  �� -	   $��1��$��close��#��[  