using EPSIC_Bataille_Navale.Views;

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
