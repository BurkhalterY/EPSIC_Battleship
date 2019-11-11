using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
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
            try {
                txt_pseudo.Text = UserPrincipal.Current.DisplayName;
            }
            catch (PrincipalServerDownException e)
            {
                txt_pseudo.Text = "Player";
            }
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

        private void Btn_demo_Click(object sender, EventArgs e)
        {
            Setup setup = new Setup(10, 1);
            setup.controller.AIChoise();

            Setup setup2 = new Setup(10, 1);
            setup2.controller.AIChoise();

            Game game = new Game(setup.controller.grid.grid.GetLength(0), 3);
            game.controller.grids = new Grid[] { setup.controller.grid, setup2.controller.grid };
            game.controller.playersNames = new string[] { "IA1", "IA2" };
            game.MakeSecondGrid();
            ((MainForm)Parent.FindForm()).LoadView(game);
            game.controller.Click(0, 0);
        }
    }
}
