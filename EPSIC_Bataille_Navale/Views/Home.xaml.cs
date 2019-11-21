using EPSIC_Bataille_Navale.Models;
using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            txt_pseudo.Text = WindowsIdentity.GetCurrent().Name;
        }

        public void SetTitle(string title)
        {
            lbl_title.Content = title;
        }

        private void Btn_solo_Click(object sender, EventArgs e)
        {
            gameType = GameType.Solo;

            setupP1 = new Setup();
            setupP1.controller.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = setupP1;
            setupP1.btn_next.Click += new RoutedEventHandler(StartGame);

            setupP2 = new Setup();
            setupP2.controller.AIChoise();
        }

        private void Btn_online_Click(object sender, EventArgs e)
        {
            /*OnlineConfig onlineConfig = new OnlineConfig();
            onlineConfig.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = onlineConfig;*/
        }

        private void Btn_credits_Click(object sender, EventArgs e)
        {
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = new Credits();
        }

        private void Btn_settings_Click(object sender, EventArgs e)
        {
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = new Settings();
        }

        private void Btn_demo_Click(object sender, EventArgs e)
        {
            gameType = GameType.Demo;
         
            setupP1 = new Setup();
            setupP1.controller.AIChoise();
            setupP1.controller.playerName = "IA1";

            setupP2 = new Setup();
            setupP2.controller.AIChoise();
            setupP2.controller.playerName = "IA2";

            StartGame();
        }

        private void StartGame(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            Game game = new Game(gameType, setupP1.size);
            game.controller.grids = new GridModel[] { setupP1.controller.grid, setupP2.controller.grid };
            game.controller.playersNames = new string[] { setupP2.controller.playerName, setupP1.controller.playerName };
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = game;
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
