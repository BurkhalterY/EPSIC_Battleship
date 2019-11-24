using System;
using System.Windows;
using System.Windows.Controls;
using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Online.xaml
    /// </summary>
    public partial class Online : Page
    {
        private OnlineController controller;
        public string playerName;
        public Setup setupP1;
        public Player player2;
        private GameType gameType;
        private Page currentPage;

        public Online()
        {
            InitializeComponent();
            controller = new OnlineController(this);
        }

        private void Btn_host_Click(object sender, RoutedEventArgs e)
        {
            btn_host.IsEnabled = false;
            btn_join.IsEnabled = false;
            lbl_status.Content = "IP : " + OnlineController.GetLocalIPAddress() + Environment.NewLine + "En attente d'un adversaire.";
            controller.Host();
            gameType = GameType.Host;
        }

        private void Btn_join_Click(object sender, RoutedEventArgs e)
        {
            btn_host.IsEnabled = false;
            btn_join.IsEnabled = false;
            controller.Join(txt_ip.Text);
            gameType = GameType.Client;
        }

        public void SetupGame()
        {
            setupP1 = new Setup();
            setupP1.controller.playerName = playerName;
            Window.GetWindow(this).Content = setupP1;
            setupP1.btn_next.Click += new RoutedEventHandler(controller.SendSetup);
            setupP1.btn_back.Click += new RoutedEventHandler(Back);
            currentPage = setupP1;
        }

        public void StartGame()
        {
            Game game = new Game(gameType, setupP1.size);
            controller.gameController = (NetworkGameController)game.controller;
            ((NetworkGameController)game.controller).onlineController = controller;
            Player[] players = new Player[2];
            players[0] = new Player(setupP1.controller.grid, player2.playerName);
            players[1] = new Player(player2.grid, setupP1.controller.playerName);
            game.controller.players = players;
            Window.GetWindow(currentPage).Content = game;
            game.RefreshGrid();

            if (gameType == GameType.Host)
            {
                game.clickable = true;
            }
        }

        public void Wait()
        {
            lbl_status.Content = "L'adversaire est encore en train de placer ces bateaux...";
            currentPage = this;
            Window.GetWindow(setupP1).Content = this;
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            controller.Terminate();
            Window.GetWindow(this).Content = new Home();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment quitter ?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                controller.Terminate();
                Window.GetWindow(setupP1).Content = new Home();
            }
        }
    }
}
