using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System.Threading;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class DemoGameController : GameController
    {
        AI ai, ai2;

        public DemoGameController(Game view) : base(view) {
            ai = new AI(this);
            ai2 = new AI(this);
        }

        public override void Click(int x, int y)
        {
            while (!finish)
            {
                if (playerTurn == 0)
                {
                    view.Refresh();
                    Thread.Sleep(50);
                    ai2.AIPlay();
                }
                if (playerTurn == 1)
                {
                    view.Refresh();
                    Thread.Sleep(50);
                    ai.AIPlay();
                }
            }
            if (finish)
            {
                view.Finish(playersNames[playerTurn]);
            }
        }
    }
}
