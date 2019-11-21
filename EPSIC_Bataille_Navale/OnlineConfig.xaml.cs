using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour OnlineConfig.xaml
    /// </summary>
    public partial class OnlineConfig : Page
    {
        public NetworkController controller;
        public string playerName;
        public Setup setupP1;
        public Setup setupP2;

        public OnlineConfig()
        {
            InitializeComponent();
            controller = new NetworkController(this);
        }

        private void Btn_back_Click(object sender, EventArgs e)
        {
            controller.run = false;
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = new Home();
        }

        private void Btn_join_Click(object sender, EventArgs e)
        {
            controller.Join(txt_ip.Text);
        }

        private void Btn_host_Click(object sender, EventArgs e)
        {
            controller.Create();
            Setup();
        }

        public void Setup()
        {
            setupP1 = new Setup();
            setupP1.onlineView = this;
            setupP1.controller.playerName = playerName;
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = setupP1;
            setupP1.btn_next.Click += new RoutedEventHandler(SendMap);
        }

        private void SendMap(object sender, EventArgs e)
        {
            while (!controller.connected)
            {
                controller.Send("player", setupP1.controller.playerName);
                controller.Send("grid", setupP1.controller.grid);
                controller.Send("go", null);
            }
        }

        public void StartGame()
        {
            Game game = new Game(controller.gameType, setupP1.size);
            game.controller.grids = new GridModel[] { setupP1.controller.grid, setupP2.controller.grid };
            game.controller.playersNames = new string[] { setupP2.controller.playerName, setupP1.controller.playerName };
            Window.GetWindow(VisualTreeHelper.GetParent(this)).Content = game;
            game.RefreshGrid();
            game.clickable = controller.gameType == GameType.Host;
        }
    }
}
