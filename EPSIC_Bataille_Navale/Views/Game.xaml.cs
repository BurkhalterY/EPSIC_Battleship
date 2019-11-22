using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public GameController controller;
        private CustomButton[,] grid;
        private CustomButton[,] gridSecond;
        public RichTextBox history;
        public int size;
        public GameType gameType;
        public bool clickable;

        public Game(GameType gameType, int size) : base()
        {
            InitializeComponent();
            this.gameType = gameType;
            this.size = size;
            switch (gameType) //Le controller dépend du type de partie
            {
                case GameType.Solo: controller = new SoloGameController(this); break;
                case GameType.Demo: controller = new DemoGameController(this); break;
            }
            MakeGrid(); //On ne génère les grilles qu'une seule fois
        }

        /// <summary>
        /// Génère dynamiquement les grilles de jeu et les bouton
        /// en fonction des options
        /// </summary>
        private void MakeGrid()
        {
            grid = new CustomButton[size, size];
            Grid gridView = Content as Grid; //Objet graphique auquel on va greffer les boutons
            double cellSize = 450.0 / size; //Taille d'une seule cellule

            gridSecond = new CustomButton[size, size];
            double cellSizeSecond = 225.0 / size;

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
                    grid[i, j].Click += new RoutedEventHandler(CellClick); //Ajout de l'évenement Click (seulement sur la grille 1)
                    gridView.Children.Add(grid[i, j]);

                    gridSecond[i, j] = new CustomButton(i, j);
                    gridSecond[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    gridSecond[i, j].VerticalAlignment = VerticalAlignment.Top;
                    gridSecond[i, j].Margin = new Thickness(i * cellSizeSecond + 475, j * cellSizeSecond + 25, 0, 0);
                    gridSecond[i, j].Width = cellSizeSecond;
                    gridSecond[i, j].Height = cellSizeSecond;
                    gridView.Children.Add(gridSecond[i, j]);
                }
            }

            //Création de la zone de texte contenant l'historique des tirs
            history = new RichTextBox();
            history.HorizontalAlignment = HorizontalAlignment.Left;
            history.VerticalAlignment = VerticalAlignment.Top;
            history.Margin = new Thickness(475, size * cellSizeSecond + 25, 0, 0);
            history.Width = 225;
            history.Height = 225;
            history.IsReadOnly = true;
            history.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            history.Document.Blocks.Clear();
            gridView.Children.Add(history);
        }

        /// <summary>
        /// Actualise le sprite des cases des grilles
        /// </summary>
        public void RefreshGrid()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Sprite sprite = new Sprite(Properties.Resources.water); //Toutes les cases ont de l'eau dessous
                    switch (controller.grids[1].grid[i, j].state)
                    {
                        case State.noBoat:
                            sprite.AddSprite(Properties.Resources.miss);
                            break;
                        case State.boat:
                            sprite.AddSprite(Properties.Resources.touch);
                            break;
                        case State.fullBoat:
                            Boat boat = controller.grids[1].grid[i, j].boat;
                            sprite.RotateSprite(boat.orientation);
                            Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("boat_" + boat.cells.Count); //Load la ressource en fonction du nombre de cases
                            sprite.AddSprite(
                                bitmap != null ? bitmap : new Bitmap(boat.cells.Count, 1) { Tag = new object() }, //Si bitmap = null, alors Renvoie un bitmap sensé mesurer la même taille que celui qui aurait dû être chargé
                                boat.orientation == Direction.Right || boat.orientation == Direction.Down //Determine le sens du bateau
                                    ? boat.cells.IndexOf(controller.grids[1].grid[i, j])
                                    : boat.cells.Count - boat.cells.IndexOf(controller.grids[1].grid[i, j]) - 1
                            );
                            break;
                    }
                    grid[i, j].Background = sprite.ToBrush();

                    sprite = new Sprite(Properties.Resources.water);
                    if (controller.grids[0].grid[i, j].boat != null)
                    {
                        Boat boat = controller.grids[0].grid[i, j].boat;
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
                                    ? boat.cells.IndexOf(controller.grids[0].grid[i, j])
                                    : boat.cells.Count - boat.cells.IndexOf(controller.grids[0].grid[i, j]) - 1
                            );
                        }
                    }
                    switch (controller.grids[0].grid[i, j].state)
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
        }

        private void CellClick(object sender, EventArgs e)
        {
            if (clickable) //Variable définissant à qui est-ce le tour de jouer
            {
                CustomButton customButton = (CustomButton)sender;
                controller.Click(customButton.x, customButton.y);
            }
        }

        /// <summary>
        /// Termine la partie
        /// </summary>
        /// <param name="winnerName">Nom du gagnant</param>
        public void Finish(string winnerName)
        {
            MessageBox.Show(winnerName + " a gagné !");
            Home home = new Home();
            Window.GetWindow(this).Content = home;
            home.lbl_title.Content = winnerName + " a gagné !";
        }
    }
}
