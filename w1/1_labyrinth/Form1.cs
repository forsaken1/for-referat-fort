using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private const int INF = 100000;
        private string[] map, origin;
        private int n;
        private Point input, first, second;
        private ArrayList path;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 2;
            path = new ArrayList();
        }

        private void DrawLabyrinth(string[] _map) {
            int size = pictureBox1.Height / n;
            Graphics g = pictureBox1.CreateGraphics();
            Brush brush = Brushes.White;

            g.FillRectangle(brush, new Rectangle(new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height)));

            for (int i = 0; i < n; i++)
                for (int j = 0; j < _map[i].Length; j++)
                {
                    switch (_map[i][j])
                    {
                        case '#': brush = Brushes.Black; break;
                        case '!': brush = Brushes.Red;   input  = new Point(i, j); break;
                        case '@': brush = Brushes.Green; break;
                        case ' ': brush = Brushes.White; break;
                        case '.': brush = Brushes.Yellow; break;
                        default: brush = Brushes.Blue; break;
                    }
                    g.FillRectangle(brush, new Rectangle(new Point(j * size, i * size), new Size(size - 1, size - 1)));
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                n = 0;
                map = new string[100];
                origin = new string[100];

                using (StreamReader sr = new StreamReader(File.Open(openFileDialog1.FileName, FileMode.Open)))
                {
                    while (!sr.EndOfStream)
                       map[n++] = sr.ReadLine();
                }
                for (int i = 0; i < n; i++)
                    origin[i] = map[i];

                DrawLabyrinth(origin);
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < n; i++)
                map[i] = origin[i];

            Point p = GoToCrossroad(comboBox1.SelectedIndex, input.X, input.Y);
            for (int i = 0; i < path.Count; i++)
            {
                MarkStep(((Point)path[i]).X, ((Point)path[i]).Y);
            }
            path.Clear();

            if (p.X >= INF)
                MessageBox.Show("No path");
            else
                MessageBox.Show("Steps: " + p.X + "; Scores: " + p.Y);

            DrawLabyrinth(map);
        }

        private bool Finish(int i, int j)
        {
            return map[i][j] == '@';
        }

        private bool IsWall(int i, int j)
        {
            return map[i][j] == '#';
        }

        private bool IsPath(int i, int j)
        {
            return map[i][j] == '.';
        }

        private bool Impasse(int i, int j)
        {
            if (IsWall(i + 1, j) || IsPath(i + 1, j))
                if (IsWall(i - 1, j) || IsPath(i - 1, j))
                    if (IsWall(i, j + 1) || IsPath(i, j + 1))
                        if (IsWall(i, j - 1) || IsPath(i, j - 1))
                            return true;
            return false;
        }

        private bool Crossroad(int i, int j)
        {
            int pathCount = 0;
            int[,] a = { {1, 0}, {-1, 0}, {0, 1}, {0, -1} };

            for(int k = 0; k < 4; k++)
                if ( !IsWall(i + a[k, 0], j + a[k, 1]) && !IsPath(i + a[k, 0], j + a[k, 1]) )
                    if (pathCount == 0)
                    {
                        first.X = i + a[k, 0];
                        first.Y = j + a[k, 1];
                        pathCount++;
                    }
                    else
                    {
                        second.X = i + a[k, 0];
                        second.Y = j + a[k, 1];
                        pathCount++;
                    }

            if (pathCount == 2)
                return true;
            else
                return false;
        }

        private void MarkStep(int i, int j)
        {
            char[] str = map[i].ToCharArray();
            str[j] = '.';
            map[i] = new string(str);
        }

        private void UnMarkStep(int i, int j)
        {
            char[] str = map[i].ToCharArray();
            str[j] = origin[i][j];
            map[i] = new string(str);
        }

        private Point GoToCrossroad(int mode, int i, int j)
        {
            if (Finish(i, j))
                return new Point(0, 0);

            int score = map[i][j] - '0';
            if (score > 9 || score < 0)
                score = 0;

            if (Impasse(i, j))
                return new Point(INF, (mode == 1 || mode == 4) ? 0 : INF);

            MarkStep(i, j);
            path.Add(new Point(i, j));

            if (Crossroad(i, j))
            {
                Point _second = new Point(second.X, second.Y);

                int from = path.Count;
                Point f = GoToCrossroad(mode, first.X, first.Y);
                int to = path.Count;

                ArrayList p = new ArrayList();
                for (int k = to - 1; k >= from; k--)
                {
                    p.Add(path[k]);
                    path.RemoveAt(k);
                }

                int _from = path.Count;
                Point _f = GoToCrossroad(mode, _second.X, _second.Y);
                int _to = path.Count;

                bool b = false;
                switch (mode)
                {
                    case 0:
                        if (f.Y > _f.Y && _f.X < INF || f.X >= INF)
                            b = true;
                        break;

                    case 1:
                        if (f.Y < _f.Y && _f.X < INF || f.X >= INF)
                            b = true;
                        break;

                    case 2: b = f.X > _f.X; break;

                    case 3:
                        if (_f.Y < f.Y && _f.X < f.X || f.X >= INF)
                            b = true;
                        break;

                    case 4:
                        if (_f.Y > f.Y && _f.X < f.X || f.X >= INF) //Отличия от просто Max Scores видны в lab7
                            b = true;
                        break;
                }
                if (b)
                {
                    UnMarkStep(i, j);
                    return new Point(1 + _f.X, score + _f.Y);
                }
                else
                {
                    ArrayList _p = new ArrayList();
                    for (int k = _to - 1; k >= _from; k--)
                    {
                        _p.Add(path[k]);
                        path.RemoveAt(k);
                    }
                    for (int k = 0; k < p.Count; k++)
                        path.Add(p[k]);

                    UnMarkStep(i, j);
                    return new Point(1 + f.X, score + f.Y);
                }
            }
            else
            {
                Point d = GoToCrossroad(mode, first.X, first.Y);
                UnMarkStep(i, j);
                return new Point(1 + d.X, score + d.Y);
            }
        }
    }
}
