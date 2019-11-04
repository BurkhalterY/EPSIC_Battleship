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
    public partial class GridView : UserControl
    {
        protected CustomPictureBox[,] grid;

        public GridView()
        {
            InitializeComponent();
            MakeGrid(10);
        }

        public GridView(int size)
        {
            InitializeComponent();
            MakeGrid(size);
        }

        protected void MakeGrid(int size)
        {
            grid = new CustomPictureBox[size, size];

            int cellSize = Width / size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new CustomPictureBox(i, j);
                    grid[i, j].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    grid[i, j].Location = new System.Drawing.Point(i * cellSize + 25, j * cellSize + 25);
                    grid[i, j].Name = "cell" + i + "_" + j;
                    grid[i, j].Size = new System.Drawing.Size(cellSize, cellSize);
                    grid[i, j].TabIndex = 2;
                    grid[i, j].TabStop = false;
                    grid[i, j].Click += new System.EventHandler(CellClick);
                    Controls.Add(grid[i, j]);
                }
            }
        }

        protected void ClearGrid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].BackColor = Color.White;
                }
            }
        }


        protected virtual void CellClick(object sender, EventArgs e) { }
    }
}
