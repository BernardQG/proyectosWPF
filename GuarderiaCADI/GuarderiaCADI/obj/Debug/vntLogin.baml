   M S B A M L   `   `   `  ����H  DGuarderiaCADI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:GuarderiaCADIGuarderiaCADI  5   	       GuarderiaCADI.vntLogin   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:GuarderiaCADI     5   	   U QMaterialDesignThemes.Wpf, Version=2.3.1.953, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   $	��500��5      $	��350��6'     � WindowStyle$
  None=�63    � 
ResizeMode$ NoResize=�6F    � WindowStartupLocation$ CenterScreen=�6\   .��5	      � ���	#FFF0F0F0.+�5
   
   �� -   5         ��Name$ 
Rectangulo��5
   9   $	��250��6   $	��Top=�6!   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5      �  � Color$ 	#FF5B0D5F��6#    � Offset$ 1Q�65   � 5      $ 	#FF921DF3��6#   � 5      $ 	#FF78249F��6#   $ 0.537Q�65   5      5      �� 5   
   -   5         ��Name$ btnClose��5   �   $	��Top=�6   $��Right=�6)   $��7q�6G   $��30��6R   $��30��6]    0  ,MaterialDesignFloatingActionMiniAccentButton#��[  6�   ��6i   Z� ��6   Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.3.1.953, Culture=neutral, PublicKeyToken=null]]	   Kind$	 Power=�6&   
�������$��20��6G   $��20��6R   $��Center=�6^   $��Center=�6y   5        5   
   $	 AccountCircle=�6"   $	��100��68   $	��100��6D   
�������$	��Top=�6e   $��Center=�6}   $��45q�6�   �� 5   
   $	��280��6   $	��240��6    
�������$��Bottom=�6:   $��Center=�6U   $
��0,90q�6r   
 �� RadiusY$
 15.022��6�    �� RadiusX$ 15.022��6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ 	#FFBBBBBB��6#      	Direction$ 0Q�65      
BlurRadius$ 15Q�6C      RenderingBias$ Quality=�6S      ShadowDepth$ 1Q�6k   5      �� 5   
   $	��280��6   $	��240��6!   $��Bottom=�6.   $
��0,90q�6I   .+�5      �� $��Iniciar Secíón��6   $	��Top=�6.   $��Center=�6F   $��10q�6c   $��20(�6o   
�������6}   �� 5       $��Center=�6   $
��0,15q�66    �� Orientation$ 
Horizontal=�6D   .+�5!        $	 Account=�6*   $��Center=�69   $��0,15,0,0q�6T   ���#7070706f   �� 5"      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Usuario��6    % !MaterialDesignFloatingHintTextBox#��[ 6C   $	��200��6~   $��5q�6�   $��Center=�6�   5#      �� 5$      $��Center=�6   $ 
Horizontal=�67   .+�5%        $		 Key=�6*   $��Center=�65   $��0,15,0,0q�6P   ���#7070706b   G� 5&      $ Contraseña��6    ) %MaterialDesignFloatingHintPasswordBox#��[ 6J   $	��200��6�   $��5q�6�   $��Center=�6�   5'      5)      �� 5*   
   -   5        $ btnContinuar��5*   �   $��Bottom=�6   $��Center=�6,   $��	Continuar��6I   $	��150��6]   $
��0,75q�6i   $��18(�6w   6�   5+      5,      