   M S B A M L   `   `   `  ����L  HAdvanceTDL-Gadget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AdvanceTDL_GadgetAdvanceTDL_Gadget  5   	   "    AdvanceTDL_Gadget.MainGadget   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:AdvanceTDL_Gadget     5   	   -   5          � Title$  gadget��5   	   $	��412��6   $	��484��6%   
������  � WindowStyle$
 None=�5
   	    � ShowActivated .  � ShowInTaskbar .   � WindowStartupLocation$ Manual=�5   	    � AllowsTransparency . .��5      �  � ColumnDefinitions 5   
   �� 5      $	��15* �6   6-   �� 5      $��* �6   6+   .+�5   
   �� 
�������$��2q�5      $��0��5       J� Opacity$	 0.8Q�5      ._�5      �  � RowDefinitions 5      �� 5      $��1* �6$   63   �� 5      $��9* �6$   62   �� 5      $
��0.5* �6$   64   ��5      �� 5       	 �� EndPoint$		 0,1&�6*   
 �� 
StartPoint$	
 1,0&�69   .��5!      �  � Color$ 	#FF0D68DD��6'   � 5"      $ 	#FFDA251D��6'    � Offset$ 1Q�69   5#      .+�5'      �� $��15(�5(      $��Center=�5)      
���ݠ��$��117,5,115,9q�5+      �� 5,      .�6   SỰ KIỆN CỦA BẠN60    J� Effect 5-      3  -System.Windows.Media.Effects.DropShadowEffect  5.      50      �� 51      -   5         ��Name$ scroll_sukien_gadget��51      $��1��6=   $
��Auto=�52      .��53      �� -   5         ��Name$ pnlSuKien_gadget��53   !   55      56      57      58      �� 59   
   $��1��5:      .+�5;      �� $��16��5<      $��16��5=      $��1q�5>      
�������
���   � �� Left$ 1��5@   $   ._�5B      �� -   5         ��Name$ btnClose��5C      $��X��5D      $��14��5E      $��14��5F      $��8(�5G      $��2 0 1 1q�5H      
������ 
�������$��1��5K      5M      5N      �� 5O      $��16��5P      $��46��5Q      $��1q�5R      
�������
���   �$ 1��5T   $   
 �� Top$ 16��64   ._�5V      �� -   5        $ btn_launch_app��5W       J� RenderTransformOrigin$ 0.5,0.5&�5X      $��	Lauch App��5Y      $��14��6)   $��8(�5Z      $	 -11��6"   $ 31��5[      $��-15,15q�6)   
�������
���   �z�5_      V� 5`      .~�5a      �� �� 5b      �� 5c       �� Angle$	 -90Q�6.   T� 5d      5e      5g      5h      5i      5j      5k      