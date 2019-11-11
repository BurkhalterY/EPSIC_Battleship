
using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class ServerGameController : GameController
    {
        public ServerGameController(Game view) : base(view) { }

        public override State Click(int x, int y)
        {
            if (playerTurn == 0)
            {
                base.Click(x, y);
            }
            if (playerTurn == 1)
            {
                
            }
            return State.invalid;
        }
    }
}
