using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace MPI.Tester.Gui.UIForm
{
    public partial class frmBoxPlot : Form
    {
        public frmBoxPlot()
        {
            InitializeComponent();
        }

        public List<double[]> _Data = new List<double[]>();

        public List<string> _Names = new List<string>();

       // private Color[] colorArray = new Color[8] { Color.Red, Color.Gold, Color.Green, Color.Blue, Color.DeepPink, Color.DarkOrange,Color.SpringGreen,Color.SlateBlue };


        private Color[] colorArray = new Color[8] { Color.Red, Color.Red, Color.Green, Color.Green, Color.Blue, Color.Blue, Color.DarkOrange, Color.DarkOrange };

        private void frmWaferMap_Load(object sender, EventArgs e)
        {
         

        }

        private void BoxPlot(List<double[]> data, List<string> names)
        {
            zedGraphControl1.GraphPane.CurveList.Clear();

            zedGraphControl1.IsShowPointValues = true;

            GraphPane myPane = zedGraphControl1.GraphPane;

            myPane.XAxis.Scale.Min = 0.5;

            myPane.XAxis.Scale.Max = names.Count + 0.5;

            for (int i = 0; i < data.Count; i++)
            {
                int iSeed = 10;
                Random ro = new Random(10);
                long tick = DateTime.Now.Ticks;
                Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
                int R = ran.Next(255);
                int G = ran.Next(255);
                int B = ran.Next(255);
                B = (R + G > 400) ? R + G - 400 : B;//0 : 380 - R - G;
                B = (B > 255) ? 255 : B;

                Color color = Color.FromArgb(R, G, B);

                if (i < this.colorArray.Length)
                {
                    color = colorArray[i];
                }

                if (data[i].Length == 0)
                {
                    return;
                }


                //median of each array
                PointPairList medians = new PointPairList();
                //median of each array
                PointPairList averages = new PointPairList();


                //75th and 25th percentile, defines the box
                PointPairList hiLowList = new PointPairList();
                //+/- 1.5*Interquartile range, extentent of wiskers
                PointPairList barList = new PointPairList();
                //outliers
                PointPairList outs = new PointPairList();
                //Add the values

                double mediansValue = percentile(data[i], 50);

                double quater = percentile(data[i], 25);

                double ThreeQuater = percentile(data[i], 75);

                medians.Add(i + 1, mediansValue);

                averages.Add(i + 1, MPI.Maths.Statistic.Average(data[i]));

                hiLowList.Add(i + 1, percentile(data[i], 75), percentile(data[i], 25));
                double iqr = 1.5 * (percentile(data[i], 75) - percentile(data[i], 25));
                double upperLimit = percentile(data[i], 75) + iqr;
                double lowerLimit = percentile(data[i], 25) - iqr;
                //The wiskers must end on an actual data point
                barList.Add(i + 1, ValueNearestButGreater(data[i], lowerLimit), ValueNearestButLess(data[i], upperLimit));
                //Sort out the outliers
                foreach (double aValue in data[i])
                {
                    if (aValue > upperLimit)
                    {
                        outs.Add(i+1, aValue);
                    }
                    if (aValue < lowerLimit)
                    {
                        outs.Add(i + 1, aValue);
                    }
                }

                //dataGridView1.Rows.Add();

                //dataGridView1[0, i].Value = names[i];
                //dataGridView1[1, i].Value = data[i].Length.ToString();

                //dataGridView1[2, i].Value = mediansValue.ToString();

                //dataGridView1[3, i].Value = quater.ToString();
                //dataGridView1[4, i].Value = ThreeQuater.ToString();
                //dataGridView1[5, i].Value = MPI.Maths.Statistic.Average(data[i]);
                //dataGridView1[6, i].Value = MPI.Maths.Statistic.StandardDeviation(data[i]);

                //Plot the items, first the median values

                //BarItem bar = myPane.AddBar(names[i], medians, color);

                LineItem averagecureve = myPane.AddCurve("", averages, color, SymbolType.Square);

                averagecureve.Symbol.Fill = new Fill(color); 

                CurveItem meadian = myPane.AddCurve("", medians, color, SymbolType.Plus);
                LineItem myLine = (LineItem)meadian;
                myLine.Line.IsVisible = false;
                myLine.Symbol.Fill.Type = FillType.Solid;

 


                names[i] = "[" + (i + 1).ToString() + "]" + names[i];

                //Wiskers
                ErrorBarItem myerror = myPane.AddErrorBar(names[i], barList, color);
                myerror.Color = color;

                //Outliers
                CurveItem upper = myPane.AddCurve("", outs, color, SymbolType.XCross);

                LineItem bLine = (LineItem)upper;
                bLine.Color = color;
                bLine.Symbol.Size = 3;
                bLine.Line.IsVisible = false;

                //Box
                HiLowBarItem myCurve = myPane.AddHiLowBar("", hiLowList, color);
                myCurve.Bar.Fill.Color = color;

                myCurve.Color = color;
                myCurve.Bar.Fill.Type = FillType.None;
            }

            //myPane.XAxis.Scale.TextLabels = names.ToArray();

            //myPane.XAxis.Type = AxisType.Text;

        }

        private double ValueNearestButLess(double[] data, double number)
        {
            double lowNums = double.MinValue;
            foreach (double n in data)
            {
                if (n <= number)
                {
                    lowNums = Math.Max(n, lowNums);
                }
            }
            return lowNums;
        }

        private double ValueNearestButGreater(double[] data, double number)
        {
            double lowNums = double.MaxValue;
            foreach (double n in data)
            {
                if (n >= number)
                {
                    lowNums = Math.Min(n, lowNums);
                }
            }
            return lowNums;
        }

        private double percentile(double[] Data, double p)
        {
            //[url]http://www.codeproject.com/KB/recipes/DescriptiveStatisticClass.aspx[/url]
            Array.Sort(Data);
            if (p >= 100.0d) return Data[Data.Length - 1];
            double position = (double)(Data.Length + 1) * p / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;
            double n = p / 100.0d * (Data.Length - 1) + 1.0d;
            if (position >= 1)
            {
                leftNumber = Data[(int)System.Math.Floor(n) - 1];
                rightNumber = Data[(int)System.Math.Floor(n)];
            }
            else
            {
                leftNumber = Data[0]; // first data
                rightNumber = Data[1]; // first data
            }
            if (leftNumber == rightNumber)
                return leftNumber;
            else
            {
                double part = n - System.Math.Floor(n);
                return leftNumber + part * (rightNumber - leftNumber);
            }
        }

        private void frmBoxPlot_Load(object sender, EventArgs e)
        {
            List<double[]> dataList = new List<double[]>();
            List<string> names = new List<string>();

            BoxPlot(_Data, _Names);

            //Set up the chart
            zedGraphControl1.GraphPane.BarSettings.Type = BarType.Overlay;
            zedGraphControl1.GraphPane.XAxis.IsVisible = true;
            zedGraphControl1.GraphPane.YAxis.IsVisible = true;
            zedGraphControl1.GraphPane.Legend.IsVisible = true;
            zedGraphControl1.GraphPane.Title.Text = "Channel Boxplot";
            zedGraphControl1.GraphPane.AxisChange();

            PlotGraph.SetGrid(this.zedGraphControl1, false, Color.Silver, Color.WhiteSmoke);

            PlotGraph.SetLabel(this.zedGraphControl1, "Channel Boxplot","Index","Value",14.0f);

            this.zedGraphControl1.GraphPane.Fill = new Fill(Color.AliceBlue);
        }
    }
}
