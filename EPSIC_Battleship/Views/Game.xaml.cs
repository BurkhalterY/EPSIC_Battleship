using EPSIC_Battleship.Controllers;
using EPSIC_Battleship.I18n;
using EPSIC_Battleship.Models;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static EPSIC_Battleship.Controllers.GameController;

namespace EPSIC_Battleship.Views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public GameController controller;
        private Button[,] grid;
        private Button[,] gridSecond;
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
            switch (gameType) // The controller depends on the game mode
            {
                case GameType.Solo: controller = new SoloGameController(players); break;
                case GameType.Demo: controller = new DemoGameController(players); break;
                case GameType.Host: case GameType.Client: controller = new NetworkGameController(players, gameType); break;
            }

            controller.OnRefresh += new Refresh(OnRefresh);
            controller.OnHistoryUpdate += new HistoryUpdate(OnHistoryUpdate);
            controller.OnActiveGrid += new ActiveGrid(OnActiveGrid);
            controller.OnFinish += new Finish(OnFinish);
            MakeGrid(); // Generate grids only once
            rtb_history.Document.Blocks.Clear();

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
        /// Generate the grid and buttons dynamically depending on settings
        /// </summary>
        private void MakeGrid()
        {
            grid = new Button[size, size];
            gridSecond = new Button[size, size];

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
                grid1.RowDefinitions.Add(new RowDefinition());
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                grid2.RowDefinitions.Add(new RowDefinition());
                grid2.ColumnDefinitions.Add(new ColumnDefinition());

                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new Button();
                    grid[i, j].Tag = new int[] { i, j };
                    grid[i, j].BorderThickness = new Thickness(1.0 / 32.0);
                    grid[i, j].BorderBrush = System.Windows.Media.Brushes.Black;
                    grid[i, j].Click += new RoutedEventHandler(Cell_Click); // Add Click event only on grid 1
                    grid[i, j].ContextMenu = menu;

                    Grid.SetColumn(grid[i, j], i);
                    Grid.SetRow(grid[i, j], j);
                    grid1.Children.Add(grid[i, j]);
                    RefreshCell(i, j, 0);


                    gridSecond[i, j] = new Button();
                    gridSecond[i, j].BorderThickness = new Thickness(1.0 / 32.0);
                    gridSecond[i, j].BorderBrush = System.Windows.Media.Brushes.Black;

                    Grid.SetColumn(gridSecond[i, j], i);
                    Grid.SetRow(gridSecond[i, j], j);
                    grid2.Children.Add(gridSecond[i, j]);
                    RefreshCell(i, j, 1);
                }
            }
        }

        /// <summary>
        /// Refresh the sprite of a case of the grid
        /// </summary>
        public void RefreshCell(int i, int j, int gridToRefresh)
        {
            if(gridToRefresh == 0)
            {
                Sprite sprite = new Sprite(Properties.Resources.water); // Every cases have water on background
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
                    case State.noFind:
                    case State.partialFind:
                        Boat boat = controller.players[1].grid.grid[i, j].boat;
                        sprite.RotateSprite(boat.orientation);
                        Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("boat_" + boat.cells.Count); // Choose the bitmap based on the boat size
                        sprite.AddSprite(
                            bitmap != null ? bitmap : new Bitmap(boat.cells.Count, 1) { Tag = new object() }, // If bitmap is null, then create a new bitmap with the size of the missing one
                            boat.orientation == Direction.Right || boat.orientation == Direction.Down // Choose the boat orientation
                                ? boat.cells.IndexOf(controller.players[1].grid.grid[i, j])
                                : boat.cells.Count - boat.cells.IndexOf(controller.players[1].grid.grid[i, j]) - 1
                        );
                        sprite.RotateSprite(Direction.Right);
                        if (controller.players[1].grid.grid[i, j].state == State.noFind)
                        {
                            sprite.AddSprite(Properties.Resources.hide);
                        }
                        else if (controller.players[1].grid.grid[i, j].state == State.partialFind)
                        {
                            sprite.AddSprite(Properties.Resources.touch);
                        }
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
                        sprite.RotateSprite(Direction.Right);
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
            if (gridActive) // To avoid playing when not our turn
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
        /// The game is finish
        /// </summary>
        /// <param name="winnerName">Name of the winner</param>
        public void OnFinish(string winnerName)
        {
            string msg = string.Format(Strings.MsgPlayerWins, winnerName);
            MessageBox.Show(msg);
            Home home = new Home();
            MainWindow.LoadPage(home);
            home.lbl_title.Content = msg;
        }

        public void OnActiveGrid(bool active)
        {
            gridActive = active;
        }

        private void btn_quit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoadPage(new Home());
        }
    }
}
