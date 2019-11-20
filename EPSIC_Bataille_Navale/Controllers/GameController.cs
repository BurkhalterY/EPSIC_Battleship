using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Windows.Media;

namespace EPSIC_Bataille_Navale.Controllers
{
    public abstract class GameController
    {
        protected Game view = null;
        public GridModel[] grids;
        public string[] playersNames = new string[2];
        public int playerTurn = 0;
        public bool finish = false;

        public GameController(Game view)
        {
            this.view = view;
        }

        public virtual void Click(int x, int y)
        {

        }

        public State ClickAt(int x, int y)
        {
            playerTurn = (playerTurn + 1) % 2;
            if (grids[playerTurn].grid[x, y].state == State.noActivity)
            {
                State state;
                if (grids[playerTurn].grid[x, y].boat != null)
                {
                    Boat boat = grids[playerTurn].grid[x, y].boat;
                    boat.touchedCell++;
                    if (boat.cells.Count == boat.touchedCell)
                    {
                        grids[playerTurn].grid[x, y].state = State.fullBoat;
                        for (int i = 0; i < grids[playerTurn].grid.GetLength(0); i++)
                        {
                            for (int j = 0; j < grids[playerTurn].grid.GetLength(1); j++)
                            {
                                if (grids[playerTurn].grid[i, j].boat == boat)
                                {
                                    grids[playerTurn].grid[i, j].state = State.fullBoat;
                                }
                            }
                        }
                        grids[playerTurn].boats.Remove(grids[playerTurn].grid[x, y].boat);
                        if (grids[playerTurn].boats.Count == 0)
                        {
                            playerTurn = (playerTurn + 1) % 2;
                            finish = true;
                        }
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
                view.RefreshGrid();
                view.Foreground = playerTurn == 0 ? Brushes.Red : Brushes.Blue;
                view.history.AppendText(((char)(x + 65)).ToString() + (y + 1) + Environment.NewLine);
                view.history.ScrollToEnd();
                return state;
            }
            else
            {
                playerTurn = (playerTurn + 1) % 2;
            }
            return State.invalid;
        }
    }
}
