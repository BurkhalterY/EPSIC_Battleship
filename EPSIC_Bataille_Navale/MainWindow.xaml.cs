using EPSIC_Bataille_Navale.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Page currentPage;

        public MainWindow()
        {
            InitializeComponent();
            //WindowState = WindowState.Maximized;
            //WindowStyle = WindowStyle.None;
            Home home = new Home();
            Content = home;
            currentPage = home;
            LoadPage(home);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public static void LoadPage(Page page)
        {
            GetWindow(currentPage).Content = page;
            currentPage = page;
        }
    }
}
