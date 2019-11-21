using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<Boat> boats = new List<Boat>();
        public List<int> boatsList;
        public int nbMines;
        private static Random random = new Random();

        public SetupController(Setup view, int size)
        {
            this.view = view;
            this.size = size;
            grid = new GridModel(size);
            boatsList = Properties.Settings.Default.boatsList.Split(',').Select(Int32.Parse).ToList();
            nbMines = Properties.Settings.Default.nbMines;
        }

        public void Click(int x, int y)
        {
            if (possibleCells.Count == 0)
            {
                clickedCell = new int[] { x, y };
                for (int i = 0; i < boatsList.Count; i++)
                {
                    for (int h = -1; h <= 1; h++)
                    {
                        for (int v = -1; v <= 1; v++)
                        {
                            if(Math.Abs(h) != Math.Abs(v))
                            {
                                bool possible = true;
                                for (int j = 0; j < boatsList[i]; j++)
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
                                    possibleCells.Add(new int[] { x + (boatsList[i] - 1) * h, y + (boatsList[i] - 1) * v });
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
                    boats.Add(boat);
                    grid.boats.Add(boat);
                    boatsList.Remove(boat.cells.Count);
                    view.EnableCancelButton(true);
                    view.EnableNextButton(boatsList.Count == 0 && nbMines == 0);
                }
                clickedCell = new int[0];
                possibleCells.Clear();
            }
            view.RefreshGrid();
        }

        public void RightClick(int x, int y)
        {
            if(nbMines > 0 && grid.grid[x, y].boat == null)
            {
                Boat boat = new Boat() { touchedCell = -1 };
                grid.grid[x, y].boat = boat;
                boat.cells.Add(grid.grid[x, y]);
                boats.Add(boat);
                nbMines--;
                view.EnableNextButton(boatsList.Count == 0 && nbMines == 0);
                view.RefreshGrid();
            }
        }

        public void DeleteLastBoat()
        {
            if (boats.Count > 0)
            {
                Boat boat = boats[boats.Count - 1];
                boats.RemoveAt(boats.Count - 1);
                if (boat.cells.Count == 1)
                {
                    nbMines++;
                }
                else
                {
                    boatsList.Add(boat.cells.Count);
                    grid.boats.Remove(boat);
                }
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (grid.grid[i, j].boat == boat)
                        {
                            grid.grid[i, j].boat = null;
                        }
                    }
                }
                if (boats.Count == 0)
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

            while (boatsList.Count > 0)
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
            while (nbMines > 0)
            {
                int x = random.Next(size);
                int y = random.Next(size);
                RightClick(x, y);
            }
        }
    }
}
