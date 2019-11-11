using EPSIC_Bataille_Navale.Controllers;
using System;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class Home : UserControl
    {
        private HomeController controller;
        public Home()
        {
            InitializeComponent();
            txt_pseudo.Text = UserPrincipal.Current.DisplayName;
            controller = new HomeController(this);
        }

        public void SetTitle(string title)
        {
            lbl_title.Text = title;
        }

        private void Btn_solo_Click(object sender, EventArgs e)
        {
            Setup setup = new Setup(10, 0);
            setup.controller.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            ((MainForm)Parent.FindForm()).LoadView(setup);
        }

        private void Btn_online_Click(object sender, EventArgs e)
        {
            OnlineConfig onlineConfig = new OnlineConfig();
            onlineConfig.playerName = txt_pseudo.Text == "" ? "Player" : txt_pseudo.Text;
            ((MainForm)Parent.FindForm()).LoadView(onlineConfig);
        }

        private void Btn_credits_Click(object sender, EventArgs e)
        {
            ((MainForm)Parent.FindForm()).LoadView(new Credits());
        }
    }
}
