using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation_of_life
{
    public partial class Form1 : Form
    {

        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int cols;
        private int rows;
        public Form1()
        {
            InitializeComponent();
        }

        private void Start()
        {
            if (timer1.Enabled)
                return;

            tSize.Enabled = false;
            tDensity.Enabled = false;
            comboxSpeed.Enabled = false;

            resolution = (int)tSize.Value;

            cols = pictureBox1.Width / resolution;
            rows = pictureBox1.Height / resolution;
            field = new bool[cols, rows];

            switch (comboxSpeed.SelectedIndex)
            {
                case 0: timer1.Interval = 110; break;
                case 1: timer1.Interval = 70; break;
                case 2: timer1.Interval = 60; break;
                case 3: timer1.Interval = 40; break;
                default: timer1.Interval = 80; break;
            }


            Random r = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = r.Next((int)tDensity.Value) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }
        private int CountNeighbours(int x, int y)
        {
            int neighboursCount = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int column = (x + j + cols) % cols;
                    int row = (y + i + rows) % rows;

                    bool ifSelfChecking = i == 0 && j == 0;
                    bool hasLife = field[column, row];

                    if (hasLife && !ifSelfChecking)
                        neighboursCount++;
                }
            }

            return neighboursCount;
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            bool[,] newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighboursCount = CountNeighbours(x, y);
                    bool hasLife = field[x, y];

                    if (!hasLife && neighboursCount == 3)
                        newField[x, y] = true;
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];

                    if (hasLife)
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }

            field = newField;
            pictureBox1.Refresh();
        }



        private void Stop()
        {
            if (timer1.Enabled == false) return;

            timer1.Stop();
            tSize.Enabled = true;
            tDensity.Enabled = true;
            comboxSpeed.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;

                if (IsMouseInBorders(x, y))
                    field[x, y] = true;
            }

            else if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;

                if (IsMouseInBorders(x, y))
                    field[x, y] = false;
            }

        }

        private bool IsMouseInBorders(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }
    }
}
