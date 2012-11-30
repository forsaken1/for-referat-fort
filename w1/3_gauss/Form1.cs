using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace task
{
    public partial class Form1 : Form
    {
        private bool mode;
        private Task sample;

        public Form1()
        {
            InitializeComponent();
        }

        private void Activate()
        {
            openFileButton.Visible = true;
            solveButton.Visible = true;
            monitor.Visible = true;
            result.Visible = true;
            SLAEButton.Visible = false;
            IntegralButton.Visible = false;
        }

        private void SLAEButton_Click(object sender, EventArgs e)
        {
            Activate();
            mode = true;
        }

        private void IntegralButton_Click(object sender, EventArgs e)
        {
            Activate();
            mode = false;
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                solveButton.Enabled = true;

                using (StreamReader sr = new StreamReader(File.Open(openFileDialog1.FileName, FileMode.Open)))
                {
                    if (mode)
                        sample = new SLAE(sr, monitor, result);
                    else
                        sample = new Integral(sr, monitor);
                }
            }
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            sample.ShowData();
        }
    }

    public class Task
    {
        public virtual void Read() { }
        public virtual void Write() { }
        public virtual void Solve() { }
        public virtual void ShowData() { }
    }

    public class SLAE : Task
    {
        private double[,] A;
        private double[] B, X;
        private int n, m;
        private StreamReader sr;
        private RichTextBox mon, res;

        public SLAE(StreamReader _sr, RichTextBox _mon, RichTextBox _res)
        {
            sr = _sr;
            mon = _mon;
            res = _res;
            Read();
        }

        public override void Read()
        {
            string[] str = new string[100];
            n = 0;
            A = new double[100, 100];
            B = new double[100];

            while (!sr.EndOfStream)
                str[n++] = sr.ReadLine();

            m = str[0].Length - 1;

            mon.Clear();

            for (int i = 0; i < n; i++)
            {
                string[] s = str[i].Split(' ');
                string st = "";

                for (int j = 0; j < s.Length; j++)
                {
                    if (j == s.Length - 1)
                    {
                        B[i] = double.Parse(s[j]);
                        st += " = " + s[j];
                    }
                    else
                    {
                        A[i, j] = double.Parse(s[j]);
                        if (j == 0)
                            st += s[j] + " x" + (j + 1);
                        else
                            st += (A[i, j] > 0 ? (" + " + s[j]) : (" - " + (- A[i, j]))) + " x" + (j + 1);
                    }
                }
                mon.AppendText(st + "\n");
            }
        }

        public override void Write()
        {

        }

        public override void Solve()
        {
            X = new double[100];
            Gauss(A, B, X);
        }

        double fabs(double d) { return d < 0 ? -d : d; }

        void Gauss(double[,] a, double[] b, double[] x)
        {
            double v;
            for (int k = 0, i, j, im; k < n - 1; k++)
            {
                im = k;
                for (i = k + 1; i < n; i++)
                {
                    if (fabs(a[im, k]) < fabs(a[i, k]))
                    {
                        im = i;
                    }
                }
                if (im != k)
                {
                    for (j = 0; j < n; j++)
                    {
                        v = a[im, j];
                        a[im, j] = a[k, j];
                        a[k, j] = v;
                    }
                    v = b[im];
                    b[im] = b[k];
                    b[k] = v;
                }
                for (i = k + 1; i < n; i++)
                {
                    v = 1.0 * a[i, k] / a[k, k];
                    a[i, k] = 0;
                    b[i] = b[i] - v * b[k];
                    if (v != 0)
                        for (j = k + 1; j < n; j++)
                        {
                            a[i, j] = a[i, j] - v * a[k, j];
                        }
                }
            }

            double s = 0;
            x[n - 1] = 1.0 * b[n - 1] / a[n - 1, n - 1];
            for (int i = n - 2, j; 0 <= i; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + a[i, j] * x[j];
                }
                x[i] = 1.0 * (b[i] - s) / a[i, i];
            }
        }
        public override void ShowData()
        {
            Solve();
            string str = "";

            for (int i = 0; i < n; i++)
                str += "x" + (i + 1) + " = " + X[i] + "; ";

            res.Clear();
            res.AppendText(str);
        }
    }

    public class Integral : Task
    {
        private string str;
        private int a, b;
        private StreamReader sr; 
        private RichTextBox mon;

        public Integral(StreamReader _sr, RichTextBox _mon)
        {
            sr = _sr;
            mon = _mon;
            Read();
        }

        public override void Read()
        {
            str = sr.ReadLine();
            str.Split(' ');

            mon.AppendText(str);
        }

        public override void Write()
        {

        }

        public override void Solve()
        {

        }

        public override void ShowData()
        {

        }
    }
}
