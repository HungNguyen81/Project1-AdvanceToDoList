   M S B A M L   `   `   `  ����E  AAdvanceTDL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:AdvanceTDL
AdvanceTDL  5   	       AdvanceTDL.RemindDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:AdvanceTDL     5   	     � Title$  RemindDialog��5   	   $	��474��5	   	   $	��772��6    � WindowStyle$
 None=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � Topmost .  � 	Resources 5         
MyOKButton �� 
TargetType �� Property �� Value �� 
TargetType	 �� Triggers
 P� Property P� Value��%�  &	        �� 5   
   " ��6#   .�5      ��  �@�OverridesDefaultStyle . 66   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $��border��6!   $��0q�6/   
���   �#��z��6W   ._�5      �� $��Center=�6/   $��Center=�6L   5      	 5      P� 5      
 �@J�IsMouseOver . . �5   "   ��  �@J�Opacity$	 0.8Q�6<   5      5      5      5      5       .��5!      �� ��5"   
   �� 5#       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5$      �  � Color$ 	#FF6C0707��6   � 5%      $ 	#FFFF5E02��6    � Offset$ 1Q�61   5&      .+�5(   
   ��  �� Left$	 542��5)      
 �� Top$	 354��5*      
���ݠ��$��5q�5,       �� CornerRadius$ 25��5-       J� Effect 5.      3  -System.Windows.Media.Effects.DropShadowEffect  5/      ._�51      �� -   5         ��Name$ btn_OK��52      $��OK��53      $	��162��55      $��57��6   $��35(�56      $
��Bold"�57      
������ 
���ݠ��#��[  5:       5;        5<      5>      5?      �� 5@   
   $��50(�5A      $��Center=�5B      $ 37��5C      $ 21��6   
���ݠ���� 5E      .�6   THÔNG BÁO6    5F        5G      5I      �� 5J   
   -   5         ��Name$ tx_tenSK��5K      $��30(�5L      $��DemiBold"�5M      
�������$ 37��5O      $	 120��6   Sự kiện:5Q      �� 5R   
   -   5        $ tx_thoigian��5S      $��30(�5T      $��DemiBold"�5U      
�������$
��Wrap=�5W      $ 37��5X      $	 180��6   Thời gian:5Z      �� 5[   
   -   5        $ tx_vitri��5\      $��30(�5]      $��DemiBold"�5^      
�������$ 37��5`      $	 280��6   
Vị trí:5b      5c      5d      