using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static EPSIC_Bataille_Navale.Controllers.GameController;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public GameController controller;
        private Button[,] grid;
        private Button[,] gridSecond;
        public RichTextBox rtb_history;
        public TextBox txt_message;
        public int size;
        public GameType gameType;
        public MenuItem sonar;
        public MenuItem nuclearBomb;
        public bool gridActive;

        public Game(GameType gameType, int size, Player[] players) : base()
        {
            InitializeComponent();
            this.gameType = gameType;
            this.size = size;
            switch (gameType) //Le controller dépend du type de partie
            {
                case GameType.Solo: controller = new SoloGameController(players); break;
                case GameType.Demo: controller = new DemoGameController(players); break;
                case GameType.Host: case GameType.Client: controller = new NetworkGameController(players, gameType); break;
            }

            controller.OnRefresh += new Refresh(OnRefresh);
            controller.OnHistoryUpdate += new HistoryUpdate(OnHistoryUpdate);
            controller.OnActiveGrid += new ActiveGrid(OnActiveGrid);
            controller.OnFinish += new Finish(OnFinish);
            MakeGrid(); //On ne génère les grilles qu'une seule fois

            if (gameType == GameType.Solo || gameType == GameType.Host)
            {
                gridActive = true;
            }
            else if(gameType == GameType.Demo)
            {
                controller.Click();
            }
        }

        /// <summary>
        /// Génère dynamiquement les grilles de jeu et les bouton
        /// en fonction des options
        /// </summary>
        private void MakeGrid()
        {
            grid = new Button[size, size];
            Grid gridView = Content as Grid; //Objet graphique auquel on va greffer les boutons
            double cellSize = 450.0 / size; //Taille d'une seule cellule

            gridSecond = new Button[size, size];
            double cellSizeSecond = 225.0 / size;

            ContextMenu menu = new ContextMenu();
            sonar = new MenuItem();
            sonar.Header = "Sonar";
            sonar.IsEnabled = Properties.Settings.Default.nbSonars > 0;
            sonar.Click += Sonar_Click;
            menu.Items.Add(sonar);
            nuclearBomb = new MenuItem();
            nuclearBomb.Header = "Bombe nucléraire";
            nuclearBomb.IsEnabled = Properties.Settings.Default.nbNuclearBombs > 0;
            nuclearBomb.Click += NuclearBomb_Click; ;
            menu.Items.Add(nuclearBomb);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new Button();
                    grid[i, j].Tag = new int[] { i, j };
                    grid[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    grid[i, j].VerticalAlignment = VerticalAlignment.Top;
                    grid[i, j].Margin = new Thickness(i * cellSize + 25, j * cellSize + 25, 0, 0);
                    grid[i, j].Width = cellSize;
                    grid[i, j].Height = cellSize;
                    grid[i, j].Click += new RoutedEventHandler(Cell_Click); //Ajout de l'évenement Click (seulement sur la grille 1)
                    grid[i, j].ContextMenu = menu;
                    gridView.Children.Add(grid[i, j]);
                    RefreshCell(i, j, 0);

                    gridSecond[i, j] = new Button();
                    gridSecond[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    gridSecond[i, j].VerticalAlignment = VerticalAlignment.Top;
                    gridSecond[i, j].Margin = new Thickness(i * cellSizeSecond + 475, j * cellSizeSecond + 25, 0, 0);
                    gridSecond[i, j].Width = cellSizeSecond;
                    gridSecond[i, j].Height = cellSizeSecond;
                    gridView.Children.Add(gridSecond[i, j]);
                    RefreshCell(i, j, 1);
                }
            }

            //Création de la zone de texte contenant l'historique des tirs
            rtb_history = new RichTextBox();
            rtb_history.HorizontalAlignment = HorizontalAlignment.Left;
            rtb_history.VerticalAlignment = VerticalAlignment.Top;
            rtb_history.Margin = new Thickness(475, size * cellSizeSecond + 25, 0, 0);
            rtb_history.Width = 225;
            rtb_history.Height = 200;
            rtb_history.IsReadOnly = true;
            rtb_history.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            rtb_history.Document.Blocks.Clear();
            gridView.Children.Add(rtb_history);

            txt_message = new TextBox();
            txt_message.HorizontalAlignment = HorizontalAlignment.Left;
            txt_message.VerticalAlignment = VerticalAlignment.Top;
            txt_message.Margin = new Thickness(475, size * cellSizeSecond + 25 + rtb_history.Height, 0, 0);
            txt_message.Width = 225;
            txt_message.Height = 25;
            txt_message.KeyDown += Txt_message_KeyDown;
            gridView.Children.Add(txt_message);
        }

        /// <summary>
        /// Actualise le sprite d'une case de la grille
        /// </summary>
        public void RefreshCell(int i, int j, int gridToRefresh)
        {
            if(gridToRefresh == 0)
            {
                Sprite sprite = new Sprite(Properties.Resources.water); //Toutes les cases ont de l'eau dessous
                switch (controller.players[1].grid.grid[i, j].state)
                {
                    case State.noBoat:
                        sprite.AddSprite(Properties.Resources.miss);
                        break;
                    case State.boat:
                        sprite.AddSprite(Properties.Resources.touch);
                        break;
                    case State.fullBoat:
                    case State.revealed:
                        Boat boat = controller.players[1].grid.grid[i, j].boat;
                        sprite.RotateSprite(boat.orientation);
                        Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("boat_" + boat.cells.Count); //Load la ressource en fonction du nombre de cases
                        sprite.AddSprite(
                            bitmap != null ? bitmap : new Bitmap(boat.cells.Count, 1) { Tag = new object() }, //Si bitmap = null, alors Renvoie un bitmap sensé mesurer la même taille que celui qui aurait dû être chargé
                            boat.orientation == Direction.Right || boat.orientation == Direction.Down //Determine le sens du bateau
                                ? boat.cells.IndexOf(controller.players[1].grid.grid[i, j])
                                : boat.cells.Count - boat.cells.IndexOf(controller.players[1].grid.grid[i, j]) - 1
                        );
                        break;
                }
                grid[i, j].Background = sprite.ToBrush();
            }
            else if(gridToRefresh == 1)
            {

                Sprite sprite = new Sprite(Properties.Resources.water);
                if (controller.players[0].grid.grid[i, j].boat != null)
                {
                    Boat boat = controller.players[0].grid.grid[i, j].boat;
                    if (boat.cells.Count == 1)
                    {
                        sprite.AddSprite(Properties.Resources.mine);
                    }
                    else
                    {
                        sprite.RotateSprite(boat.orientation);
                        Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("boat_" + boat.cells.Count);
                        sprite.AddSprite(
                            bitmap != null ? bitmap : new Bitmap(boat.cells.Count, 1) { Tag = new object() },
                            boat.orientation == Direction.Right || boat.orientation == Direction.Down
                                ? boat.cells.IndexOf(controller.players[0].grid.grid[i, j])
                                : boat.cells.Count - boat.cells.IndexOf(controller.players[0].grid.grid[i, j]) - 1
                        );
                    }
                }
                switch (controller.players[0].grid.grid[i, j].state)
                {
                    case State.noActivity:
                        sprite.AddSprite(Properties.Resources.hide);
                        break;
                    case State.noBoat:
                        sprite.AddSprite(Properties.Resources.miss);
                        break;
                    case State.boat:
                        sprite.AddSprite(Properties.Resources.touch);
                        break;
                }
                gridSecond[i, j].Background = sprite.ToBrush();
            }
        }

        public void OnRefresh(int x, int y, int gridToRefresh)
        {
            RefreshCell(x, y, gridToRefresh);
        }

        public void OnHistoryUpdate(string message, int playerTurn)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(message);
            paragraph.Foreground = playerTurn == 0 ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Blue;
            paragraph.LineHeight = 1;
            rtb_history.Document.Blocks.Add(paragraph);
            rtb_history.ScrollToEnd();
        }

        private void Txt_message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                controller.SendMessage(txt_message.Text, 0);
                txt_message.Clear();
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            if (gridActive) //Variable définissant à qui est-ce le tour de jouer
            {
                Button button = (Button)sender;
                int[] coord = (int[])button.Tag;
                controller.Click(coord[0], coord[1]);
            }
        }

        private void Sonar_Click(object sender, RoutedEventArgs e)
        {
            if (gridActive)
            {
                controller.Click(0, 0, ActionType.Sonar);
                if (controller.players[0].sonars == 0)
                {
                    sonar.IsEnabled = false;
                }
            }
        }

        private void NuclearBomb_Click(object sender, RoutedEventArgs e)
        {
            if (gridActive)
            {
                MenuItem item = sender as MenuItem;
                int[] coord = (int[])(((ContextMenu)item.Parent).PlacementTarget as Button).Tag;
                controller.Click(coord[0], coord[1], ActionType.NuclearBomb);

                if (controller.players[0].nuclearBombs == 0)
                {
                    nuclearBomb.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Termine la partie
        /// </summary>
        /// <param name="winnerName">Nom du gagnant</param>
        public void OnFinish(string winnerName)
        {
            MessageBox.Show(winnerName + " a gagné !");
            Home home = new Home();
            MainWindow.LoadPage(home);
            home.lbl_title.Content = winnerName + " a gagné !";
        }

        public void OnActiveGrid(bool active)
        {
            gridActive = active;
        }
    }
}
