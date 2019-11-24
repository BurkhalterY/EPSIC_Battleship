using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web.Script.Serialization;
using System.Windows;

namespace EPSIC_Bataille_Navale.Controllers
{
    public enum Action
    {
        none,
        settings,
        init,
        clic,
        sonar,
        nuclearBomb
    }

    public class OnlineController
    {
        private readonly BackgroundWorker backgroundWaitClient = new BackgroundWorker();
        private readonly BackgroundWorker backgroundReceiver = new BackgroundWorker();
        public readonly BackgroundWorker backgroundSender = new BackgroundWorker(); 
        
        private Online view;
        public NetworkGameController gameController;
        private TcpListener server;
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        public Action message;
        public object objectToSend;
        public string receivedString;
        private int setupsOk = 0;

        public OnlineController(Online view)
        {
            this.view = view;

            backgroundWaitClient.DoWork += WaitClient;
            backgroundWaitClient.WorkerReportsProgress = true;
            backgroundWaitClient.ProgressChanged += StopWaitClient;
            backgroundWaitClient.WorkerSupportsCancellation = true;

            backgroundReceiver.DoWork += ReceiveBackground;
            backgroundReceiver.WorkerReportsProgress = true;
            backgroundReceiver.ProgressChanged += UpdateFromReceive;
            backgroundReceiver.WorkerSupportsCancellation = true;

            backgroundSender.DoWork += SendBackground;
        }

        /// <summary>
        /// Démarre le serveur
        /// </summary>
        public void Host()
        {
            backgroundWaitClient.RunWorkerAsync();
        }

        /// <summary>
        /// Rejoint une partie à l'IP indiquée
        /// </summary>
        /// <param name="ip"></param>
        public void Join(string ip)
        {
            try
            {
                client = new TcpClient();
                IPEndPoint server = new IPEndPoint(IPAddress.Parse(ip), 5001);
                client.Connect(server);

                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());
                writer.AutoFlush = true;
                backgroundReceiver.RunWorkerAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Hôte introuvable.");
                view.btn_host.IsEnabled = true;
                view.btn_join.IsEnabled = true;
            }
        }

        private void WaitClient(object sender, DoWorkEventArgs e)
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 5001);
                server.Start();
                client = server.AcceptTcpClient();
                backgroundWaitClient.ReportProgress(0);
            }
            catch (SocketException)
            {
                backgroundWaitClient.ReportProgress(1);
            }
            catch (ObjectDisposedException) { }
        }

        private void StopWaitClient(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());
                backgroundReceiver.RunWorkerAsync();

                message = Action.settings;
                objectToSend = new Dictionary<string, string>() {
                    { "size", Properties.Settings.Default.size.ToString() },
                    { "boatsList", Properties.Settings.Default.boatsList },
                    { "nbMines", Properties.Settings.Default.nbMines.ToString() },
                    { "iaSleepTime", Properties.Settings.Default.iaSleepTime.ToString() },
                    { "nbSonars", Properties.Settings.Default.nbSonars.ToString() },
                    { "nbNuclearBombs", Properties.Settings.Default.nbNuclearBombs.ToString() },
                    { "nuclearBombRange", Properties.Settings.Default.nuclearBombRange.ToString() }
                };
                backgroundSender.RunWorkerAsync();
                view.SetupGame();
            }
            else
            {
                view.lbl_status.Content = "";
                MessageBox.Show("Impossible de créer la partie.");
                view.btn_host.IsEnabled = true;
                view.btn_join.IsEnabled = true;
            }
        }
        
        private void ReceiveBackground(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    int message = int.Parse(reader.ReadLine());
                    receivedString = reader.ReadLine();
                    backgroundReceiver.ReportProgress(message);
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private void UpdateFromReceive(object sender, ProgressChangedEventArgs e)
        {
            Action message = (Action)e.ProgressPercentage;
            if (message != Action.none && receivedString != null)
            {
                switch (message)
                {
                    case Action.settings:
                        Dictionary<string, string> settings = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(receivedString);
                        Properties.Settings.Default.size = int.Parse(settings["size"]);
                        Properties.Settings.Default.boatsList = settings["boatsList"];
                        Properties.Settings.Default.nbMines = int.Parse(settings["nbMines"]);
                        Properties.Settings.Default.iaSleepTime = double.Parse(settings["iaSleepTime"]);
                        Properties.Settings.Default.nbSonars = int.Parse(settings["nbSonars"]);
                        Properties.Settings.Default.nbNuclearBombs = int.Parse(settings["nbNuclearBombs"]);
                        Properties.Settings.Default.nuclearBombRange = double.Parse(settings["nuclearBombRange"]);
                        Properties.Settings.Default.Save();
                        view.SetupGame();
                        break;
                    case Action.init:
                        view.player2 = new JavaScriptSerializer().Deserialize<SendablePlayer>(receivedString).ToPlayer();
                        StartGame();
                        break;
                    case Action.clic:
                        int[] coord = new JavaScriptSerializer().Deserialize<int[]>(receivedString);
                        gameController.ClickAt(coord[0], coord[1]);
                        gameController.CheckWinAndTurn();
                        break;
                    case Action.sonar:
                        int[] coord3 = new JavaScriptSerializer().Deserialize<int[]>(receivedString);
                        gameController.Sonar(coord3[0], coord3[1]);
                        gameController.CheckWinAndTurn();
                        break;
                    case Action.nuclearBomb:
                        int[] coord2 = new JavaScriptSerializer().Deserialize<int[]>(receivedString);
                        gameController.NuclearAttack(coord2[0], coord2[1]);
                        gameController.CheckWinAndTurn();
                        break;
                }
            }   
        }

        private void SendBackground(object sender, DoWorkEventArgs e)
        {
            try {
                writer.WriteLine((int)message);
                writer.WriteLine(new JavaScriptSerializer().Serialize(objectToSend));
                writer.Flush();

                message = Action.none;
                objectToSend = null;
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Fonction à appeller pour fermer le serveur et autres objets réseaux
        /// </summary>
        public void Terminate()
        {
            backgroundWaitClient.CancelAsync();
            backgroundReceiver.CancelAsync();
            if (reader != null) reader.Close();
            if (writer != null) writer.Close();
            if (client != null) client.Close();
            if (server != null) server.Stop();
        }

        public void SendSetup(object sender, RoutedEventArgs e)
        {
            Player player = new Player(view.setupP1.controller.grid, view.setupP1.controller.playerName);
            message = Action.init;
            objectToSend = new SendablePlayer(player);
            backgroundSender.RunWorkerAsync();
            view.Wait();
            StartGame();
        }

        /// <summary>
        /// Lance une partie si les 2 joueurs ont placé leurs bateaux
        /// </summary>
        private void StartGame()
        {
            setupsOk++;
            if(setupsOk == 2)
            {
                view.StartGame();
            }
        }

        /// <summary>
        /// Retourne l'adresse IP locale
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "erreur";
        }
    }
}
