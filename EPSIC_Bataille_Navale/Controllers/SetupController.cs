using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SetupController
    {
        private Setup view = null;
        public GridModel grid;
        public string playerName = "";
        public int[] clickedCell = new int[0];
        public List<int[]> possibleCells = new List<int[]>();
        public int size;
        private static Random random = new Random();

        public SetupController(Setup view, int size)
        {
            this.view = view;
            this.size = size;
            grid = new GridModel(size);
        }

        public void Click(int x, int y)
        {
            if (possibleCells.Count == 0)
            {
                clickedCell = new int[] { x, y };
                for (int i = 0; i < grid.boatsList.Count; i++)
                {
                    for (int h = -1; h <= 1; h++)
                    {
                        for (int v = -1; v <= 1; v++)
                        {
                            if(Math.Abs(h) != Math.Abs(v))
                            {
                                bool possible = true;
                                for (int j = 0; j < grid.boatsList[i]; j++)
                                {
                                    if (x + j * h < 0 || x + j * h >= size || y + j * v < 0 || y + j * v >= size)
                                    {
                                        possible = false;
                                        break;
                                    }
                                    if (grid.grid[x + j * h, y + j * v].boat != null)
                                    {
                                        possible = false;
                                    }
                                }
                                if (possible)
                                {
                                    possibleCells.Add(new int[] { x + (grid.boatsList[i] - 1) * h, y + (grid.boatsList[i] - 1) * v });
                                }
                            }
                        }
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
                    boat.orientation = (Direction)(Math.Atan2(y - clickedCell[1], x - clickedCell[0]) * 180.0 / Math.PI);

                    for (int i = minX; i <= maxX; i++)
                    {
                        for (int j = minY; j <= maxY; j++)
                        {
                            grid.grid[i, j].boat = boat;
                            boat.cells.Add(grid.grid[i, j]);
                        }
                    }
                    grid.boats.Add(boat);
                    grid.boatsList.Remove(boat.cells.Count);
                    view.EnableCancelButton(true);
                    view.EnableNextButton(grid.boatsList.Count == 0);
                }
                clickedCell = new int[0];
                possibleCells.Clear();
            }
            view.RefreshGrid();
        }

        public void DeleteLastBoat()
        {
            if(grid.boats.Count > 0)
            {
                Boat boat = grid.boats[grid.boats.Count - 1];
                grid.boats.RemoveAt(grid.boats.Count - 1);
                grid.boatsList.Add(boat.cells.Count);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size ; j++)
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
                view.RefreshGrid();
            }
        }

        public void AIChoise()
        {
            playerName = "L'IA"; 

            while (grid.boatsList.Count > 0)
            {
                if (possibleCells.Count > 0)
                {
                    int[] cell = possibleCells[random.Next(possibleCells.Count)];
                    Click(cell[0], cell[1]);
                }
                else
                {
                    int x = random.Next(size);
                    int y = random.Next(size);
                    Click(x, y);
                }
            }
        }
    }
}
