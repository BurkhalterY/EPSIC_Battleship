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
    public partial class MainForm : Form
    {
        private MainFormController controller;
        public MainForm()
        {
            InitializeComponent();
            controller = new MainFormController(this);
            LoadView(new Home());
        }

        public void LoadView(Control view)
        {
            view.Dock = DockStyle.Fill;
            panel.Controls.Clear();
            panel.Controls.Add(view);
        }
    }
}
