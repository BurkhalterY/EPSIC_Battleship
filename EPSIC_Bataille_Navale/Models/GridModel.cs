using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Models
{
    public class GridModel
    {
        public Cell[,] grid;
        public List<Boat> boats = new List<Boat>();

        public GridModel(int gridSize)
        {
            grid = new Cell[gridSize, gridSize];
            for(int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i, j] = new Cell();
                    grid[i, j].x = i;
                    grid[i, j].y = j;
                }
            }
        }
    }
}
