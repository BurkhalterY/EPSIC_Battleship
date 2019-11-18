using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Models
{
    // Création de la classe "Grid"
    public class GridModel
    {
        public Cell[,] grid;
        public List<Boat> boats = new List<Boat>();
        public List<int> boatsList = new List<int>(new int[] { 2, 3, 3, 4, 5 }); // Liste des différentes taille des bateaux

        // Création de la grille
        public GridModel(int gridSize)
        {
            grid = new Cell[gridSize, gridSize];
            // parcours tout les "x"
            for(int i = 0; i < grid.GetLength(0); i++)
            {   // parcours tout les "y"
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Cell();
                }
            }
        }
    }
}
