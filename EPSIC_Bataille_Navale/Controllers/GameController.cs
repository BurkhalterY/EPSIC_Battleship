using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSIC_Bataille_Navale.Controllers
{
    public abstract class GameController
    {
        protected Game view = null;
        public Grid[] grids;
        public int playerTurn = 0;

        public GameController(Game view)
        {
            this.view = view;
        }

        public State Click(int x, int y)
        {
            if (grids[playerTurn].grid[x, y].state == State.noActivity)
            {
                State state;
                if (grids[playerTurn].grid[x, y].boat != null)
                {
                    Boat boat = grids[playerTurn].grid[x, y].boat;
                    boat.length--;
                    if (boat.length == 0)
                    {
                        grids[playerTurn].grid[x, y].state = State.fullBoat;
                        state = State.fullBoat;
                    }
                    else
                    {
                        grids[playerTurn].grid[x, y].state = State.boat;
                        state = State.boat;
                    }
                    
                }
                else
                {
                    grids[playerTurn].grid[x, y].state = State.noBoat;
                    state = State.noBoat;
                }
                playerTurn = (playerTurn + 1) % 2;
                view.RefreshGrid();
                return state;
            }
            return State.invalid;
        }

        public void SetGrids(Grid[] grids)
        {
            this.grids = grids;
            view.MakeSecondGrid(grids[0].grid.GetLength(0));
        }
    }
}
