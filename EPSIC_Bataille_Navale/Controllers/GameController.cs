using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Documents;
using System.Windows.Media;

namespace EPSIC_Bataille_Navale.Controllers
{
    public enum ActionType {
        NormalShot,
        Sonar,
        NuclearBomb
    }

    public abstract class GameController
    {
        public Game view;
        public Player[] players = new Player[2];
        public int playerTurn = 0;
        public bool finish = false;
        public int size;

        public GameController(Game view)
        {
            this.view = view;
            size = view.size;
        }

        /// <summary>
        /// Méthode appelée par les views lors d'un clic sur un bouton
        /// </summary>
        /// <param name="x">Coordonnée X du button</param>
        /// <param name="y">Coordonnée Y du button</param>
        public abstract void Click(int x = 0, int y = 0, ActionType action = ActionType.NormalShot);

        /// <summary>
        /// Tire standart sur une case
        /// </summary>
        /// <param name="x">Coordonnée X de la case</param>
        /// <param name="y">Coordonnée Y de la case</param>
        /// <returns>Le nouvel état de la case visée</returns>
        public State ClickAt(int x, int y)
        {
            playerTurn = (playerTurn + 1) % 2;
            State state = Shot(x, y);
            if(state != State.invalid)
            {
                if(state == State.noBoat)
                {
                    new SoundPlayer(Properties.Resources.splash).Play();
                }
                else if (state == State.boat || state == State.fullBoat)
                {
                    new SoundPlayer(Properties.Resources.explosion).Play();
                }
                view.RefreshGrid();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(players[playerTurn].playerName + "\t: " + ((char)(x + 65)).ToString() + (y + 1));
                paragraph.Foreground = playerTurn == 0 ? Brushes.Red : Brushes.Blue;
                paragraph.LineHeight = 1;
                view.history.Document.Blocks.Add(paragraph);
                view.history.ScrollToEnd();
            }
            else
            {
                playerTurn = (playerTurn + 1) % 2;
            }
            return state;
        }

        /// <summary>
        /// Tir sur une seule case
        /// </summary>
        /// <param name="x">Coordonnée X de la case</param>
        /// <param name="y">Coordonnée Y de la case</param>
        /// <returns>Le nouvel état de la case visée</returns>
        private State Shot(int x, int y)
        {
            if (players[playerTurn].grid.grid[x, y].state == State.noActivity || players[playerTurn].grid.grid[x, y].state == State.revealed)
            {
                State state;
                if (players[playerTurn].grid.grid[x, y].boat != null)
                {
                    Boat boat = players[playerTurn].grid.grid[x, y].boat;
                    boat.touchedCell++;
                    if (boat.cells.Count == boat.touchedCell)
                    {
                        players[playerTurn].grid.grid[x, y].state = State.fullBoat;
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (players[playerTurn].grid.grid[i, j].boat == boat)
                                {
                                    players[playerTurn].grid.grid[i, j].state = State.fullBoat;
                                }
                            }
                        }
                        players[playerTurn].grid.boats.Remove(players[playerTurn].grid.grid[x, y].boat);
                        if (players[playerTurn].grid.boats.Count == 0)
                        {
                            finish = true;
                        }
                        state = State.fullBoat;
                    }
                    else
                    {
                        players[playerTurn].grid.grid[x, y].state = State.boat;
                        state = State.boat;
                    }
                }
                else
                {
                    players[playerTurn].grid.grid[x, y].state = State.noBoat;
                    state = State.noBoat;
                }
                return state;
            }
            return State.invalid;
        }

        /// <summary>
        /// Révèle l'emplacement d'un bateau
        /// </summary>
        public int[] Sonar()
        {
            playerTurn = (playerTurn + 1) % 2;
            Random random = new Random();
            List<Cell> cells = new List<Cell>();
            foreach (Boat boat in players[playerTurn].grid.boats)
            {
                bool intact = true;
                foreach (Cell cell in boat.cells)
                {
                    if (cell.state != State.noActivity)
                    {
                        intact = false;
                        break;
                    }
                }
                if (intact)
                {
                    cells.Add(boat.cells[random.Next(boat.cells.Count)]); //Pour que les bateaux de différente taille aient le même risque d'être trouvé
                }
            }
            if (cells.Count > 0)
            {
                Cell cell = cells[random.Next(cells.Count)];
                cell.state = State.revealed;
                players[playerTurn].sonars--;
                if(playerTurn == 1 && players[playerTurn].sonars == 0)
                {
                    view.sonar.IsEnabled = false;
                }
                new SoundPlayer(Properties.Resources.sonar).Play();
                view.RefreshGrid();

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(players[playerTurn].playerName + " utilise sonar");
                paragraph.Foreground = playerTurn == 0 ? Brushes.Red : Brushes.Blue;
                paragraph.LineHeight = 1;
                view.history.Document.Blocks.Add(paragraph);
                view.history.ScrollToEnd();
                
                return new int[] { cell.x, cell.y };
            }
            else
            {
                playerTurn = (playerTurn + 1) % 2;
                return new int[0];
            }
        }

        public void Sonar(int x, int y)
        {
            playerTurn = (playerTurn + 1) % 2;
            players[playerTurn].grid.grid[x, y].state = State.revealed;
            players[playerTurn].sonars--;
            if (playerTurn == 1 && players[playerTurn].sonars == 0)
            {
                view.sonar.IsEnabled = false;
            }
            new SoundPlayer(Properties.Resources.sonar).Play();
            view.RefreshGrid();

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(players[playerTurn].playerName + " utilise sonar");
            paragraph.Foreground = playerTurn == 0 ? Brushes.Red : Brushes.Blue;
            paragraph.LineHeight = 1;
            view.history.Document.Blocks.Add(paragraph);
            view.history.ScrollToEnd();
        }

        /// <summary>
        /// Lance une bombe nucléaire en (x;y)
        /// </summary>
        /// <param name="x">Centre X de l'attaque</param>
        /// <param name="y">Centre Y de l'attaque</param>
        public void NuclearAttack(int x, int y)
        {
            playerTurn = (playerTurn + 1) % 2;
            for(int i = 0; i < players[playerTurn].grid.grid.GetLength(0); i++)
            {
                for (int j = 0; j < players[playerTurn].grid.grid.GetLength(1); j++)
                {
                    if (Math.Sqrt(Math.Pow(x - i, 2) + Math.Pow(y - j, 2)) <= Properties.Settings.Default.nuclearBombRange)
                    {
                        Shot(i, j);
                    }
                }
            }

            players[playerTurn].nuclearBombs--;
            if (playerTurn == 1 && players[playerTurn].nuclearBombs == 0)
            {
                view.nuclearBomb.IsEnabled = false;
            }
            new SoundPlayer(Properties.Resources.explosion).Play();
            view.RefreshGrid();
            
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(players[playerTurn].playerName + " lance une bombe nucléaire en " + ((char)(x + 65)).ToString() + (y + 1));
            paragraph.Foreground = playerTurn == 0 ? Brushes.Red : Brushes.Blue;
            paragraph.LineHeight = 1;
            view.history.Document.Blocks.Add(paragraph);
            view.history.ScrollToEnd();
        }
    }
}
