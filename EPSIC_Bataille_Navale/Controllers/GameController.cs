using EPSIC_Bataille_Navale.Models;
using System;
using System.Collections.Generic;
using System.Media;

namespace EPSIC_Bataille_Navale.Controllers
{
    public enum ActionType {
        NormalShot,
        Sonar,
        NuclearBomb
    }

    public abstract class GameController
    {
        public delegate void Refresh(int x, int y, int gridToRefresh);
        public delegate void HistoryUpdate(string message, int playerTurn);
        public delegate void ActiveGrid(bool active);
        public delegate void Finish(string winnerName);

        public event Refresh OnRefresh;
        public event HistoryUpdate OnHistoryUpdate;
        public event ActiveGrid OnActiveGrid;
        public event Finish OnFinish;


        public Player[] players = new Player[2];
        public int playerTurn = 0, playerNotTurn = 1;
        public bool finish = false;

        public GameController(Player[] players)
        {
            this.players = players;
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
            State state = Shot(x, y);
            if(state != State.invalid)
            {
                OnHistoryUpdate(players[playerTurn].playerName + "\t: " + ((char)(x + 65)).ToString() + (y + 1), playerTurn);
                InvertPlayer();
                if (state == State.noBoat)
                {
                    new SoundPlayer(Properties.Resources.splash).Play();
                }
                else if (state == State.boat || state == State.fullBoat)
                {
                    new SoundPlayer(Properties.Resources.explosion).Play();
                }
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
            if (players[playerNotTurn].grid.grid[x, y].state == State.noActivity || players[playerNotTurn].grid.grid[x, y].state == State.revealed)
            {
                State state;
                if (players[playerNotTurn].grid.grid[x, y].boat != null)
                {
                    Boat boat = players[playerNotTurn].grid.grid[x, y].boat;
                    boat.touchedCell++;
                    if (boat.cells.Count == boat.touchedCell)
                    {
                        players[playerNotTurn].grid.grid[x, y].state = State.fullBoat;
                        foreach (Cell cell in boat.cells)
                        {
                            cell.state = State.fullBoat;
                            OnRefresh(cell.x, cell.y, playerTurn);
                        }
                        players[playerNotTurn].grid.boats.Remove(players[playerNotTurn].grid.grid[x, y].boat);
                        if (players[playerNotTurn].grid.boats.Count == 0)
                        {
                            finish = true;
                        }
                        state = State.fullBoat;
                    }
                    else
                    {
                        players[playerNotTurn].grid.grid[x, y].state = State.boat;
                        state = State.boat;
                        OnRefresh(x, y, playerTurn);
                    }
                }
                else
                {
                    players[playerNotTurn].grid.grid[x, y].state = State.noBoat;
                    state = State.noBoat;
                    OnRefresh(x, y, playerTurn);
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
            if (players[playerTurn].sonars > 0)
            {
                Random random = new Random();
                List<Cell> cells = new List<Cell>();
                foreach (Boat boat in players[playerNotTurn].grid.boats)
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

                    new SoundPlayer(Properties.Resources.sonar).Play();

                    OnRefresh(cell.x, cell.y, playerTurn);
                    OnHistoryUpdate(players[playerTurn].playerName + " utilise sonar", playerTurn);
                    InvertPlayer();
                    return new int[] { cell.x, cell.y };
                }
            }
            return new int[0];
        }

        public bool Sonar(int x, int y)
        {
            if (players[playerTurn].sonars > 0)
            {
                players[playerNotTurn].grid.grid[x, y].state = State.revealed;
                players[playerTurn].sonars--;
                new SoundPlayer(Properties.Resources.sonar).Play();
                OnRefresh(x, y, playerTurn);
                OnHistoryUpdate(players[playerTurn].playerName + " utilise sonar", playerTurn);
                InvertPlayer();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Lance une bombe nucléaire en (x;y)
        /// </summary>
        /// <param name="x">Centre X de l'attaque</param>
        /// <param name="y">Centre Y de l'attaque</param>
        public List<int[]> NuclearAttack(int x, int y)
        {
            List<int[]> shots = new List<int[]>();
            if (players[playerTurn].nuclearBombs > 0)
            {
                new SoundPlayer(Properties.Resources.explosion).Play();

                for (int i = 0; i < players[playerNotTurn].grid.grid.GetLength(0); i++)
                {
                    for (int j = 0; j < players[playerNotTurn].grid.grid.GetLength(1); j++)
                    {
                        if (Math.Sqrt(Math.Pow(x - i, 2) + Math.Pow(y - j, 2)) <= Properties.Settings.Default.nuclearBombRange)
                        {
                            shots.Add(new int[] { i, j, (int)Shot(i, j) });
                            OnRefresh(i, j, playerTurn);
                        }
                    }
                }
                players[playerTurn].nuclearBombs--;
                OnHistoryUpdate(players[playerTurn].playerName + " lance une bombe nucléaire en " + ((char)(x + 65)).ToString() + (y + 1), playerTurn);
                InvertPlayer();
            }
            return shots;
        }

        public virtual bool SendMessage(string message, int player)
        {
            if(message != "")
            {
                OnHistoryUpdate(players[player].playerName + " : " + message, player);
                return true;
            }
            return false;
        }

        protected void InvertPlayer()
        {
            int prov = playerTurn;
            playerTurn = playerNotTurn;
            playerNotTurn = prov;
        }

        protected void RaiseOnRefresh(int x, int y, int gridToRefresh)
        {
            OnRefresh(x, y, gridToRefresh);
        }

        protected void RaiseOnHistoryUpdate(string message, int playerTurn)
        {
            OnHistoryUpdate(message, playerTurn);
        }

        protected void RaiseOnActiveGrid(bool active)
        {
            OnActiveGrid(active);
        }

        protected void RaiseOnFinish(string winnerName)
        {
            OnFinish(winnerName);
        }
    }
}
