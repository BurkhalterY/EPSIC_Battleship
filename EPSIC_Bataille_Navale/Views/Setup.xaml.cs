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
        private CustomButton[,] grid;
        public int size;

        public Setup() : base()
        {
            InitializeComponent();
            size = Properties.Settings.Default.size;
            controller = new SetupController(this, size);
            MakeGrid();
            RefreshGrid();
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            controller.DeleteLastBoat();
        }

        public void EnableNextButton(bool value)
        {
            btn_next.IsEnabled = value;
        }

        public void EnableCancelButton(bool value)
        {
            btn_cancel.IsEnabled = value;
        }

        private void Btn_next_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void Finish()
        {
            Setup setup = new Setup();
            setup.controller.AIChoise();

            Game game = new Game(GameType.Solo, size);
            game.controller.grids = new Models.GridModel[] { controller.grid, setup.controller.grid };
            game.controller.playersNames = new string[] { controller.playerName, setup.controller.playerName };
            Window.GetWindow(this).Content = game;
            game.RefreshGrid();
        }

        protected void MakeGrid()
        {
            grid = new CustomButton[size, size];
            Grid gridView = Content as Grid;
            int cellSize = 450 / size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new CustomButton(i, j);
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

        protected void CellClick(object sender, EventArgs e)
        {
            CustomButton customButton = (CustomButton)sender;
            controller.Click(customButton.x, customButton.y);
        }

        protected void CellRightClick(object sender, MouseButtonEventArgs e)
        {
            CustomButton customButton = (CustomButton)sender;
            controller.RightClick(customButton.x, customButton.y);
        }

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
                            sprite.AddSprite((Bitmap)Properties.Resources.ResourceManager.GetObject("boat_" + boat.cells.Count), boat.orientation == Direction.Right || boat.orientation == Direction.Down ? boat.cells.IndexOf(controller.grid.grid[i, j]) : boat.cells.Count - boat.cells.IndexOf(controller.grid.grid[i, j]) - 1, 0);
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

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Home();
        }
    }
}
