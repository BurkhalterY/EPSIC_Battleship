using EPSIC_Bataille_Navale.Views;

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
                    ai2.AIPlay();
                }
                if (playerTurn == 1)
                {
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
