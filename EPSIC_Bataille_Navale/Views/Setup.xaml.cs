using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Setup.xaml
    /// </summary>
    public partial class Setup : Page
    {
        public SetupController controller;
        private Button[,] grid;
        public int size;

        public Setup() : base()
        {
            InitializeComponent();
            size = Properties.Settings.Default.size;
            controller = new SetupController(this, size);
            MakeGrid();
            RefreshGrid();
        }

        /// <summary>
        /// Génère dynamiquement la grille
        /// </summary>
        protected void MakeGrid()
        {
            grid = new Button[size, size];
            Grid gridView = Content as Grid;
            int cellSize = 450 / size;

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
                    grid[i, j].Click += new RoutedEventHandler(CellClick);
                    grid[i, j].PreviewMouseRightButtonDown += CellRightClick;
                    gridView.Children.Add(grid[i, j]);
                }
            }
        }

        /// <summary>
        /// Actualise le sprite des cases de la grille
        /// </summary>
        public void RefreshGrid()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
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
                        }
                    }
                    grid[i, j].Background = sprite.ToBrush();
                }
            }
            for (int i = 0; i < controller.possibleCells.Count; i++)
            {
                grid[controller.possibleCells[i][0], controller.possibleCells[i][1]].Background = System.Windows.Media.Brushes.LightGreen;
            }
            if (controller.clickedCell.Length == 2)
            {
                grid[controller.clickedCell[0], controller.clickedCell[1]].Background = System.Windows.Media.Brushes.Yellow;
            }
        }
        
        protected void CellClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int[] coord = (int[])button.Tag;
            controller.Click(coord[0], coord[1]);
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
    }
}
