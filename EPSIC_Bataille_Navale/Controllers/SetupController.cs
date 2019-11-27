using EPSIC_Bataille_Navale.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SetupController
    {
        public delegate void Refresh(int x, int y);
        public delegate void EnableBtnCancel(bool active);
        public delegate void EnableBtnNext(bool active);

        public event Refresh OnRefresh;
        public event EnableBtnCancel OnEnableBtnCancel;
        public event EnableBtnNext OnEnableBtnNext;


        public GridModel grid;
        public string playerName;
        public Cell clickedCell;
        public List<Cell> possibleCells = new List<Cell>();
        public int size;
        public List<Boat> boats = new List<Boat>();
        public List<int> boatsList;
        public int nbMines;
        private static Random random = new Random();

        public SetupController(int size)
        {
            this.size = size;
            grid = new GridModel(size);
            boatsList = Properties.Settings.Default.boatsList.Split(',').Select(Int32.Parse).ToList();
            nbMines = Properties.Settings.Default.nbMines;
        }

        /// <summary>
        /// Méthode appelée par les views lors d'un clic sur un bouton
        /// Sert à poser les bateaux
        /// </summary>
        /// <param name="x">Coordonnée X du button</param>
        /// <param name="y">Coordonnée Y du button</param>
        public void Click(Cell cell)
        {
            if (possibleCells.Count == 0)
            {
                if (clickedCell != null)
                {
                    Cell provCell = clickedCell;
                    clickedCell = null;
                    OnRefresh?.Invoke(provCell.x, provCell.y);
                }
                clickedCell = cell;
                OnRefresh?.Invoke(clickedCell.x, clickedCell.y);
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
                                    if (cell.x + j * h < 0 || cell.x + j * h >= size || cell.y + j * v < 0 || cell.y + j * v >= size)
                                    {
                                        possible = false;
                                        break;
                                    }
                                    if (grid.grid[cell.x + j * h, cell.y + j * v].boat != null)
                                    {
                                        possible = false;
                                    }
                                }
                                if (possible)
                                {
                                    Cell possibleCell = grid.grid[cell.x + (boatsList[i] - 1) * h, cell.y + (boatsList[i] - 1) * v];
                                    possibleCells.Add(possibleCell);
                                    OnRefresh?.Invoke(possibleCell.x, possibleCell.y);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (possibleCells.Contains(cell))
                {
                    int minX = Math.Min(cell.x, clickedCell.x);
                    int maxX = Math.Max(cell.x, clickedCell.x);
                    int minY = Math.Min(cell.y, clickedCell.y);
                    int maxY = Math.Max(cell.y, clickedCell.y);
                    Boat boat = new Boat();
                    boat.startCell = new int[] { minX, minY };
                    boat.orientation = (Direction)(Math.Atan2(cell.y - clickedCell.y, cell.x - clickedCell.x) * 180.0 / Math.PI);

                    for (int i = minX; i <= maxX; i++)
                    {
                        for (int j = minY; j <= maxY; j++)
                        {
                            grid.grid[i, j].boat = boat;
                            boat.cells.Add(grid.grid[i, j]);
                        }
                    }
                    foreach (Cell boatCell in boat.cells)
                    {
                        OnRefresh?.Invoke(boatCell.x, boatCell.y);
                    }
                    boats.Add(boat);
                    grid.boats.Add(boat);
                    boatsList.Remove(boat.cells.Count);
                    OnEnableBtnCancel?.Invoke(true);
                    OnEnableBtnNext?.Invoke(boatsList.Count == 0 && nbMines == 0);
                }
                Cell provCell = clickedCell;
                clickedCell = null;
                OnRefresh?.Invoke(provCell.x, provCell.y);
                ClearPossibles();
            }
        }

        /// <summary>
        /// Méthode appelée par les views lors d'un clic droit sur un bouton
        /// Sert à poser une mine
        /// </summary>
        /// <param name="x">Coordonnée X du button</param>
        /// <param name="y">Coordonnée Y du button</param>
        public void RightClick(int x, int y)
        {
            if(nbMines > 0 && grid.grid[x, y].boat == null)
            {
                ClearPossibles();
                Boat boat = new Boat() { touchedCell = -1 };
                boat.startCell = new int[] { x, y };
                grid.grid[x, y].boat = boat;
                boat.cells.Add(grid.grid[x, y]);
                boats.Add(boat);
                nbMines--;
                OnEnableBtnNext?.Invoke(boatsList.Count == 0 && nbMines == 0);
                OnRefresh?.Invoke(x, y);
            }
        }

        /// <summary>
        /// Supprime le dernier bateau ou la dernière mine posé
        /// </summary>
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

                foreach (Cell cell in boat.cells)
                {
                    cell.boat = null;
                    OnRefresh(cell.x, cell.y);
                }

                if (boats.Count == 0)
                {
                    OnEnableBtnCancel?.Invoke(false);
                }
                OnEnableBtnNext?.Invoke(false);
            }
        }

        /// <summary>
        /// Choix aléatoire de la position des bateaux
        /// </summary>
        public void AIChoise()
        {
            ClearPossibles();
            while (boats.Count > 0)
            {
                DeleteLastBoat();
            }
            while (boatsList.Count > 0)
            {
                if (possibleCells.Count > 0)
                {
                    Click(possibleCells[random.Next(possibleCells.Count)]);
                }
                else
                {
                    int x = random.Next(size);
                    int y = random.Next(size);
                    Click(grid.grid[x, y]);
                }
            }
            while (nbMines > 0)
            {
                int x = random.Next(size);
                int y = random.Next(size);
                RightClick(x, y);
            }
        }

        private void ClearPossibles()
        {
            while (possibleCells.Count > 0)
            {
                Cell removedCell = possibleCells[0];
                possibleCells.RemoveAt(0);
                OnRefresh?.Invoke(removedCell.x, removedCell.y);
            }
        }
    }
}
