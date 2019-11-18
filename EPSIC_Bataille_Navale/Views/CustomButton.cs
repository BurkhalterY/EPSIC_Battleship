using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    public class CustomButton : Button
    {
        public int x, y;
        public CustomButton(int x, int y) : base()
        {
            this.x = x;
            this.y = y;
        }
    }
}
