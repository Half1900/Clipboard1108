using Clipboards.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clipboards
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        Config con=new Config();
        public Setting()
        {
            InitializeComponent();
            initial();
        }

        private void initial()
        {
            checkbox_auto_start.IsChecked = con.read_auto_start();
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool auto_start=(bool)checkbox_auto_start.IsChecked;      
            con.write("auto_start",auto_start);            
            if (auto_start == true) con.add_auto_start();
            else con.del_auto_start();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                WindowState = WindowState.Normal;
                DragMove();
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


    }
}
