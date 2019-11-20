using EPSIC_Bataille_Navale.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPSIC_Bataille_Navale.Models
{
    public class GridModel
    {
        public Cell[,] grid;
        public List<Boat> boats = new List<Boat>();
        public List<int> boatsList;

        public GridModel(int gridSize)
        {
            boatsList = Settings.Default.boatsList.Split(',').Select(Int32.Parse).ToList();
            grid = new Cell[gridSize, gridSize];
            for(int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i, j] = new Cell();
                }
            }
        }
    }
}
