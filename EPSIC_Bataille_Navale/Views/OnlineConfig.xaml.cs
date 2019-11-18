using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour OnlineConfig.xaml
    /// </summary>
    public partial class OnlineConfig : Page
    {
        public string playerName = "";

        public OnlineConfig()
        {
            InitializeComponent();
        }

        private void Btn_back_Click(object sender, EventArgs e)
        {
            (Parent as MainWindow).Content = new Home();
        }
        private void Btn_join_Click(object sender, EventArgs e)
        {
            /*TcpClient client = new TcpClient();
            client.Connect(txt_serv_address.Text, 8888);
            NetworkStream flux = client.GetStream();
            StreamReader reader = new StreamReader(flux);
            StreamWriter writer = new StreamWriter(flux);
            writer.WriteLine("dddddddddeiowfnreèà23");
            writer.Flush();*/
        }
        private void Btn_host_Click(object sender, EventArgs e)
        {
            /*TcpListener server = new TcpListener(IPAddress.Any, 8888);
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
            server.Stop();*/
        }
    }
}
