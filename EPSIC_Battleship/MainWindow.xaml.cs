﻿using EPSIC_Battleship.I18n;
using EPSIC_Battleship.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EPSIC_Battleship
{
    public partial class MainWindow : Window
    {
        private static Page currentPage;
        private static bool FullScreen;

        public MainWindow()
        {
            if (Environment.OSVersion.Version.Major < 10) // Not Windows 10
            {
                try
                {
                    Application.Current.Resources.Source = new Uri("/PresentationFramework.Aero2;component/themes/Aero2.NormalColor.xaml", UriKind.RelativeOrAbsolute);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show(Strings.ErrMissingPresentationFrameworkAero2DLL);
                }
            }
            InitializeComponent();
            Home home = new Home();
            Content = home;
            currentPage = home;
            LoadPage(home);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.F11))
            {
                if (FullScreen)
                {
                    ResizeMode = ResizeMode.CanResize;
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    FullScreen = false;
                }
                else
                {
                    ResizeMode = ResizeMode.NoResize;
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                    FullScreen = true;
                }
            }
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
