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
        private List<int[]> possibles = new List<int[]>();
        private int[] step = new int[4];
        private byte directions = 0b1111; // urdl

        public SoloGameController(Game view) : base(view) {}

        public void AIPlay()
        {
            if (possibles.Count == 0)
            {
                for (int i = 0; i < grids[0].grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grids[0].grid.GetLength(1); j++)
                    {
                        if (!IsAldryClicked(i, j))
                        {
                            possibles.Add(new int[] { i, j });
                        }
                    }
                }
            }
            Random random = new Random();

            int cellSelected = random.Next(0, possibles.Count - 1);
            State state = base.Click(possibles[cellSelected][0], possibles[cellSelected][1]);
            shots.Add(new int[] { possibles[cellSelected][0], possibles[cellSelected][1], State2Int(state) });
            possibles.RemoveAt(cellSelected);

            if (state == State.fullBoat)
            {
                phase = 0;
            }
            if (phase == 0)
            {
                if (state == State.boat)
                {
                    phase = 1;
                    directions = 0b1111;
                    step[0] = step[2] = possibles[cellSelected][1];
                    step[1] = step[3] = possibles[cellSelected][0];

                    possibles.Clear();
                    if (GetLastShot()[1] - 1 >= 0 && !IsAldryClicked(GetLastShot()[0], GetLastShot()[1] - 1))
                    {
                        possibles.Add(new int[] { GetLastShot()[0], GetLastShot()[1] - 1 });
                    }
                    if (GetLastShot()[0] + 1 < grids[0].grid.GetLength(0) && !IsAldryClicked(GetLastShot()[0] + 1, GetLastShot()[1]))
                    {
                        possibles.Add(new int[] { GetLastShot()[0] + 1, GetLastShot()[1] });
                    }
                    if (GetLastShot()[1] + 1 < grids[0].grid.GetLength(1) && !IsAldryClicked(GetLastShot()[0], GetLastShot()[1] + 1))
                    {
                        possibles.Add(new int[] { GetLastShot()[0], GetLastShot()[1] + 1 });
                    }
                    if (GetLastShot()[0] - 1 >= 0 && !IsAldryClicked(GetLastShot()[0] - 1, GetLastShot()[1]))
                    {
                        possibles.Add(new int[] { GetLastShot()[0] - 1, GetLastShot()[1] });
                    }
                }
            }
            else if (phase == 1)
            {
                if (state == State.noBoat)
                {
                    if (step[0] > GetLastShot()[1])
                    {
                        directions &= 0b0111;
                    }
                    else if (step[1] < GetLastShot()[0])
                    {
                        directions &= 0b1011;
                    }
                    else if (step[2] < GetLastShot()[1])
                    {
                        directions &= 0b1101;
                    }
                    else if (step[3] > GetLastShot()[0])
                    {
                        directions &= 0b1110;
                    }
                }
                else if (state == State.boat)
                {
                    if (step[0] == GetLastShot()[1])
                    {
                        Console.WriteLine("h");
                        step[1] = Math.Min(step[1], GetLastShot()[0]);
                        step[3] = Math.Max(step[3], GetLastShot()[0]);
                        directions &= 0b0101;
                    }
                    else if (step[1] == GetLastShot()[0])
                    {
                        Console.WriteLine("v");
                        step[0] = Math.Min(step[0], GetLastShot()[1]);
                        step[2] = Math.Max(step[2], GetLastShot()[1]);
                        directions &= 0b1010;
                    }
                }

                if ((directions & 0b1000) == 0b1000 && step[0] - 1 >= 0 && !IsAldryClicked(step[3], step[0] - 1))
                {
                    possibles.Add(new int[] { step[3], step[0] - 1 });
                }
                if ((directions & 0b0100) == 0b0100 && step[1] + 1 < grids[0].grid.GetLength(0) && !IsAldryClicked(step[1] + 1, step[2]))
                {
                    possibles.Add(new int[] { step[1] + 1, step[2] });
                }
                if ((directions & 0b0010) == 0b0010 && step[2] + 1 < grids[0].grid.GetLength(1) && !IsAldryClicked(step[1], step[2] + 1))
                {
                    possibles.Add(new int[] { step[1], step[2] + 1 });
                }
                if ((directions & 0b0001) == 0b0001 && step[3] - 1 >= 0 && !IsAldryClicked(step[3] - 1, step[0]))
                {
                    possibles.Add(new int[] { step[3] - 1, step[0] });
                }
            }
            Console.WriteLine(Convert.ToString(directions, 2));
        }

        private bool IsAldryClicked(int x, int y)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                if (shots[i][0] == x && shots[i][1] == y)
                {
                    return true;
                }
            }
            return false;
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

        private int[] GetLastShot()
        {
            if (shots.Count == 0)
            {
                return new int[] { -1, -1, -1 };
            }
            else
            {
                return shots[shots.Count - 1];
            }
        }

        public override State Click(int x, int y)
        {
            if(playerTurn == 0)
            {
                base.Click(x, y);
                //System.Threading.Thread.Sleep(1500);
                AIPlay();
            }
            return State.invalid;
        }
    }
}
