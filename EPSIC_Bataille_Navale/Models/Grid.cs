using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Models
{
    public class Grid
    {
        public Cell[,] grid;
        public List<Boat> boats = new List<Boat>();
        public List<int> boatsList = new List<int>(new int[] { 2, 3, 3, 4, 5 });

        public Grid(int gridSize)
        {
            grid = new Cell[gridSize, gridSize];

            for(int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Cell();
                }
            }
        }
    }
}
