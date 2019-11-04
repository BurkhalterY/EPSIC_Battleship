using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class Game : GridView
    {
        public GameController controller;
        private CustomPictureBox[,] gridSecond;

        public Game(int size, int code) : base(size)
        {
            InitializeComponent();
            switch (code)
            {
                case 0: controller = new SoloGameController(this); break;
            }
            ClearGrid();
        }

        public void MakeSecondGrid(int size)
        {
            gridSecond = new CustomPictureBox[size, size];

            int cellSize = Width / 3 / size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    gridSecond[i, j] = new CustomPictureBox(i, j);
                    gridSecond[i, j].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    gridSecond[i, j].Location = new System.Drawing.Point(i * cellSize + 475, j * cellSize + 25);
                    gridSecond[i, j].Name = "cell_second" + i + "_" + j;
                    gridSecond[i, j].Size = new System.Drawing.Size(cellSize, cellSize);
                    gridSecond[i, j].TabIndex = 2;
                    gridSecond[i, j].TabStop = false;
                    Controls.Add(gridSecond[i, j]);
                }
            }
            RefreshGrid();
        }

        protected override void CellClick(object sender, EventArgs e)
        {
            CustomPictureBox customPictureBox = (CustomPictureBox)sender;
            controller.Click(customPictureBox.x, customPictureBox.y);
        }

        public void RefreshGrid()
        {
            Grid dataGrid = controller.grids[1];
            for (int i = 0; i < dataGrid.grid.GetLength(0); i++)
            {
                for (int j = 0; j < dataGrid.grid.GetLength(1); j++)
                {
                    switch (dataGrid.grid[i, j].state)
                    {
                        case State.noActivity:
                            grid[i, j].BackColor = Color.White;
                            break;
                        case State.noBoat:
                            grid[i, j].BackColor = Color.Yellow;
                            break;
                        case State.boat:
                            grid[i, j].BackColor = Color.Red;
                            break;
                        case State.fullBoat:
                            grid[i, j].BackColor = Color.DarkRed;
                            break;
                    }
                }
            }

            dataGrid = controller.grids[0];
            for (int i = 0; i < dataGrid.grid.GetLength(0); i++)
            {
                for (int j = 0; j < dataGrid.grid.GetLength(1); j++)
                {
                    switch (dataGrid.grid[i, j].state)
                    {
                        case State.noActivity:
                            if (dataGrid.grid[i, j].boat != null)
                            {
                                gridSecond[i, j].BackColor = Color.Aqua;
                            }
                            else
                            {
                                gridSecond[i, j].BackColor = Color.White;
                            }
                            break;
                        case State.noBoat:
                            gridSecond[i, j].BackColor = Color.Yellow;
                            break;
                        case State.boat:
                            gridSecond[i, j].BackColor = Color.Red;
                            break;
                        case State.fullBoat:
                            gridSecond[i, j].BackColor = Color.DarkRed;
                            break;
                    }
                }
            }
        }
    }
}
