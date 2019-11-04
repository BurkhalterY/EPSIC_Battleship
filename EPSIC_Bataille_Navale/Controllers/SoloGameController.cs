using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SoloGameController : GameController
    {
        private int phase = 0;
        private List<int[]> shots = new List<int[]>();

        public SoloGameController(Game view) : base(view) { }

        public void AIPlay()
        {
            Random random = new Random();
            if (phase == 0)
            {
                while (GetLastShot() == State.invalid)
                {
                    int x = random.Next(0, grids[0].grid.GetLength(0) - 1);
                    int y = random.Next(0, grids[0].grid.GetLength(1) - 1);
                    shots.Add(new int[] { x, y, State2Int(Click(x, y)) });
                }
                if (GetLastShot() == State.boat)
                {
                    phase = 1;
                }
            }
            else if (phase == 1)
            {

            }
        }

        private static int State2Int(State state)
        {
            switch (state)
            {
                case State.noActivity:
                    return 0;
                case State.noBoat:
                    return 1;
                case State.boat:
                    return 2;
                case State.fullBoat:
                    return 3;
                default:
                    return 4;
            }
        }

        private static State Int2State(int state)
        {
            switch (state)
            {
                case 0:
                    return State.noActivity;
                case 1:
                    return State.noBoat;
                case 2:
                    return State.boat;
                case 3:
                    return State.fullBoat;
                default:
                    return State.invalid;
            }
        }

        private State GetLastShot()
        {
            if(shots.Count == 0)
            {
                return State.invalid;
            }
            else
            {
                return Int2State(shots[shots.Count - 1][2]);
            }
        }
    }
}
