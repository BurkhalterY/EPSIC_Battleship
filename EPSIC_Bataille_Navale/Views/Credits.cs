using System;
using System.Windows.Forms;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class Credits : UserControl
    {
        public Credits()
        {
            InitializeComponent();
        }

        // Revient en arrière (home)
        private void Btn_back_Click(object sender, EventArgs e)
        {
            ((MainForm)Parent.FindForm()).LoadView(new Home());
        }

        private void lbl_credits_Click(object sender, EventArgs e)
        {

        }
    }
}
