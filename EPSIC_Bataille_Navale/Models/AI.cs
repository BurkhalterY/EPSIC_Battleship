using EPSIC_Bataille_Navale.Controllers;
using System;
using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Models
{
    class AI
    {
        private GameController controller;
        private int phase = 0;
        private List<int[]> shots = new List<int[]>();
        private List<int[]> possibles = new List<int[]>();
        private int[] step = new int[6];
        private byte directions = 0b1111; //urdl
        private int gridMask = random.Next(0, 1);
        private static Random random = new Random();

        public AI(GameController controller) {
            this.controller = controller;
        }

        /// <summary>
        /// Appeler cette fonction pour faire jouer l'IA
        /// </summary>
        public void AIPlay()
        {
            while (possibles.Count == 0)
            {
                //Si la liste des cases possibles est vide, je rajoute toutes celles dans lesquels nous n'avons pas encore tiré
                for (int i = 0; i < controller.size; i++)
                {
                    for (int j = 0; j < controller.size; j++)
                    {
                        if (!IsAldryClicked(i, j) && (i + gridMask) % 2 == j % 2)
                        {
                            possibles.Add(new int[] { i, j });
                        }
                    }
                }
                phase = 0;
                if(possibles.Count == 0) //Si toujours vide, je change le masque de la grille
                {
                    gridMask = (gridMask + 1) % 2;
                }
            }

            //Tir sur une case aléatoire parmis les possibles
            int cellSelected = random.Next(possibles.Count);
            int x = possibles[cellSelected][0];
            int y = possibles[cellSelected][1];
            State state = controller.ClickAt(x, y);
            shots.Add(new int[] { x, y }); //On l'ajoute à la liste des shots
            possibles.RemoveAt(cellSelected); //Et on supprime la case de la liste de possibilité

            if (phase == 0) //phase 0 = tirs aléatoires
            {
                if (state == State.boat)
                {
                    phase = 1;
                    directions = 0b1111;
                    step[1] = step[3] = step[4] = x;
                    step[0] = step[2] = step[5] = y;

                    //On vide les possibles puis on ajoute chaqu'une des cases adjacentes
                    possibles.Clear();
                    if (y - 1 >= 0 && !IsAldryClicked(x, y - 1))
                    {
                        possibles.Add(new int[] { x, y - 1 });
                    }
                    if (x + 1 < controller.size && !IsAldryClicked(x + 1, y))
                    {
                        possibles.Add(new int[] { x + 1, y });
                    }
                    if (y + 1 < controller.size && !IsAldryClicked(x, y + 1))
                    {
                        possibles.Add(new int[] { x, y + 1 });
                    }
                    if (x - 1 >= 0 && !IsAldryClicked(x - 1, y))
                    {
                        possibles.Add(new int[] { x - 1, y });
                    }
                }
                //Si state != State.boat, rester en phase 0
            }
            else if (phase == 1) //phase 1 = tirs cases adjacentes
            {
                if (state == State.noBoat)
                {
                    //Si il n'y a pas de bateau sur la case adjacente, on peut supprimer la direction des directions possibles
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
                    //Si 2 bateaux sur la même ligne, on peut supprimer les directions qui ne partagent pas la meme direction
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
                    //On check s'il est possible d'utiliser les direction restantes, si oui : on ajoute la case correspondante
                    if ((directions & 0b1000) == 0b1000 && step[0] - 1 >= 0 && !IsAldryClicked(step[4], step[0] - 1))
                    {
                        possibles.Add(new int[] { step[4], step[0] - 1 });
                    }
                    if ((directions & 0b0100) == 0b0100 && step[3] + 1 < controller.size && !IsAldryClicked(step[3] + 1, step[5]))
                    {
                        possibles.Add(new int[] { step[3] + 1, step[5] });
                    }
                    if ((directions & 0b0010) == 0b0010 && step[2] + 1 < controller.size && !IsAldryClicked(step[4], step[2] + 1))
                    {
                        possibles.Add(new int[] { step[4], step[2] + 1 });
                    }
                    if ((directions & 0b0001) == 0b0001 && step[1] - 1 >= 0 && !IsAldryClicked(step[1] - 1, step[5]))
                    {
                        possibles.Add(new int[] { step[1] - 1, step[5] });
                    }
                }

                //Si possibles est toujours vide, alors c'est que nous avont touché plusieurs bateaux
                if (possibles.Count == 0)
                {
                    //On repasse alors en revu tous les shots,
                    for (int i = 0; i < shots.Count; i++)
                    {
                        //...et si l'un contient encore un bateau non coulé
                        if (controller.grids[controller.playerTurn].grid[shots[i][0], shots[i][1]].state == State.boat)
                        {
                            directions = 0b1111;
                            step[1] = step[3] = step[4] = shots[i][0];
                            step[0] = step[2] = step[5] = shots[i][1];

                            if (shots[i][1] - 1 >= 0 && !IsAldryClicked(shots[i][0], shots[i][1] - 1))
                            {
                                possibles.Add(new int[] { shots[i][0], shots[i][1] - 1 });
                            }
                            if (shots[i][0] + 1 < controller.size && !IsAldryClicked(shots[i][0] + 1, shots[i][1]))
                            {
                                possibles.Add(new int[] { shots[i][0] + 1, shots[i][1] });
                            }
                            if (shots[i][1] + 1 < controller.size && !IsAldryClicked(shots[i][0], shots[i][1] + 1))
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
        /// Vérifie si la case (x;y) à déjà été visée
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Case déjà visée</returns>
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
