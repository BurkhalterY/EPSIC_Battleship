using System;
using System.Drawing;
using System.Windows.Forms;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class GridView : UserControl
    {
        protected CustomPictureBox[,] grid;
        public int size = 10;

        public GridView()
        {
            InitializeComponent();
            MakeGrid();
        }

        public GridView(int size)
        {
            InitializeComponent();
            this.size = size;
            MakeGrid();
        }

        protected void MakeGrid()
        {
            grid = new CustomPictureBox[size, size];

            int cellSize = Width / size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new CustomPictureBox(i, j);
                    grid[i, j].BorderStyle = BorderStyle.FixedSingle;
                    grid[i, j].Location = new Point(i * cellSize + 25, j * cellSize + 25);
                    grid[i, j].Name = "cell" + i + "_" + j;
                    grid[i, j].Size = new System.Drawing.Size(cellSize, cellSize);
                    grid[i, j].BackColor = Color.White;
                    grid[i, j].TabIndex = 2;
                    grid[i, j].TabStop = false;
                    grid[i, j].Click += new EventHandler(CellClick);
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
