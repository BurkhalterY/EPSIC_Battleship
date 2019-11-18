using EPSIC_Bataille_Navale.Views;

namespace EPSIC_Bataille_Navale.Controllers
{
    // Propriétées de la classe ServerGame
    public class ServerGameController : GameController
    {
        // Initialisation
        public ServerGameController(Game view) : base(view) { }
        
        // Pas implémenté
        public override void Click(int x, int y)
        {
           
        }
    }
}
