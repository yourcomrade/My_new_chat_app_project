   M S B A M L   `   `   `  ����V  RProject_chat_app_and_server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Project_chat_app_and_serverProject_chat_app_and_server  5   	   ]3clr-namespace:Project_chat_app_and_server.ViewModel%Project_chat_app_and_server.ViewModel  5   	   ,    &Project_chat_app_and_server.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:Project_chat_app_and_server     5   	   I	viewModel3clr-namespace:Project_chat_app_and_server.ViewModel     5   	     � Title$  Chat_app_client��5	   	   $	��550��6!   $	��825��6.    � 
ResizeMode$ 	CanResize=�5
   	    � DataContext 5   
   ;   5Project_chat_app_and_server.ViewModel.MyMainViewModel  5   
   6&   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��400 �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $	��200 �6    �� 5      $
��Auto �6    .+�5      � $��0��6   .+�5      �  5      �� 5      $	��200 �6+   �� 5      $	��200 �6+   .+�5      � $��0��6   .+�5       ��  �� Orientation$ Vertical=�6%   .+�5!      �� $��5q�6(   Enter IP Address of server6P   �� 5"      $��5q�6&   ��61   �� *	IPaddress+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=��� 5#      $��5q�6(   
Enter port6?   �� 5$      $��5q�6&   ��61   �� *	portnum+$ TwoWay=��� 5%      $��5q�6(   Enter username6C   �� 5&      -   5        $��
myUsername��5&   1   $��5q�6&   ��6C   �� *myProfile.Username+$ TwoWay=�$ PropertyChanged=�5'      5(      � 5)      $��1��6   .+�5*      �� $��30q�6!   ._�5+      �� $ Vertical=�6)   .+�5,   "   �� $��20��6)   $��Connect��65   $��10q�6G   ��6S   �� *Init_connect_to_server+�� 5-   "   $��	0,10,0,10q�6,   ��6?   �� *
Register+�� 5.   "   $��20��6)   $��
Disconnect��65   $��10q�6J   ��6V   �� *
Disconnect+5/      50      51      53      55      � 56      $��1��6   .+�57      �� $ Vertical=�6   .+�58      �� $��Visible=�6#   $	��285��6I   .��59      �� 
������    ItemContainerStyle#��[  5;   #     ViewItemTemplate#��[ 5<   #   ��59   #   �� *
users_list+5<   #   5=      � 5>      .+�5?      �� $ 
Horizontal=�6%   .+�5@      �� -   5        $��ButtonLoadAva��5@   q   $��Avatar��6%   $��5q�66   $��50��6A   $��50��6M   6q   �� 5A      $
��Auto��6K   $
��Auto��6X   ��6$   �� *myProfile.Username+6X   5C      5D      5E      5F      5G      � 5H   
   $��1��6    5I      �� 5J      $	��480 �6   �� 5K      $
��Auto �6   .+�5M      � $��0��6   $	��415��6    .+�5N      �� $��Visible=�6   .��5O      ��    ChatbubbleItemContainerStyle#��[ 5P        ChatBubbleTemplate#��[ 5Q      ��5O      �� *chat_bubbles+5Q      5R      5S      � 5T      $��1��6   .+�5U      �� $ 
Horizontal=�6   .+�5V      �� $��3q�6    �� CornerRadius$ 2��6(   ._�5W      �� $��3q�6"   $��60��6-   $	��300��69   	 �� TextWrapping$
	 Wrap=�6E   ��6Y   �� *TextMessage+$ TwoWay=�$ PropertyChanged=�5X      �� 5Y      $��3q�6   $ 2��6(   ._�5Z      �� $��40��6!   ��6,   �� *Send_message+��5[      �� 5\   "   
 �� ImageSource$
 Icon/send_icon.png��6-    j� Stretch$
 Fill=�6N   6_   5^      5_      �� 5`      $��3q�6   $ 2��6(   ._�5a      �� -   5        $��40��5a   !   ��5b      �� 5c   "   $
 Icon/plus_icon.png��6-   $
 Fill=�6N   6_   5e      5f      5g      5h      5i      5j      5m      