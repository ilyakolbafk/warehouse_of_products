   M S B A M L   `   `   `  ����=  91C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:_1C_1C  5   	       _1C.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:_1C     5   	   -   5          � Title$#  1С: Производство��5   	   $	��500��6"   $	��900��62   .��5	      � .+�5
   
   R� -   5         R�Name$ Registry��5
      $	��300��6%   $
��Left=�61    � ContextMenu 5      �� 5      -   5         ��Name$ TreeViewContextMenu��5      .@�5      m� -   5         m�Name$ 
AddSection��5      $!��Создать раздел��63   6K   m� 5      -   5        $ EditSection��5      $#��Изменить раздел��64   6M   m� 5      -   5        $ RemoveSection��5      $!��Удалить раздел��66   6N   5       � 	Resources 5       �� DataType   _1C.Section �� ItemsSource��%}   (��       5   +   *, +)�� 6   "  6+    5   +   �� *
Sections+.R�5      �� ��6    �� *Name+5      5      5      �� 5   
   -   5         ��Name$
 Save��5      $��Сохранить��6   ${�False��63   $��30��6E   $��Bottom=�5      $
��Left=�6,   $��	25,0,0,25q�5      $	��100��6$   60   �� 5   
   -   5        $
 Load��5      $��Загрузить��6   $��30��63   $��Bottom=�5      $
��Left=�6,   $��
175,0,0,25q�5       $	��100��6%   61   &   System.Windows.Controls.DataGrid  5!   
   -	   5        	  Name$	 ProductGrid��5"   '   $��	300,0,0,0q�6   
   AutoGenerateColumns$
 False��5#         CanUserAddRows$ False��60      
IsReadOnly$
 True��6G      Columns 5$      0  *System.Windows.Controls.DataGridTextColumn  5%      ,  &System.Windows.Controls.DataGridColumn   Header$0 *Название товара/услуги��6%      MinWidth$	 200Q�5&   %      MaxWidth$	 400Q�64   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5%   E   ��  �� Path$
 Name�5&   4     5'      $ Артикул��6%   $	 100Q�5(   %   $	 150Q�64    5'   6   �� $
 Code�5(   4     5)      $ Цена��6%   $	 100Q�5*   %   $	 150Q�64    5)   3   �� $ Price�5*   4     5+      $ Количество��6%   $	 100Q�5,   %   $	 150Q�64    5+   9   �� $ Number�5,   4     5-      $ Описание��6%   $	 100Q�5.   %   $	 400Q�64    5-   7   �� $ Description�5.   4    50      �� 51      -
   5        $ DataGridContextMenu��51      .@�52      m� -   5        $ 
AddProduct��52      $!��Добавить товар��63   6K   m� 53      -   5        $ EditProduct��53      $+��%Редактировать товар��64   6Q   m� 54      -   5        $ RemoveProduct��54      $��Удалить товар��66   6M   m� 55      -   5        $ OpenPicture��55      $%��Открыть картинку��64   6N   56      58      �� 59   
   -   5        $ 
RandomFill��59      $ ��Сгенерировать��6%   $��30��6=   $��Bottom=�5:      $
��Left=�6,   $��
325,0,0,25q�5;      $	��100��6%   61   �� 5<   
   -   5        $ Report��5<      $��
Отчет��6!   ${�False��61   $��30��6C   $��Bottom=�5=      $��Right=�6,   $��	0,0,25,25q�5>      $	��100��6$   60   5?      5@      