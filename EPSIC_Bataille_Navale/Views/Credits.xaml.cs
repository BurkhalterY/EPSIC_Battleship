﻿using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Interaction logic for Credits.xaml
    /// </summary>
    public partial class Credits : Page
    {
        public Credits()
        {
            InitializeComponent();
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoadPage(new Home());
        }
    }
}
