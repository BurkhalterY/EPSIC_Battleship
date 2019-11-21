using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web.Script.Serialization;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class NetworkController
    {
        private OnlineConfig view;
        private TcpClient client;
        private TcpListener server;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread t;
        public bool run = true;
        public bool connected = false;
        public GameType gameType;

        public NetworkController(OnlineConfig view)
        {
            this.view = view;
        }

        public void Create()
        {
            gameType = GameType.Host;

            server = new TcpListener(IPAddress.Any, 8081);
            server.Start();

            run = true;
            t = new Thread(new ThreadStart(ServerRecieve));
            t.IsBackground = true;
            t.Start();
        }

        public void Join(string ip)
        {
            gameType = GameType.Client;

            client = new TcpClient();
            client.Connect(ip, 8081);

            run = true;
            t = new Thread(new ThreadStart(ClientRecieve));
            t.Start();
        }

        public void ClientRecieve()
        {
            NetworkStream flux = client.GetStream();
            reader = new StreamReader(flux);
            writer = new StreamWriter(flux);
            connected = true;

            while (run)
            {
                Receive(reader.ReadLine());
            }
            client.Close();
            connected = false;
        }

        public void ServerRecieve()
        {
            while (run && !server.Pending())
            {
                Thread.Sleep(10);
            }
            if (run)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream flux = client.GetStream();
                reader = new StreamReader(flux);
                writer = new StreamWriter(flux);
                connected = true;
            }

            while (run)
            {
                Receive(reader.ReadLine());
            }
            server.Stop();
            connected = false;
        }

        public void Send(string message, object obj)
        {
            writer.WriteLine(new JavaScriptSerializer().Serialize(new SendableObject(message, obj)));
            writer.Flush();
        }

        public void Receive(string json)
        {
            SendableObject sendableObject = new JavaScriptSerializer().Deserialize<SendableObject>(json);
            switch (sendableObject.message)
            {
                case "player":
                    view.setupP2.controller.playerName = sendableObject.data.ToString();
                    break;
                case "grid":
                    view.setupP2.controller.grid = (GridModel)sendableObject.data;
                    break;
                case "go":
                    view.StartGame();
                    break;
            }
        }
    }
}
