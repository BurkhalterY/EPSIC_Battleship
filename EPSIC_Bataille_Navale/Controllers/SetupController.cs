using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SetupController
    { // Déclaration des propriétées
        private Setup view = null;
        public Grid grid;
        public string playerName = "";
        private int[] clickedCell = new int[2];
        private List<int[]> possibleCells = new List<int[]>();

        public SetupController(Setup view, int size)
        {
            // Création d'une nouvelle grille
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
                    // Possiblité de placement à droite
                    bool possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++) // Parcours la liste des bateaux à placer
                    {
                        if (x + j >= grid.grid.GetLength(0)) // Empêche le bateau de sortir de la grille
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x + j, y].boat != null) // Vérifie si un bateau n'est pas déjà là
                        {
                            possible = false; // Si un bateau est déjà là on n'a pas la possibilité de placer ici
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x + grid.boatsList[i] - 1, y }); // La cellule devient verte 
                    }

                    // --- LEFT ---
                    // Possibilité de placement à gauche
                    possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++) // Parcours la liste des bateaux à placer
                    {
                        if (x - j < 0)
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x - j, y].boat != null) // Vérifie si un bateau n'est pas déjà là
                        {
                            possible = false;
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x - grid.boatsList[i] + 1, y }); // La cellule devient verte 
                    }

                    // --- DOWN ---
                    // Possiblité de placement en haut
                    possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++) // Parcours la liste des bateaux à placer
                    {
                        if (y + j >= grid.grid.GetLength(1)) // Empêche le bateau de sortir de la grille
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x, y + j].boat != null) // Vérifie si un bateau n'est pas déjà là
                        {
                            possible = false;
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x, y + grid.boatsList[i] - 1 }); // La cellule devient verte
                    }

                    // --- UP ---
                    // Possiblité de placement en bas
                    possible = true;
                    for (int j = 0; j < grid.boatsList[i]; j++) // Parcours la liste des bateaux à placer
                    {
                        if (y - j < 0)
                        {
                            possible = false;
                            break;
                        }
                        if (grid.grid[x, y - j].boat != null) // Vérifie si un bateau n'est pas déjà là
                        {
                            possible = false;
                        }
                    }
                    if (possible)
                    {
                        possibleCells.Add(new int[] { x, y - grid.boatsList[i] + 1 }); // La cellule devient verte
                    }
                }
            }
            else
            {
                bool validClick = false; // Initilisation de la variable "validClick"
                foreach (int[] cell in possibleCells) // Parcous toutes les cellules vertes
                {
                    if (x == cell[0] && y == cell[1]) // Si la cellule cliquée est dans les cellules vertes
                    {
                        validClick = true;
                        break;
                    }
                }
                if (validClick) // Si validClick est égal à "true" (donc si la cellule cliquée est dans les cellules vertes)
                {
                    // Aide la boucle for en dessous
                    int minX = Math.Min(x, clickedCell[0]);
                    int maxX = Math.Max(x, clickedCell[0]);
                    int minY = Math.Min(y, clickedCell[1]);
                    int maxY = Math.Max(y, clickedCell[1]);
                    Boat boat = new Boat(); // Crée un bateau vide
                    // Met un bateau dans chaque cellules verte selectionné
                    for (int i = minX; i <= maxX; i++)
                    {
                        for (int j = minY; j <= maxY; j++)
                        {
                            boat.length++;
                            grid.grid[i, j].boat = boat;
                        }
                    }
                    grid.boats.Add(boat); // Ajoute la bateau dans la grille
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

        // Méthode pour supprimer les bateau qu'on vient de placer 
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

        // Méthodes qui place les bateaux de l'IA
        public void AIChoise()
        {
            playerName = "L'IA"; 

            while (grid.boatsList.Count > 0)
            {
                int x = new Random().Next(0, grid.grid.GetLength(0) - 1); // Choisi une case random dans x
                int y = new Random().Next(0, grid.grid.GetLength(1) - 1); // Choisi une case random dans y
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
