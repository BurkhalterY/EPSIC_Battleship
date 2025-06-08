using EPSIC_Bataille_Navale.Controllers;
using System;
using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Models
{
    public class AI
    {
        private GameController controller;
        private int phase;
        private List<int[]> shots = new List<int[]>();
        private List<int[]> possibles = new List<int[]>();
        private int[] step = new int[6];
        private byte directions = 0b1111; // urdl
        private int gridMask = random.Next(2); // Because boat of length 1 doesn't exist (by default), we can exclude exactly one case of two
        private static Random random = new Random();
        private int size;
        private int missSince;

        public AI(GameController controller)
        {
            this.controller = controller;
            size = controller.players[0].grid.grid.GetLength(0);
        }

        /// <summary>
        /// Call this method to make AI plays
        /// </summary>
        public void AIPlay()
        {
            if (missSince < 5 || controller.players[controller.playerTurn].sonars == 0 || controller.players[controller.playerTurn].nuclearBombs == 0)
            {
                while (possibles.Count == 0)
                {
                    // If the list of possible cases is empty, add all cases that where we didn't shot yet
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (!IsAldryClicked(i, j) && (i + gridMask) % 2 == j % 2)
                            {
                                possibles.Add(new int[] { i, j });
                            }
                        }
                    }
                    phase = 0;
                    if (possibles.Count == 0) // If is still empty, change the grid mask
                    {
                        gridMask = (gridMask + 1) % 2;
                    }
                }

                // Shot a random cases from candidates
                int cellSelected = random.Next(possibles.Count);
                int x = possibles[cellSelected][0];
                int y = possibles[cellSelected][1];
                State state = controller.ClickAt(x, y);
                shots.Add(new int[] { x, y }); // Add to shot list
                possibles.RemoveAt(cellSelected); // And remove it from candidates

                if (state == State.noBoat)
                {
                    missSince++;
                }

                DeterminePossibles(x, y, state);
            }
            else
            {
                if (possibles.Count == 1)
                {
                    List<int[]> cells = controller.NuclearAttack(possibles[0][0], possibles[0][1]);
                    possibles.Clear();
                    foreach (int[] cell in cells)
                    {
                        shots.Add(new int[] { cell[0], cell[1] });
                        DeterminePossibles(cell[0], cell[1], (State)cell[2]);
                    }
                    missSince = 0;
                }
                else
                {
                    possibles.Clear();
                    possibles.Add(controller.Sonar());
                }
            }
        }

        private void DeterminePossibles(int x, int y, State state)
        {
            if (phase == 0) // phase 0 = random shots
            {
                if (state == State.boat)
                {
                    phase = 1;
                    directions = 0b1111;
                    step[1] = step[3] = step[4] = x;
                    step[0] = step[2] = step[5] = y;

                    // Empty the candidate list then add all adjacent cases
                    possibles.Clear();
                    if (y - 1 >= 0 && !IsAldryClicked(x, y - 1))
                    {
                        possibles.Add(new int[] { x, y - 1 });
                    }
                    if (x + 1 < size && !IsAldryClicked(x + 1, y))
                    {
                        possibles.Add(new int[] { x + 1, y });
                    }
                    if (y + 1 < size && !IsAldryClicked(x, y + 1))
                    {
                        possibles.Add(new int[] { x, y + 1 });
                    }
                    if (x - 1 >= 0 && !IsAldryClicked(x - 1, y))
                    {
                        possibles.Add(new int[] { x - 1, y });
                    }
                }
                // If state != State.boat, keep in phase 0
            }
            else if (phase == 1) // phase 1 = shot on adjacent cases
            {
                if (state == State.noBoat)
                {
                    // If there is not boat on the adjacent case, do not continue in this direction anymore
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
                    // If there is 2 boat cases on the same line, remove its perpendicular directions
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
                    // If it is possible to use remaining directions, add their coresponding cases
                    if ((directions & 0b1000) == 0b1000 && step[0] - 1 >= 0 && !IsAldryClicked(step[4], step[0] - 1))
                    {
                        possibles.Add(new int[] { step[4], step[0] - 1 });
                    }
                    if ((directions & 0b0100) == 0b0100 && step[3] + 1 < size && !IsAldryClicked(step[3] + 1, step[5]))
                    {
                        possibles.Add(new int[] { step[3] + 1, step[5] });
                    }
                    if ((directions & 0b0010) == 0b0010 && step[2] + 1 < size && !IsAldryClicked(step[4], step[2] + 1))
                    {
                        possibles.Add(new int[] { step[4], step[2] + 1 });
                    }
                    if ((directions & 0b0001) == 0b0001 && step[1] - 1 >= 0 && !IsAldryClicked(step[1] - 1, step[5]))
                    {
                        possibles.Add(new int[] { step[1] - 1, step[5] });
                    }
                }

                // If candidates is still empty, thus many boats are touched
                if (possibles.Count == 0)
                {
                    // Browse all previous shots again...
                    for (int i = 0; i < shots.Count; i++)
                    {
                        // ...and check if there is still an untouched boat
                        if (controller.players[controller.playerTurn].grid.grid[shots[i][0], shots[i][1]].state == State.boat)
                        {
                            directions = 0b1111;
                            step[1] = step[3] = step[4] = shots[i][0];
                            step[0] = step[2] = step[5] = shots[i][1];

                            if (shots[i][1] - 1 >= 0 && !IsAldryClicked(shots[i][0], shots[i][1] - 1))
                            {
                                possibles.Add(new int[] { shots[i][0], shots[i][1] - 1 });
                            }
                            if (shots[i][0] + 1 < size && !IsAldryClicked(shots[i][0] + 1, shots[i][1]))
                            {
                                possibles.Add(new int[] { shots[i][0] + 1, shots[i][1] });
                            }
                            if (shots[i][1] + 1 < size && !IsAldryClicked(shots[i][0], shots[i][1] + 1))
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

        /// <summary>
        /// Check if the case (x;y) is tested yet
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Is already clicked</returns>
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
    }
}
