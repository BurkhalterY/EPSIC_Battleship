using EPSIC_Bataille_Navale.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class Home : UserControl
    {
        private HomeController controller;
        public Home()
        {
            InitializeComponent();
            controller = new HomeController(this);
        }

        private void Btn_solo_Click(object sender, EventArgs e)
        {
            ((MainForm)Parent.FindForm()).LoadView(new Setup(10));
        }
    }
}
