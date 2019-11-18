using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SoloGameController : GameController
    {
        AI ai;

        public SoloGameController(Game view) : base(view) {
            ai = new AI(this);
        }

        public override void Click(int x, int y)
        {
            if (playerTurn == 0)
            {
                ClickAt(x, y);
            }
            if (playerTurn == 1)
            {
                ai.AIPlay();
            }
            if (finish)
            {
                view.Finish(playersNames[playerTurn]);
            }
        }
    }
}
