using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;

namespace EPSIC_Bataille_Navale.Controllers
{
    // Permet de générer un combat entre 2 IA
    public class DemoGameController : GameController
    {
        // Création de 2 IA
        AI ai, ai2;

        // Initialisation
        public DemoGameController(Game view) : base(view) {
            ai = new AI(this);
            ai2 = new AI(this);
        }

        // Création du combat
        public override void Click(int x, int y)
        {
            while (!finish)
            {
                if (playerTurn == 0)
                {
                    ai2.AIPlay();
                }
                if (playerTurn == 1)
                {
                    ai.AIPlay();
                }
            }
            // Si le combat est fini
            if (finish)
            {
                view.Finish(playersNames[playerTurn]);
            }
        }
    }
}
