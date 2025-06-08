using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private HomeController controller = new HomeController();
        private Setup setup;

        public Home()
        {
            InitializeComponent();
            if (Properties.Settings.Default.playerName == "")
            {
                CheckPlayerName();
            }
            txt_pseudo.Text = Properties.Settings.Default.playerName;
        }

        private void Btn_solo_Click(object sender, RoutedEventArgs e)
        {
            CheckPlayerName();
            controller.PlaySolo();
            setup = new Setup(controller.setupP1.size);
            MainWindow.LoadPage(setup);
            setup.btn_next.Click += new RoutedEventHandler(StartGame);
            setup.btn_back.Click += new RoutedEventHandler(Back);
        }

        private void Btn_online_Click(object sender, RoutedEventArgs e)
        {
            CheckPlayerName();
            Online online = new Online();
            MainWindow.LoadPage(online);
        }

        private void Btn_credits_Click(object sender, RoutedEventArgs e)
        {
            CheckPlayerName();
            MainWindow.LoadPage(new Credits());
        }

        private void Btn_settings_Click(object sender, RoutedEventArgs e)
        {
            CheckPlayerName();
            MainWindow.LoadPage(new Settings());
        }

        private void Btn_demo_Click(object sender, RoutedEventArgs e)
        {
            CheckPlayerName();
            controller.PlayDemo();
            StartGame();
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            controller.setupP1.playerName = Properties.Settings.Default.playerName;
            controller.setupP1.grid = setup.controller.grid;
            StartGame();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.LoadPage(new Home());
        }

        /// <summary>
        /// Start a game
        /// </summary>
        /// <param name="dependencyObject"></param>
        public void StartGame()
        {
            Player[] players = new Player[2];
            players[0] = new Player(controller.setupP1.grid, controller.setupP1.playerName);
            players[1] = new Player(controller.setupP2.grid, controller.setupP2.playerName);
            MainWindow.LoadPage(new Game(controller.gameType, controller.setupP1.size, players));
        }

        private void CheckPlayerName()
        {
            if (txt_pseudo.Text == "")
            {
                Properties.Settings.Default.playerName = Environment.UserName;
            }
            else
            {
                Properties.Settings.Default.playerName = txt_pseudo.Text;
            }
            Properties.Settings.Default.Save();
        }
    }
}
