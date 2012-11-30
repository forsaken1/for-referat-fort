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

        private void SLAEButton_Click(object sender, EventArgs e)
        {
            openFileButton.Visible = true;
            solveButton.Visible = true;
            monitor.Visible = true;
            result.Visible = true;
            SLAEButton.Visible = false;
            IntegralButton.Visible = false;
            mode = true;
        }

        private void IntegralButton_Click(object sender, EventArgs e)
        {
            openFileButton.Visible = true;
            solveButton.Visible = true;
            monitor.Visible = true;
            result.Visible = true;
            SLAEButton.Visible = false;
            IntegralButton.Visible = false;
            mode = false;
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(File.Open(openFileDialog1.FileName, FileMode.Open)))
                {
                    if (mode)
                        sample = new SLAE(sr, monitor);
                    else
                        sample = new Integral(sr, monitor);
                }
            }
        }
    }

    public class Task
    {
        public Task()
        {

        }

        public virtual void Read(StreamReader sr, RichTextBox edit)
        {

        }

        public virtual void Write()
        {

        }

        public virtual void Solve()
        {

        }

        public virtual void ShowData()
        {

        }
    }

    public class SLAE : Task
    {
        private double[,] slae;
        private int n;

        public SLAE(StreamReader sr, RichTextBox edit)
        {
            Read(sr, edit);
        }

        public override void Read(StreamReader sr, RichTextBox edit)
        {
            string[] str = new string[100];
            n = 0;
            slae = new double[100, 100];

            while (!sr.EndOfStream)
                str[n++] = sr.ReadLine();

            for (int i = 0; i < n; i++)
            {
                string[] s = str[i].Split(' ');
                for (int j = 0; j < s.Length; j++)
                    slae[i, j] = double.Parse(s[j]);

                edit.AppendText(str[i] + "\n");
            }
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

    public class Integral : Task
    {
        public Integral(StreamReader sr, RichTextBox edit)
        {

        }

        public override void Read(StreamReader sr, RichTextBox edit)
        {

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
