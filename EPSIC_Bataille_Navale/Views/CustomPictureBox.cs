using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPSIC_Bataille_Navale.Views
{
    public class CustomPictureBox : PictureBox
    {
        public int x, y;
        public CustomPictureBox(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
