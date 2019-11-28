using System.Windows;
using System.Windows.Controls;
using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using static EPSIC_Bataille_Navale.Controllers.OnlineController;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Online.xaml
    /// </summary>
    public partial class Online : Page
    {
        private OnlineController controller;
        private Setup setup;

        public Online()
        {
            InitializeComponent();
            txt_ip.Text = Properties.Settings.Default.ip;
            controller = new OnlineController();
            controller.OnEnableButtons += new EnableButtons(OnEnableButtons);
            controller.OnUpdateMessage += new UpdateMessage(OnUpdateMessage);
            controller.OnSetupGame += new SetupGame(OnSetupGame);
            controller.OnStartGame += new StartGame(OnStartGame);
            controller.OnWait += new Wait(OnWait);
        }

        private void Btn_host_Click(object sender, RoutedEventArgs e)
        {
            controller.Host();
        }

        private void Btn_join_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ip = txt_ip.Text;
            Properties.Settings.Default.Save();
            controller.Join(txt_ip.Text);
        }

        /// <summary>
        /// Crée et affiche la view de setup
        /// </summary>
        public void OnSetupGame()
        {
            setup = new Setup(Properties.Settings.Default.size);
            MainWindow.LoadPage(setup);
            setup.btn_next.Click += new RoutedEventHandler(Wait);
            setup.btn_back.Click += new RoutedEventHandler(Back);
        }

        /// <summary>
        /// Crée et affiche la game view
        /// </summary>
        public void OnStartGame()
        {
            Game game = new Game(controller.gameType, controller.player1.grid.grid.GetLength(0), new Player[] { controller.player1, controller.player2 });
            controller.gameController = (NetworkGameController)game.controller;
            ((NetworkGameController)game.controller).onlineController = controller;
            MainWindow.LoadPage(game);
        }
    
        /// <summary>
        /// Écran d'attente
        /// </summary>
        public void OnWait()
        {
            controller.player1 = new Player(setup.controller.grid, Properties.Settings.Default.playerName);
            lbl_status.Content = "L'adversaire est encore en train de\nplacer ses bateaux...";
            btn_back.IsEnabled = false;
            MainWindow.LoadPage(this);
        }
        
        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            controller.Terminate();
            MainWindow.LoadPage(new Home());
        }

        private void Wait(object sender, RoutedEventArgs e)
        {
            controller.player1 = new Player(setup.controller.grid, Properties.Settings.Default.playerName);
            controller.SendSetup();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment quitter ?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                controller.Terminate();
                MainWindow.LoadPage(new Home());
            }
        }

        public void OnUpdateMessage(string message)
        {
            lbl_status.Content = message;
        }

        public void OnEnableButtons(bool active)
        {
            btn_host.IsEnabled = active;
            btn_join.IsEnabled = active;
        }
    }
}
