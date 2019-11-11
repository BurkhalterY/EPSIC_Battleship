using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SetupController
    {
        private Setup view = null;
        public Grid grid;
        public string playerName = "";
        private int[] clickedCell = new int[2];
        private List<int[]> possibleCells = new List<int[]>();

        public SetupController(Setup view, int size)
        {
            this.view = view;
            grid = new Grid(size);
        }

        public void Click(int x, int y)
        {
            if (possibleCells.Count == 0)
            {
                clickedCell = new int[] { x, y };
                for (int i = 0; i < grid.boatsList.Count; i++)
                {
                    // --- RIGHT ---

                    bool possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++)
                    {
                        if (x + j >= grid.grid.GetLength(0))
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x + j, y].boat != null)
                        {
                            possible = false;
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x + grid.boatsList[i] - 1, y });
                    }

                    // --- LEFT ---

                    possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++)
                    {
                        if (x - j < 0)
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x - j, y].boat != null)
                        {
                            possible = false;
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x - grid.boatsList[i] + 1, y });
                    }

                    // --- DOWN ---

                    possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++)
                    {
                        if (y + j >= grid.grid.GetLength(1))
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x, y + j].boat != null)
                        {
                            possible = false;
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x, y + grid.boatsList[i] - 1 });
                    }

                    // --- UP ---

                    possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++)
                    {
                        if (y - j < 0)
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x, y - j].boat != null)
                        {
                            possible = false;
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x, y - grid.boatsList[i] + 1 });
                    }
                }
            }
            else
            {
                bool validClick = false;
                foreach (int[] cell in possibleCells)
                {
                    if (x == cell[0] && y == cell[1])
                    {
                        validClick = true;
                        break;
                    }
                }
                if (validClick)
                {
                    int minX = Math.Min(x, clickedCell[0]);
                    int maxX = Math.Max(x, clickedCell[0]);
                    int minY = Math.Min(y, clickedCell[1]);
                    int maxY = Math.Max(y, clickedCell[1]);
                    Boat boat = new Boat();
                    for (int i = minX; i <= maxX; i++)
                    {
                        for (int j = minY; j <= maxY; j++)
                        {
                            boat.length++;
                            grid.grid[i, j].boat = boat;
                        }
                    }
                    grid.boats.Add(boat);
                    grid.boatsList.Remove(boat.length);
                    view.EnableCancelButton(true);
                    if (grid.boatsList.Count == 0)
                    {
                        view.EnableNextButton(true);
                    }
                }
                clickedCell = new int[] { };
                possibleCells.Clear();
            }
            view.RefreshGrid(grid, clickedCell, possibleCells);
        }

        public void DeleteLastBoat()
        {
            if(grid.boats.Count > 0)
            {
                Boat boat = grid.boats[grid.boats.Count - 1];
                grid.boats.RemoveAt(grid.boats.Count - 1);
                grid.boatsList.Add(boat.length);
                for (int i = 0; i < grid.grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.grid.GetLength(1); j++)
                    {
                        if (grid.grid[i, j].boat == boat)
                        {
                            grid.grid[i, j].boat = null;
                        }
                    }
                }
                if (grid.boats.Count == 0)
                {
                    view.EnableCancelButton(false);
                }
                view.EnableNextButton(false);
                view.RefreshGrid(grid, clickedCell, possibleCells);
            }
        }

        public void AIChoise()
        {
            playerName = "L'IA";

            while (grid.boatsList.Count > 0)
            {
                int x = new Random().Next(0, grid.grid.GetLength(0) - 1);
                int y = new Random().Next(0, grid.grid.GetLength(1) - 1);
                Click(x, y);
                if(possibleCells.Count > 0)
                {
                    int[] cell = possibleCells[new Random().Next(0, possibleCells.Count - 1)];
                    Click(cell[0], cell[1]);
                }
                else
                {
                    Click(x, y);
                }
            }
        }
    }
}
