using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class HomeController
    {
        private Home view = null;

        public HomeController(Home view)
        {
            this.view = view;
        }
    }
}
