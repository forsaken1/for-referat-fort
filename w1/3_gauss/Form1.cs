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

        private void ActivateButtons()
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
            ActivateButtons();
            mode = true;
        }

        private void IntegralButton_Click(object sender, EventArgs e)
        {
            ActivateButtons();
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
                        sample = new Integral(sr, monitor, result);
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

        public void Write(string str)
        {
            using (StreamWriter sw = new StreamWriter(File.Open("output.txt", FileMode.Create)))
            {
                sw.Write(str);
            }
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
            Write(str);
        }
    }

    public class Result
    {
        public double acc;
        public string rest;

        public Result(double v, string r)
        {
            this.acc = v;
            this.rest = r;
        }
    }

    public class Parser
    {
        private Dictionary<string, double> variables;

        public Parser()
        {
            variables = new Dictionary<string, double>();
        }

        public void SetVariable(string variableName, double variableValue)
        {
            variables.Add(variableName, variableValue);
        }

        public double GetVariable(string variableName)
        {
            if (!variables.ContainsKey(variableName))
            {
                return 0.0;
            }
            double value;
            variables.TryGetValue(variableName, out value);
            return value;
        }

        public double Parse(string s)
        {
            Result result = PlusMinus(s);
            return result.acc;
        }

        private Result PlusMinus(string s)
        {
            Result current = MulDiv(s);
            double acc = current.acc;

            while (current.rest.Length > 0)
            {
                if (!(current.rest[0] == '+' || current.rest[0] == '-')) break;

                char sign = current.rest[0];
                string next = current.rest.Substring(1);

                current = MulDiv(next);
                if (sign == '+')
                {
                    acc += current.acc;
                }
                else
                {
                    acc -= current.acc;
                }
            }
            return new Result(acc, current.rest);
        }

        private Result Bracket(string s)
        {
            char zeroChar = s[0];
            if (zeroChar == '(')
            {
                Result r = PlusMinus(s.Substring(1));
                if (!(r.rest.Length > 0) && r.rest[0] == ')')
                    r.rest = r.rest.Substring(1);

                return r;
            }
            return FunctionVariable(s);
        }

        private Result FunctionVariable(String s)
        {
            String f = "";
            int i = 0;
            while (i < s.Length && (char.IsLetter(s[i]) || (char.IsDigit(s[i]) && i > 0)))
            {
                f += s[i];
                i++;
            }
            if (f.Length > 0)
            {
                if (s.Length > i && s[i] == '(')
                {
                    Result r = Bracket(s.Substring(f.Length));
                    return processFunction(f, r);
                }
                else
                {
                    return new Result(GetVariable(f), s.Substring(f.Length));
                }
            }
            return Num(s);
        }

        private Result MulDiv(String s)
        {
            Result current = Bracket(s);

            double acc = current.acc;
            while (true)
            {
                if (current.rest.Length == 0)
                    return current;

                char sign = current.rest[0];
                if ((sign != '*' && sign != '/'))
                    return current;

                string next = current.rest.Substring(1);
                Result right = Bracket(next);

                if (sign == '*')
                    acc *= right.acc;
                else
                    acc /= right.acc;

                current = new Result(acc, right.rest);
            }
        }

        private Result Num(string s)
        {
            int i = 0;
            bool negative = false;
            if (s[0] == '-')
            {
                negative = true;
                s = s.Substring(1);
            }
            while (i < s.Length && (char.IsDigit(s[i]) || s[i] == '.'))
            {
                i++;
            }

            double dPart = double.Parse(s.Substring(0, i));
            if (negative) dPart = -dPart;
            string restPart = s.Substring(i);

            return new Result(dPart, restPart);
        }

        private Result processFunction(String func, Result r)
        {
            if (func == "sin")
                return new Result(Math.Sin(r.acc / 180 * Math.PI), r.rest);
            else
                if (func == "cos")
                    return new Result(Math.Cos(r.acc / 180 * Math.PI), r.rest);
                else
                    if (func == "tan")
                        return new Result(Math.Tan(r.acc / 180 * Math.PI), r.rest);
            return r;
        }
    }

    public class Integral : Task
    {
        private string exp, strResult;
        private double a, b;
        private StreamReader sr;
        private RichTextBox mon, res;

        public Integral(StreamReader _sr, RichTextBox _mon, RichTextBox _res)
        {
            sr = _sr;
            res = _res;
            mon = _mon;
            Read();
        }

        public override void Read()
        {
            string str = sr.ReadLine();
            string[] s = str.Split(' ');
            a = double.Parse(s[0]);
            b = double.Parse(s[1]);
            exp = s[2];
            mon.AppendText(str);
        }

        public override void Write()
        {
            using (StreamWriter sw = new StreamWriter(File.Open("output.txt", FileMode.Create)))
            {
                sw.Write(strResult);
            }
        }

        public override void Solve()
        {
            Parser parser = new Parser();
            parser.SetVariable("x", a);
            double d1 = parser.Parse(exp);

            parser = new Parser();
            parser.SetVariable("x", b);
            double d2 = parser.Parse(exp);

            strResult = "Integral = " + (d1 + d2) / 2 * (b - a); 
        }

        public override void ShowData()
        {
            Solve();
            res.Clear();
            res.AppendText("" + strResult);
            Write();
        }
    }
}
