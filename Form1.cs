using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DSP2
{
    public partial class Form1 : Form
    {
        private int N = 0;
        private int M = 0;

        private double fi = 0;

        private List<double> X = new List<double>();

        private List<double> xSkc1 = new List<double>();
        private List<double> xSkc2 = new List<double>();
        public List<double> A = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        private void HarmonikFunction()
        {
            double K = 3 * N / 4;

            // double dM = 2 * N - K;

            Random rand = new Random();

            M = rand.Next((int)K, (int)(2 * N));

            double step = (double)M / N;

            int cycle = 0;

            //  double n = 0;

            for (double i = 0; i < M; i++)
            {
                if (cycle == M / 8)
                {
                    double x1 = new double();
                    x1 = Math.Sin((2 * Math.PI * (M - 1) / N) + fi);
                    X.Add(x1);
                    cycle = 0;
                    continue;
                }

                double x = new double();

                x = Math.Sin((2 * Math.PI * i / N) + fi);


                X.Add(x);
                cycle++;

                double xskc12 = new double();
                double xskc22 = new double();
                xskc12 = Math.Sqrt(Math.Abs(SKC1()));
                xskc22 = SKC2();

                xSkc1.Add(xskc12);
                xSkc2.Add(xskc22);

            }
            AFunction();

            //double xskc12 = SKC1();
            //double xskc22 = SKC2();
            //double a2 = AFunction();

            XSKC1.Text = Math.Abs(DeltaX(xSkc1[xSkc1.Count - 1])).ToString();
            XSKC2.Text = Math.Abs(DeltaX(xSkc2[xSkc2.Count - 1])).ToString();
            Amlit.Text = Math.Abs(DeltaA(A[A.Count - 1])).ToString();

            Draw();
        }

        private double SKC1()
        {
            double res = 0;

            double x = 0;

            for (int i = 0; i < X.Count; i++)
            {
                x += Math.Pow(X[i], 2);
            }

            res = 1 / (1 + (double)M);

            res = res * x;

            //  double res1 = Math.Sqrt(res);

            // XSKC1.Text = res1.ToString();

            return res;
        }

        private double SKC2()
        {

            double x = 0;

            for (int i = 0; i < X.Count; i++)
            {
                x = x + X[i];
            }

            x = (1 / (1 + (double)M)) * x;

            x = Math.Pow(x, 2);

            x = SKC1() - x;

            x = Math.Sqrt(Math.Abs(x));

            //  XSKC2.Text = x.ToString();

            return x;
        }


        private double DeltaX(double x)
        {
            double res = 0;

            res = 0.707 - x;

            return res;
        }

        private double DeltaA(double a)
        {
            double res = 0;

            res = 1 - a;

            return res;
        }

        private void AFunction()
        {

            double Res = 0;
            double Img = 0;
            double alf = 0;

            for (int i = 0; i < M; i++)
            {
                double sum = new double();
                double sumRes = 0, sumImg = 0;

                for (int j = 0; j < X.Count; j++)
                {
                    alf = -2 * Math.PI * i * j / X.Count;
                    Res = Math.Cos(alf);
                    Img = Math.Sin(alf);

                    Res = Res * X[j];
                    Img = Img * X[j];

                    sumRes += Res;
                    sumImg += Img;

                }

                sumRes = sumRes / M;
                sumImg = sumImg / M;

                sum = Math.Sqrt(Math.Pow(sumRes, 2) + Math.Pow(sumImg, 2));
                A.Add(sum);
            }

        }

        
        private void DeltaFuction()
        {
            List<double> Afun = new List<double>();
            List<double> Bfun = new List<double>();
            List<double> X1fun = new List<double>();
            List<double> X2fun = new List<double>();


            for (int newM = 1; newM < M; newM++)
            {
                double Res = 0;
                double Img = 0;
                double alf = 0;

                for (int i = 0; i < newM; i++)
                {
                    double sum = new double();
                    double sumRes = 0, sumImg = 0;

                    for (int j = 0; j < X.Count; j++)
                    {
                        alf = -2 * Math.PI * i * j / X.Count;
                        Res = Math.Cos(alf);
                        Img = Math.Sin(alf);

                        Res = Res * X[j];
                        Img = Img * X[j];

                        sumRes += Res;
                        sumImg += Img;

                    }

                    sumRes = sumRes / M;
                    sumImg = sumImg / M;

                    sum = Math.Sqrt(Math.Pow(sumRes, 2) + Math.Pow(sumImg, 2));
                    Afun.Add(sum);
                }

                double sum1 = new double();

                for(int i = 0; i < Afun.Count; i++)
                {
                    sum1 += Afun[i];
                }

                sum1 = sum1 / Afun.Count;

                Bfun.Add(sum1);
                Afun.Clear();


                double x = 0;
                double x2 = 0;

                for (int i = 0; i < newM; i++)
                {
                    x += Math.Pow(X[i], 2);
                    x2 += X[i];
                }


                double res = new double();

                res = 1 / (1 + (double)newM);

                res = res * x;

                X1fun.Add(Math.Sqrt(Math.Abs(res)));


                double res2 = new double();

                res2 = (1 / (1 + (double)newM)) * x2;

                res2 = Math.Pow(res2, 2);

                res2 = Math.Pow(X1fun[newM-1],2) - res2;

                res2 = Math.Sqrt(Math.Abs(res2));

                X2fun.Add(res2);

            }




            var series1 = new Series
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "ChartArea",
                Name = "XSKC1",
            };

            var series2 = new Series
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "ChartArea",
                Name = "XSKC2",
            };

            var series3 = new Series
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "ChartArea",
                Name = "A",
            };

            var legind = new Legend
            {
                Docking = Docking.Bottom,
            };


            for (int i = 0; i < X1fun.Count; i++)
            {
                series1.Points.AddXY(i, 0.707 - X1fun[i]);
                series2.Points.AddXY(i, 0.707 - X2fun[i]);
            }

            for (int i = 0; i < Bfun.Count; i++)
            {
                series3.Points.AddXY(i, 1 - Bfun[i]);
            }

            
            Chart.Series.Add(series1);
            Chart.Series.Add(series2);
            Chart.Series.Add(series3);

            Chart.Legends.Add(legind);

        }
        
 
        private void Draw()
        {
            var series = new Series
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "ChartArea",
                Name = "X",
            };

            var legind = new Legend
            {
                Docking = Docking.Bottom,
            };


            for (int i = 0; i < X.Count; i++)
            {

                double y = X[i] ;

                series.Points.AddXY(i, y);
            }



            Chart.Series.Add(series);

            Chart.Legends.Add(legind);
        }

        private void DrawDely()
        {
            var series1 = new Series
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "ChartArea",
                Name = "XSKC1",
            };

            var series2 = new Series
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "ChartArea",
                Name = "XSKC2",
            };

            var series3 = new Series
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "ChartArea",
                Name = "A",
            };

            var legind = new Legend
            {
                Docking = Docking.Bottom,
            };


            for(int i=0; i<xSkc1.Count; i++)
            {
                series1.Points.AddXY(i, xSkc1[i]);
                series2.Points.AddXY(i, xSkc2[i]);
            }

            for (int i = 0; i < A.Count; i++)
            {
                series3.Points.AddXY(i, A[i]);
            }


            Chart.Series.Add(series1);
            Chart.Series.Add(series2);
            Chart.Series.Add(series3);

            Chart.Legends.Add(legind);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                xSkc1.Clear();
                xSkc2.Clear();
                A.Clear();
                X.Clear();
                Chart.Series[0].Points.Clear();
                Chart.Legends.Clear();
                Chart.Series.Clear();
            }
            catch { }

            try
            {
                Chart.Series[1].Points.Clear();
                Chart.Series[2].Points.Clear();
                Chart.Series[3].Points.Clear();
            }
            catch { }

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    N = 64;
                    break;
                case 1:
                    N = 128;
                    break;
                case 2:
                    N = 256;
                    break;
                case 3:
                    N = 512;
                    break;
                case 4:
                    N = 1024;
                    break;
                case 5:
                    N = 2048;
                    break;
                default:
                    break;
            }

            switch (comboBox2.SelectedIndex)
            {

                case 0:
                    fi = 0;
                    HarmonikFunction();
                    break;
                case 1:
                    
                    fi = Math.PI / 8;
                    HarmonikFunction();
                    break;
                default:
                    break;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Chart.Legends.Clear();
                Chart.Series[0].Points.Clear();
                Chart.Series[1].Points.Clear();
                Chart.Series[2].Points.Clear();
                Chart.Series[3].Points.Clear();
                Chart.Series.Clear();
            }
            catch { }
            Chart.Series.Clear();
            DrawDely();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Chart.Legends.Clear();
                Chart.Series[0].Points.Clear();
                Chart.Series[1].Points.Clear();
                Chart.Series[2].Points.Clear();
                Chart.Series[3].Points.Clear();

                Chart.Series.Clear();
            }
            catch { }
            Chart.Series.Clear();
            DeltaFuction();
        }
    }
}
