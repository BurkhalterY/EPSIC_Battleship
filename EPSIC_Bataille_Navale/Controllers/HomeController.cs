using EPSIC_Bataille_Navale.Views;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class HomeController
    {
        // Propriétées de la classe home
        private Home view = null;

        // Initialisation
        public HomeController(Home view)
        {
            this.view = view;
        }
    }
}
