   M S B A M L   `   `   `  ����H  DGuarderiaCADI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:GuarderiaCADIGuarderiaCADI  5   	        GuarderiaCADI.vntGuarderia   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   U QMaterialDesignThemes.Wpf, Version=2.3.1.953, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   'localclr-namespace:GuarderiaCADI  5   	   $	��600��5      $
��1024��6'     � 
ResizeMode$  NoResize=�64    � WindowStartupLocation$ CenterScreen=�6J    � WindowStyle$
 None=�6o    � Icon$ Assets/icono.png��6�    � 	Resources 5	         ScrollThumbs �� 
TargetType �� Property �� Value �� 
TargetType	 �Name
 ��Name �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton P� 
SourceName  ButtonSelectBrush  	DarkBrush �� Angle�� 5
   
   %�  &	        '�� D    �� 5      " n�6)   .�5      ��  � �� 5      �� 5      " n�6*   .R�5      � $
	 Grid��6#   .+�5   "   �� $��Stretch=�6,   $��Stretch=�6J   $
��Auto��6f   $
��Auto��6s   
������ 6�   �� 5   "   $
 
Rectangle1��6)   $ 10��6=   $��Stretch=�6O   $��Stretch=�6m   $
��Auto��6�   $
��Auto��6�   #��z��6�   5       5      P� 5   "    �@�Tag$ 
Horizontal��69   . �5   &   �� $ 
Rectangle1��6-    � ��$
 Auto��6V   �� 5   &   $ 
Rectangle1��6-    � ��$ 7��6W   5   #   5      5      5      �� 5      " ��6/   .�5       ��  �@��IsFlicksEnabled .  6;   �� 5!       � ��
 �����6/   �� 5"       � ��
 �����6/   �� 5#       � ��$ 10��6*   �� 5$       � �� 5%      �� 5&      " ��6*   .R�5'      � $	 GridRoot��6#   $��19��65   ��6@   Z� .+�5(   "   ]� $ 
PART_Track��6(   $��0��6<   ��. |�.   5)   &   n� 5*   *   $ Thumb��60   #��z��6?   #���   6i    5,   &   �� 5-   *   $ PageUp��67   $��ScrollBar.PageDownCommand��6G   $ 0Q�6k   |�.  6w    5/   &   �� 50   *   $ PageDown��67   $��ScrollBar.PageUpCommand��6I   $ 0Q�6k   |�.  6w   52   #   53       55      P� 56   "   $ Thumb��6*    �@J�IsMouseOver . . �57   &   �� # �  6-   $ Thumb��6Y    � ��6l   58   #   P� 59   "   $ Thumb��6*    �@n�
IsDragging . . �5:   &   �� # �  6-   $ Thumb��6Q    � ��6d   5;   #   P� 5=   "    � {� .  . �5>   &   �� $ Thumb��6-    � y�$ 	Collapsed=�6V   5?   #   P� 5@   "    �@��Orientation$ 
Horizontal=�6A   . �5A   &   �� $ GridRoot��6-    �@�LayoutTransform 5B   *   �� 5C   .   $	 -90Q�6>   5E   '   �� 5F   &   $ 
PART_Track��6-    �@�LayoutTransform 5G   *   �� 5H   .   $	 -90Q�6>   5J   '   �� 5K   &    � ��$
 Auto��6>   �� 5L   &    � ��$ 12��6?   �� 5M   &   $ Thumb��6-    �@�Tag$ 
Horizontal��6O   �� 5N   &   $ PageDown��6-    � ��$ ScrollBar.PageLeftCommand��6V   �� 5O   &   $ PageUp��6-    � ��$  ScrollBar.PageRightCommand��6T   5P   #   5R      5T      5U      5V      .��5Y      � ���	#FFF0F0F0.+�5[      � $��
250 60 0 0q�6   .+�5\      �� .��5]      � -   5        $	 GridBase��5]      $	��Top=�6)   $��Center=�6A   5^      5_      � 5a   
   $	��250��6   $
��Left=�6   ���	#FF323131 J� Effect 5b      3  -System.Windows.Media.Effects.DropShadowEffect  5c         
BlurRadius$ 20Q�6#      Color$ 	#FFD4D3D3��63      ShadowDepth$ 1Q�6E      RenderingBias$ Quality=�6U   .+�5e      ?  9MaterialDesignThemes.Wpf.Transitions.TransitioningContent  -   5          Name$ TrainsitionigContentSlide��5e   2   C  =MaterialDesignThemes.Wpf.Transitions.TransitioningContentBase   OpeningEffect 6U   D  >MaterialDesignThemes.Wpf.Transitions.TransitionEffectExtension  *SlideInFromLeft+    Duration$  0:0:0.2X�.��5f      � -   5        $	 
GridCursor��5f      $��10��6+   $��	0 180 0 0q�66   ���	#FFAF02FF$
��Left=�6`   $��60��6{   $	��Top=�6�   5g      �� 5h      $��0 60 0 0q�6   $	��Top=�6+   .+�5i      � .+�5k      �� ! �� Orientation$! 
Horizontal=�6!   $��Center=�6:   $��Center=�6W   .+�5l      �� $��C��6$   $��Cooper Black)�6-   
���  ��$��60(�6X   $��5q�6f   �� 5m      $��A��6$   $��Cooper Black)�6-   ���	#FFFBAE0A$��60(�6^   $��5q�6l   �� 5n      $��D��6$   $��Cooper Blacks)�6-   ���	#FF22D822$��60(�6_   $��5q�6m   �� 5o      $��I��6$   $��Cooper Black)�6-   ���	#FF0097FF$��60(�6^   $��5q�6l   5p      5q      �� 5r      -   5        " ��Name$" ListMenu��5r      $6��0/GuarderiaCADI;component/#Champagne & Limousines)�6,   
�������$
��0 40q�6�   # �� SelectedIndex$# 0��6�   .@�5s      �� $��60��6#   .��5t      �� $! 
Horizontal=�6%   .+�5u      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.3.1.953, Culture=neutral, PublicKeyToken=null]]$   Kind$
$ Home=�66   $��35��6B   $��35��6M   $��Center=�6Y   $
��10 0q�6t   �� 5v      $��	Principal��6(   $��Center=�69   $��20(�6T   $
��10 0q�6b   5w      5x      �� 5y      $��60��6#   .��5z      �� $! 
Horizontal=�6%   .+�5{        $
$ Flag=�66   $��35��6B   $��35��6M   $��Center=�6Y   $
��10 0q�6t   �� 5|      $��Grupos��6(   $��Center=�66   $��20(�6Q   $
��10 0q�6_   5}      5~      �� 5      $��60��6#   .��5�      �� $! 
Horizontal=�6%   .+�5�        $
$ Baby=�66   $��35��6B   $��35��6M   $��Center=�6Y   $
��10 0q�6t   �� 5�      $��Niños��6(   $��Center=�65   $��20(�6P   $
��10 0q�6^   5�      5�      �� 5�      $��60��6#   .��5�      �� $! 
Horizontal=�6%   .+�5�        $$ Flower=�66   $��35��6D   $��35��6O   $��Center=�6[   $
��10 0q�6v   �� 5�      $��
Educadoras��6(   $��Center=�6:   $��20(�6U   $
��10 0q�6c   5�      5�      5�      5�      5�      � 5�   
   -   5        $	 
GridTitulo��5�      $��12��6D   $	��Top=�6P   ���	#FFAF02FF6h   � 5�   
   $	��Top=�6   $��
250 20 5 0q�6'   .+�5�      �� $4��.Sistema de Gestion en Grupos de Guarderia CADI��6   $
��Left=�6N   $
��80 5q�6i   $	��Top=�6w   $��20(�6�   ���	#FF535252$6��0/GuarderiaCADI;component/#Champagne & Limousines)�6�   �� 5�      $! 
Horizontal=�6   $	��Top=�62   $��Right=�6J   .+�5�      �� -   5        % ��Name$% btnMini��5�      $��Center=�6@   $��25��6[   $��25��6f    0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 6s   $
��10 0q�6�   ��6�   Z� ��6�   Z� .��5�        $$ WindowMinimize=�6.   $��15��6D   $��15��6O   ���	#FF535252$��Center=�6s   $��Center=�6�   5�      �� 5�      -   5        $% btnMaxi��5�      $��Center=�6@   $��25��6[   $��25��6f   #��[ 6s   $
��10 0q�6�   ��6�   Z� ��6�   Z� .��5�        $$ WindowMaximize=�6.   $��15��6D   $��15��6O   ���	#FF535252$��Center=�6s   $��Center=�6�   5�      �� 5�      -   5        $% btnPower��5�      $��Center=�6B   $��25��6]   $��25��6h   #��[ 6u   $
��10 0q�6�   ��6�   Z� ��6�   Z� .��5�        $$ Power=�6.   $��15��6;   $��15��6F   ���	#FF535252$��Center=�6j   $��Center=�6�   5�      5�      5�      5�      5�      