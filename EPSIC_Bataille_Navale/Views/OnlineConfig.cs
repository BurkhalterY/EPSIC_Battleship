using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class OnlineConfig : UserControl
    {
        public string playerName = "";

        public OnlineConfig()
        {
            InitializeComponent();
        }

        private void Btn_back_Click(object sender, EventArgs e)
        {
            ((MainForm)Parent.FindForm()).LoadView(new Home());
        }
        private void Btn_join_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect(txt_serv_address.Text, 8888);
            NetworkStream flux = client.GetStream();
            StreamReader reader = new StreamReader(flux);
            StreamWriter writer = new StreamWriter(flux);
            writer.WriteLine("dddddddddeiowfnreèà23");
            writer.Flush();
        }
        private void Btn_host_Click(object sender, EventArgs e)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            TcpClient client = server.AcceptTcpClient();

            NetworkStream flux = client.GetStream();
            StreamReader reader = new StreamReader(flux);
            StreamWriter writer = new StreamWriter(flux);

            while (true)
            {
                Console.WriteLine(reader.ReadLine());
            }

            client.Close();
            server.Stop();
        }

        private void OnlineConfig_Load(object sender, EventArgs e)
        {

        }
    }
}
