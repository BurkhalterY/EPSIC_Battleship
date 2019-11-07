﻿using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SoloGameController : GameController
    {
        private int phase = 0;
        private List<int[]> shots = new List<int[]>();
        private List<int[]> possibles = new List<int[]>();
        private int[] step = new int[6];
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
                phase = 0;
            }
            Random random = new Random();
            int cellSelected = random.Next(0, possibles.Count - 1);
            int x = possibles[cellSelected][0];
            int y = possibles[cellSelected][1];
            State state = base.Click(x, y);
            shots.Add(new int[] { x, y, State2Int(state) });
            possibles.RemoveAt(cellSelected);

            if (phase == 0)
            {
                if (state == State.boat)
                {
                    phase = 1;
                    directions = 0b1111;
                    step[1] = step[3] = step[4] = x;
                    step[0] = step[2] = step[5] = y;

                    possibles.Clear();
                    if (y - 1 >= 0 && !IsAldryClicked(x, y - 1))
                    {
                        possibles.Add(new int[] { x, y - 1 });
                    }
                    if (x + 1 < grids[0].grid.GetLength(0) && !IsAldryClicked(x + 1, y))
                    {
                        possibles.Add(new int[] { x + 1, y });
                    }
                    if (y + 1 < grids[0].grid.GetLength(1) && !IsAldryClicked(x, y + 1))
                    {
                        possibles.Add(new int[] { x, y + 1 });
                    }
                    if (x - 1 >= 0 && !IsAldryClicked(x - 1, y))
                    {
                        possibles.Add(new int[] { x - 1, y });
                    }
                }
            }
            else if (phase == 1)
            {
                if (state == State.noBoat)
                {
                    if (step[0] > y)
                    {
                        directions &= 0b0111;
                    }
                    else if (step[1] < x)
                    {
                        directions &= 0b1011;
                    }
                    else if (step[2] < y)
                    {
                        directions &= 0b1101;
                    }
                    else if (step[3] > x)
                    {
                        directions &= 0b1110;
                    }
                }
                else if (state == State.boat)
                {
                    if (step[5] == y)
                    {
                        step[1] = Math.Min(step[1], x);
                        step[3] = Math.Max(step[3], x);
                        directions &= 0b0101;
                    }
                    else if (step[4] == x)
                    {
                        step[0] = Math.Min(step[0], y);
                        step[2] = Math.Max(step[2], y);
                        directions &= 0b1010;
                    }
                }

                possibles.Clear();
                if (state != State.fullBoat)
                {
                    if ((directions & 0b1000) == 0b1000 && step[0] - 1 >= 0 && !IsAldryClicked(step[4], step[0] - 1))
                    {
                        possibles.Add(new int[] { step[4], step[0] - 1 });
                    }
                    if ((directions & 0b0100) == 0b0100 && step[3] + 1 < grids[0].grid.GetLength(0) && !IsAldryClicked(step[3] + 1, step[5]))
                    {
                        possibles.Add(new int[] { step[3] + 1, step[5] });
                    }
                    if ((directions & 0b0010) == 0b0010 && step[2] + 1 < grids[0].grid.GetLength(1) && !IsAldryClicked(step[4], step[2] + 1))
                    {
                        possibles.Add(new int[] { step[4], step[2] + 1 });
                    }
                    if ((directions & 0b0001) == 0b0001 && step[1] - 1 >= 0 && !IsAldryClicked(step[1] - 1, step[5]))
                    {
                        possibles.Add(new int[] { step[1] - 1, step[5] });
                    }
                }

                if (possibles.Count == 0)
                {
                    for (int i = 0; i < shots.Count; i++)
                    {
                        if (grids[playerTurn].grid[shots[i][0], shots[i][1]].state == State.boat)
                        {
                            directions = 0b1111;
                            step[1] = step[3] = step[4] = shots[i][0];
                            step[0] = step[2] = step[5] = shots[i][1];

                            if (shots[i][1] - 1 >= 0 && !IsAldryClicked(shots[i][0], shots[i][1] - 1))
                            {
                                possibles.Add(new int[] { shots[i][0], shots[i][1] - 1 });
                            }
                            if (shots[i][0] + 1 < grids[0].grid.GetLength(0) && !IsAldryClicked(shots[i][0] + 1, shots[i][1]))
                            {
                                possibles.Add(new int[] { shots[i][0] + 1, shots[i][1] });
                            }
                            if (shots[i][1] + 1 < grids[0].grid.GetLength(1) && !IsAldryClicked(shots[i][0], shots[i][1] + 1))
                            {
                                possibles.Add(new int[] { shots[i][0], shots[i][1] + 1 });
                            }
                            if (shots[i][0] - 1 >= 0 && !IsAldryClicked(shots[i][0] - 1, shots[i][1]))
                            {
                                possibles.Add(new int[] { shots[i][0] - 1, shots[i][1] });
                            }
                        }
                    }
                }
            }
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

        public override State Click(int x, int y)
        {
            if(playerTurn == 0)
            {
                base.Click(x, y);
                //System.Threading.Thread.Sleep(1500);
            }
            if(playerTurn == 1)
            {
                AIPlay();
            }
            return State.invalid;
        }
    }
}