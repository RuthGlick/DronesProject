   M S B A M L   `   `   `  ����   PL    PL.ParcelListView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$  Parcels list�� � Icon$= 7https://cdn-icons-png.flaticon.com/128/2526/2526496.png��
�������.��� -   $��MainGrid�� � RowDefinitions �� $	��80* ��� $	��50* ��� $	��50* ��� $
��400* �.+��� $��1��.+��  � ColumnDefinitions �� $��* ��� $��* ��� $��* �.+��� -   $��0��$��group by sender��   styleButton#��[  �� -    ��Name$ Group��$��1��$��group by target��#��[  �� -   $ clear��$��2��$��clear��$��Stretch=�#��[  � -   $��UpGrid��$��0��$��Stretch=�$
��auto��$��Stretch=�$
��auto�� �� $��* ��� $��* ��� $��* �.+��� $��0�� �� Orientation$ Vertical=�$��Stretch=�$��Stretch=�.+��� -   $ PrioritySelectorButton��$��PrioritySelector:��$��Stretch=�#��[  �� -   $��PrioritySelector��$y�	Collapsed=�$��Stretch=��� $��1��.+�� $��Stretch=� �� $��* ��� $��* �.+��� $��0��$��Stretch=�$ Vertical=�.+��� -	   $ StatusSelectorButton��$��Stretch=�$��Status:��$��Stretch=�#��[  �� -
   $��StatusSelector��$y�	Collapsed=�$��Stretch=��� -   $ DateSelectorButton��$��1��$��Date:��#��[  �� $��2��$ Vertical=�$��Stretch=�$��Stretch=�.+��� -   $ WeightSelectorButton��$��WeightSelector:��$��Stretch=�#��[  �� -   $��WeightSelector��$y�	Collapsed=�$��Stretch=�&   System.Windows.Controls.Calendar  -     Name$ 
MyCalender��   SelectionMode$ MultipleRange=�$	��200��$��0��$��3��   DisplayMode$ Month=�$y�	Collapsed=�$��Stretch=�� -   $��ListGrid��$��3�� �� $��7* ��� $��* �.+��� -   	 ��Name$	 ParcelsListView��
�������
�������$��Center=�$��0��
 �� SelectionMode$
 Single=����� *
ParcelView+����  �� 
TargetType" ��.���  �� Property � �� �� Value$ 15(���  � ��$ 35����  � ��$ 5q���  � ��
 �������  � �� ��  �� 
TargetType" ��.R��� #��z��._��� $��Center=�$��Center=� �� Triggers P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  � ��
 �   ���  � ��
 �"�"� �� 
GroupStyle (  "System.Windows.Controls.GroupStyle     HeaderTemplate �� .R��� $
��Bold"�$��14(����� *Name+� -   $��bottom��$��1��$��Stretch=�$
��auto��$��Stretch=�$
��auto�� �� $��* ��� $��* �.+��� -   $��0��$��
add parcel��#��[  �� -   $��1��$��close��#��[  