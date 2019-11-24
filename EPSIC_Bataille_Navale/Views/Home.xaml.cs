using EPSIC_Bataille_Navale.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private Setup setupP1;
        private Setup setupP2;
        private GameType gameType;

        public Home()
        {
            InitializeComponent();
            txt_pseudo.Text = Environment.UserName;
        }

        private void Btn_solo_Click(object sender, RoutedEventArgs e)
        {
            gameType = GameType.Solo;

            setupP1 = new Setup();
            setupP1.controller.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(this).Content = setupP1;
            setupP1.btn_next.Click += new RoutedEventHandler(StartGame);
            setupP1.btn_back.Click += new RoutedEventHandler(StartGame);

            setupP2 = new Setup();
            setupP2.controller.AIChoise();
            setupP2.controller.playerName = "L'IA";
        }

        private void Btn_online_Click(object sender, RoutedEventArgs e)
        {
            Online online = new Online();
            online.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(this).Content = online;
        }

        private void Btn_credits_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Credits();
        }

        private void Btn_settings_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Settings();
        }

        private void Btn_demo_Click(object sender, RoutedEventArgs e)
        {
            gameType = GameType.Demo;
         
            setupP1 = new Setup();
            setupP1.controller.AIChoise();
            setupP1.controller.playerName = "IA1";

            setupP2 = new Setup();
            setupP2.controller.AIChoise();
            setupP2.controller.playerName = "IA2";

            StartGame(this);
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            StartGame((DependencyObject)sender);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(setupP1).Content = new Home();
        }

        /// <summary>
        /// Démarre une partie
        /// </summary>
        /// <param name="dependencyObject"></param>
        private void StartGame(DependencyObject dependencyObject)
        {
            Game game = new Game(gameType, setupP1.size);
            Player[] players = new Player[2];
            players[0] = new Player(setupP1.controller.grid, setupP2.controller.playerName);
            players[1] = new Player(setupP2.controller.grid, setupP1.controller.playerName);
            game.controller.players = players;
            Window.GetWindow(dependencyObject).Content = game;
            game.RefreshGrid();

            if (gameType == GameType.Solo)
            {
                game.clickable = true;
            }
            else if (gameType == GameType.Demo)
            {
                game.controller.Click();
            }
        }
    }
}
