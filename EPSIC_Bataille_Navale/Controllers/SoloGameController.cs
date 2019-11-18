using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System.Threading;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SoloGameController : GameController
    {
        //Déclaration de la variable
        AI ai;

        public SoloGameController(Game view) : base(view) {
            ai = new AI(this); // Crée un nouvel IA
        }

        // Méthode qui gère le tout par tour 
        public override void Click(int x, int y)
        {
            if (playerTurn == 0)
            {
                ClickAt(x, y);
            }
            if (playerTurn == 1)
            {
                view.Refresh();
                Thread.Sleep(500);
                ai.AIPlay();
            }
            if (finish)
            {
                view.Finish(playersNames[playerTurn]);
            }
        }
    }
}
