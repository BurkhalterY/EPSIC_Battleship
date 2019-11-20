using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.DirectoryServices.AccountManagement;
using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private HomeController controller;
        public Home()
        {
            InitializeComponent();

            // Essaye de récupérer le nom de session du joueur
            try
            {
                txt_pseudo.Text = UserPrincipal.Current.DisplayName;
            }
            catch (PrincipalServerDownException)
            {
                txt_pseudo.Text = "Player";
            }
            controller = new HomeController(this);
        }

        // Définir un titre
        public void SetTitle(string title)
        {
            lbl_title.Content = title;
        }

        // Le bouton pour lancer une partie solo
        private void Btn_solo_Click(object sender, EventArgs e)
        {
            Setup setup = new Setup(0);
            setup.controller.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(this).Content = setup;
        }

        // Le bouton pour lancer une partie online (A FAIRE)
        private void Btn_online_Click(object sender, EventArgs e)
        {
            OnlineConfig onlineConfig = new OnlineConfig();
            onlineConfig.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(this).Content = onlineConfig;
        }

        // Le bouton pour voir les crédits du jeu
        private void Btn_credits_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Content = new Credits();
        }

        // Le bouton pour lancer une démo du jeu (Avec 2 IA qui s'affronte)
        private void Btn_demo_Click(object sender, EventArgs e)
        {
            Setup setup = new Setup(1);
            setup.controller.AIChoise();

            Setup setup2 = new Setup(1);
            setup2.controller.AIChoise();

            Game game = new Game(setup.controller.grid.grid.GetLength(0), 3);
            game.controller.grids = new Models.GridModel[] { setup.controller.grid, setup2.controller.grid };
            game.controller.playersNames = new string[] { "IA1", "IA2" };
            Window.GetWindow(this).Content = game;
            game.controller.Click(0, 0);
        }
    }
}
