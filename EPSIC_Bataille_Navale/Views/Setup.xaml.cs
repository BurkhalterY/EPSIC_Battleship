using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static EPSIC_Bataille_Navale.Controllers.SetupController;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Page
    {
        public SetupController controller;
        private Button[,] grid;
        public int size;

        public Setup(int size) : base()
        {
            InitializeComponent();
            this.size = size;
            controller = new SetupController(size);
            controller.OnRefresh += new Refresh(OnRefresh);
            controller.OnEnableBtnCancel += new EnableBtnCancel(OnEnableBtnCancel);
            controller.OnEnableBtnNext += new EnableBtnNext(OnEnableBtnNext);
            MakeGrid();
        }

        /// <summary>
        /// Generate the grid dynamically
        /// </summary>
        protected void MakeGrid()
        {
            grid = new Button[size, size];

            for (int i = 0; i < size; i++)
            {
                grid1.RowDefinitions.Add(new RowDefinition());
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new Button();
                    grid[i, j].Tag = new int[] { i, j };
                    grid[i, j].BorderThickness = new Thickness(1.0 / 32.0);
                    grid[i, j].BorderBrush = System.Windows.Media.Brushes.Black;
                    grid[i, j].Click += new RoutedEventHandler(CellClick);
                    grid[i, j].PreviewMouseRightButtonDown += CellRightClick;

                    Grid.SetColumn(grid[i, j], i);
                    Grid.SetRow(grid[i, j], j);
                    grid1.Children.Add(grid[i, j]);
                    OnRefresh(i, j);
                }
            }
        }

        /// <summary>
        /// Refresh the sprite of a case of the grid
        /// </summary>
        public void OnRefresh(int i, int j)
        {
            Sprite sprite = new Sprite(Properties.Resources.water);
            Boat boat = controller.grid.grid[i, j].boat;
            if (boat != null)
            {
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
                            ? boat.cells.IndexOf(controller.grid.grid[i, j])
                            : boat.cells.Count - boat.cells.IndexOf(controller.grid.grid[i, j]) - 1
                    );
                    sprite.RotateSprite(Direction.Right);
                }
            }
            else
            {
                if (controller.possibleCells.Contains(controller.grid.grid[i, j]))
                {
                    sprite.AddSprite(Properties.Resources.light_green);
                }
                else if (controller.clickedCell == controller.grid.grid[i, j])
                {
                    sprite.AddSprite(Properties.Resources.yellow);
                }
            }
            grid[i, j].Background = sprite.ToBrush();
        }
        
        protected void CellClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int[] coord = (int[])button.Tag;
            controller.Click(controller.grid.grid[coord[0], coord[1]]);
        }

        protected void CellRightClick(object sender, MouseButtonEventArgs e)
        {
            Button button = (Button)sender;
            int[] coord = (int[])button.Tag;
            controller.RightClick(coord[0], coord[1]);
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            controller.DeleteLastBoat();
        }

        private void Btn_random_Click(object sender, RoutedEventArgs e)
        {
            controller.AIChoise();
        }

        public void OnEnableBtnCancel(bool active)
        {
            btn_cancel.IsEnabled = active;
        }

        public void OnEnableBtnNext(bool active)
        {
            btn_next.IsEnabled = active;
        }
    }
}
