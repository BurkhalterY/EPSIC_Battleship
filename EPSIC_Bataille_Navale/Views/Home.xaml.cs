using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.Security.Principal;
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
            txt_pseudo.Text = WindowsIdentity.GetCurrent().Name;
            controller = new HomeController(this);
        }

        public void SetTitle(string title)
        {
            lbl_title.Content = title;
        }

        private void Btn_solo_Click(object sender, EventArgs e)
        {
            Setup setup = new Setup();
            setup.controller.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(this).Content = setup;
        }

        private void Btn_online_Click(object sender, EventArgs e)
        {
            OnlineConfig onlineConfig = new OnlineConfig();
            onlineConfig.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            Window.GetWindow(this).Content = onlineConfig;
        }

        private void Btn_credits_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Content = new Credits();
        }

        private void Btn_settings_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Content = new Settings();
        }

        private void Btn_demo_Click(object sender, EventArgs e)
        {
            Setup setup = new Setup();
            setup.controller.AIChoise();

            Setup setup2 = new Setup();
            setup2.controller.AIChoise();

            Game game = new Game(GameType.Demo, setup.size);
            game.controller.grids = new GridModel[] { setup.controller.grid, setup2.controller.grid };
            game.controller.playersNames = new string[] { "IA1", "IA2" };
            game.clickable = false;
            Window.GetWindow(this).Content = game;
            game.RefreshGrid();
            game.controller.Click();
        }
    }
}
