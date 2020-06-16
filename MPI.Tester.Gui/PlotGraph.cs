using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using ZedGraph;

namespace MPI.Tester.Gui
{
    public class PlotGraph
    {
		private static PointPairList pplist = new PointPairList();
		
        public static void SetLabel(ZedGraphControl z, string title, string xName, string yName, float fontSpecSize)
        {
            z.GraphPane.XAxis.Title.IsVisible = true;
            z.GraphPane.Title.Text = title;
            z.GraphPane.XAxis.Title.Text = xName;
            z.GraphPane.YAxis.Title.Text = yName;
            z.GraphPane.XAxis.Title.FontSpec.Size = fontSpecSize;
            z.GraphPane.YAxis.Title.FontSpec.Size = fontSpecSize;
            z.GraphPane.Title.FontSpec.Size = fontSpecSize + 2;
        }

        // Multi-YAxis, Roy Huang, 20150805
        public static void SetLabel(ZedGraphControl z, string title, string xName, string[] yNames, float fontSpecSize)
        {
            if (yNames == null || yNames.Length == 0)
            {
                return;
            }

            z.GraphPane.YAxisList.Clear();

            z.GraphPane.XAxis.Title.IsVisible = true;
            z.GraphPane.Title.Text = title;
            z.GraphPane.XAxis.Title.Text = xName;

            for (int i = 0; i < yNames.Length; i++)
            {
                z.GraphPane.AddYAxis(yNames[i]);
            }

            z.GraphPane.XAxis.Title.FontSpec.Size = fontSpecSize;
            z.GraphPane.YAxis.Title.FontSpec.Size = fontSpecSize;
            z.GraphPane.Title.FontSpec.Size = fontSpecSize + 2;
        }

        public static void SetLabel(ZedGraphControl z, string title, string xName, string[] yNames, string[] y2Names, Color[] yFontColors, Color[] y2FontColors, float fontSpecSize)
        {
            if (yNames == null || yNames.Length == 0)
            {
                return;
            }

            bool isEnableY2Axis = true;

            if (y2Names == null || y2Names.Length == 0)
            {
                isEnableY2Axis = false;
            }

            z.GraphPane.XAxis.Title.IsVisible = true;
            z.GraphPane.Title.Text = title;
            z.GraphPane.XAxis.Title.Text = xName;
       
            z.GraphPane.YAxisList.Clear();

            for (int i = 0; i < yNames.Length; i++)
            {
                z.GraphPane.AddYAxis(yNames[i]);
                z.GraphPane.YAxisList[i].Title.FontSpec.FontColor = yFontColors[i];
            }

            if (isEnableY2Axis)
            {
                z.GraphPane.Y2AxisList.Clear();

                for (int i = 0; i < y2Names.Length; i++)
                {
                    z.GraphPane.AddY2Axis(y2Names[i]);
                    z.GraphPane.Y2AxisList[i].Title.FontSpec.FontColor = y2FontColors[i];
                    z.GraphPane.Y2AxisList[i].IsVisible = true;
                }
            }
            else
            {
                z.GraphPane.Y2Axis.IsVisible = false;
            }

            z.GraphPane.XAxis.Title.FontSpec.Size = fontSpecSize;
            z.GraphPane.YAxis.Title.FontSpec.Size = fontSpecSize;
            z.GraphPane.Title.FontSpec.Size = fontSpecSize + 2;          
        }

        public static void SetGrid(ZedGraphControl z, bool isDashGrid, Color gridColor, Color backColor)
        {
            z.GraphPane.Chart.Fill.Color = backColor;
            z.GraphPane.Chart.Fill.Type = FillType.Brush;
            z.GraphPane.YAxis.MajorGrid.IsVisible = true;
            z.GraphPane.YAxis.MinorGrid.Color = gridColor;
            z.GraphPane.XAxis.MajorGrid.IsVisible = true;
            z.GraphPane.XAxis.MajorGrid.Color = gridColor; //X 軸 虛線 顏色

            if (isDashGrid == false)
            {
                z.GraphPane.XAxis.MajorGrid.DashOff = 0.0F;
                z.GraphPane.YAxis.MajorGrid.DashOff = 0.0F;
            }
            else
            {
                z.GraphPane.XAxis.MajorGrid.DashOff = 1.0F;
                z.GraphPane.YAxis.MajorGrid.DashOff = 1.0F;
            }
            z.GraphPane.XAxis.MajorGrid.IsZeroLine = true;
            z.GraphPane.YAxis.MajorGrid.Color = gridColor;//Y 軸 虛線 顏色 軸 虛線 顏色     
        }

        public static void SetXYAxis(ZedGraphControl z, double Xmin, double Xmax, double Ymin, double Ymax)
        {
            if (Xmax != 0)
            {
                z.GraphPane.XAxis.Scale.Max = Xmax;
            }

            if (Xmin != 0)
            {
                z.GraphPane.XAxis.Scale.Min = Xmin;
            }

            if (Ymax != 0)
            {
                z.GraphPane.YAxis.Scale.Max = Ymax;
            }

            if (Ymin != 0)
            {
                z.GraphPane.YAxis.Scale.Min = Ymin;
            }

            z.Refresh();
        }

        public static void SetXYAxis(ZedGraphControl z, double Xmin, double Xmax, double Ymin, double Ymax, double Xgap, double Ygap)
        {
            if (Xmax != 0)
            {
                z.GraphPane.XAxis.Scale.Max = Xmax;
            }

            if (Xmin != 0)
            {
                z.GraphPane.XAxis.Scale.Min = Xmin;
            }

            if (Ymax != 0)
            {
                z.GraphPane.YAxis.Scale.Max = Ymax;
            }

            if (Ymin != 0)
            {
                z.GraphPane.YAxis.Scale.Min = Ymin;
            }

            if (Xgap != 0)
            {
                z.GraphPane.XAxis.AxisGap = (float)Xgap;
            }

            if (Ygap != 0)
            {
                z.GraphPane.YAxis.AxisGap = (float)Ygap;
            }

            z.Refresh();
        }

        /// <summary>
        ///  Draw Plot
        /// </summary>
        /// <param name="z">Draw</param>
        /// <param name="xData"></param>
        /// <param name="yData"></param>
        /// <param name="isCurve">Line Show</param>
        /// <param name="lineWidth">Line Width 1.5F</param>
        /// <param name="color">Color.X</param>
        /// <param name="symbol">Point Type</param>
        /// <param name="IsShowPointsValue"></param>
        /// <param name="IsShowLegend">Data Legend</param>
        /// <param name="legendName"></param>
        /// <returns></returns>
        public static bool DrawPlot(ZedGraphControl z,
                                                         double[] xData,
                                                         double[] yData,
                                                         bool isCurve,
                                                         float lineWidth,
                                                         Color color,
                                                         SymbolType symbol,
                                                         bool IsFillPoint, bool IsShowPointsValue,
                                                         bool IsShowLegend, string legendName, int Yindex = 0, bool xlogMode = false, bool ylogMode = false)
        {
            if (xData == null || yData == null)
            {
                return false;
            }

            z.GraphPane.Legend.IsVisible = IsShowLegend;
            z.GraphPane.Title.FontSpec.FontColor = Color.DarkBlue;

            PointPairList list2 = new PointPairList();

            List<double> yList = new List<double>();

            yList.AddRange(yData);
            if (ylogMode && yList.Count > 0)
            {
                for (int i = 0; i < yList.Count; ++i)
                {
                    yList[i] = Math.Abs(yList[i]);
                }
            }

            List<double> xList = new List<double>();
            xList.AddRange(xData);
            if (xlogMode && xList.Count > 0)
            {
                for (int i = 0; i < xList.Count; ++i)
                {
                    xList[i] = Math.Abs(xList[i]);
                }
            }

            for (int i = 0; i < xData.Length; i++)
            {
                list2.Add(xList[i], yList[i]);
            }

            LineItem myCurve2;

            myCurve2 = z.GraphPane.AddCurve(legendName, list2, color, symbol);
            
            myCurve2.YAxisIndex = Yindex;
            myCurve2.Line.Width = lineWidth;
            myCurve2.Line.IsVisible = isCurve;
            myCurve2.Symbol.Fill = new Fill(color);
            myCurve2.Symbol.Fill.IsVisible = IsFillPoint;

            z.GraphPane.XAxis.Type = xlogMode ? AxisType.Log : AxisType.Linear;
            z.GraphPane.YAxis.Type = ylogMode ? AxisType.Log : AxisType.Linear;

            z.AxisChange();
            z.Refresh();
            z.IsShowPointValues = IsShowPointsValue;
            z.IsShowCursorValues = false;
            z.IsAutoScrollRange = true;
            return true;
        }

        public static bool DrawDeputyPlot(ZedGraphControl z,
                                                 double[] xData,
                                                 double[] yData,
                                                 bool isCurve,
                                                 float lineWidth,
                                                 Color color,
                                                 SymbolType symbol,
                                                 bool IsFillPoint, bool IsShowPointsValue,
                                                 bool IsShowLegend, string legendName, int Y2index = 0)
        {
            if (xData == null)
            {
                return false;
            }

            z.GraphPane.Legend.IsVisible = IsShowLegend;
            z.GraphPane.Title.FontSpec.FontColor = Color.DarkBlue;

            PointPairList list2 = new PointPairList();

            for (int i = 0; i < xData.Length; i++)
            {
                list2.Add(xData[i], yData[i]);
            }

            LineItem myCurve2;

            myCurve2 = z.GraphPane.AddCurve(legendName, list2, color, symbol);

            myCurve2.YAxisIndex = Y2index;
            myCurve2.IsY2Axis = true;

            myCurve2.Line.Width = lineWidth;
            myCurve2.Line.IsVisible = isCurve;
            myCurve2.Symbol.Fill = new Fill(color);
            myCurve2.Symbol.Fill.IsVisible = IsFillPoint;
            z.AxisChange();
            z.Refresh();
            z.IsShowPointValues = IsShowPointsValue;
            z.IsShowCursorValues = false;
            z.IsAutoScrollRange = true;
            return true;
        }

        public static bool DrawPlot(ZedGraphControl z,
                                                        string[] title,
                                                        double[] xData,
                                                        double[] yData,
                                                        bool isCurve,
                                                        float lineWidth,
                                                        Color color,
                                                        SymbolType symbol,
                                                        bool IsFillPoint, bool IsShowPointsValue,
                                                        bool IsShowLegend, string legendName)
        {
            if (xData == null)
            {
                return false;
            }

            z.GraphPane.Legend.IsVisible = IsShowLegend;
            z.GraphPane.Title.FontSpec.FontColor = Color.DarkBlue;

            PointPairList list2 = new PointPairList();

            for (int i = 0; i < xData.Length; i++)
            {
                list2.Add(xData[i], yData[i]);
            }

            LineItem myCurve2;

            myCurve2 = z.GraphPane.AddCurve(legendName, list2, color, symbol);
            myCurve2.Line.Width = lineWidth;
            myCurve2.Line.IsVisible = isCurve;
            myCurve2.Symbol.Fill = new Fill(color);
            myCurve2.Symbol.Fill.IsVisible = IsFillPoint;
            z.GraphPane.XAxis.Scale.TextLabels = title;
            z.GraphPane.XAxis.Type = AxisType.Text;
            z.AxisChange();
            z.Refresh();
            z.IsShowPointValues = IsShowPointsValue;
            z.IsShowCursorValues = false;
            z.IsAutoScrollRange = true;
            return true;
        }


        public static bool DrawPlotRealTimeUpdate(ZedGraphControl z,
                                                      double[] xData,
                                                      double[] yData,
                                                      bool isCurve,
                                                      float lineWidth,
                                                      Color color,
                                                      SymbolType symbol,
                                                      bool IsFillPoint, bool IsShowPointsValue,
                                                      bool IsShowLegend, string legendName)
        {
            if (xData == null)
            {
                return false;
            }

            z.GraphPane.Legend.IsVisible = IsShowLegend;
            z.GraphPane.Title.FontSpec.FontColor = Color.DarkBlue;

            // PointPairList list2 = new PointPairList();
            if (z.GraphPane.CurveList.Count != 0)
            {
                z.GraphPane.CurveList.Clear();
            }

            pplist.Clear();

            for (int i = 0; i < xData.Length; i++)
            {
                pplist.Add(xData[i], yData[i]);
            }

            LineItem myCurve2;

            myCurve2 = z.GraphPane.AddCurve(legendName, pplist, color, symbol);
            myCurve2.Line.Width = lineWidth;
            myCurve2.Line.IsVisible = isCurve;
            myCurve2.Symbol.Fill = new Fill(color);
            myCurve2.Symbol.Fill.IsVisible = IsFillPoint;
            z.AxisChange();
            z.Refresh();
            z.IsShowPointValues = IsShowPointsValue;
            z.IsShowCursorValues = false;
            z.IsAutoScrollRange = true;
            return true;
        }



		 public static bool DrawPlot(ZedGraphControl z, double[] yData,
															 bool isCurve, float lineWidth,
															 Color color, SymbolType symbol, bool IsShowPointsValue,
															 bool IsShowLegend, string legendName)
		 {
			 if (yData == null)
			 {
				 return false;
			 }

			 double[] xData=new double[yData.Length];

			 z.GraphPane.Legend.IsVisible = IsShowLegend;
			 z.GraphPane.Title.FontSpec.FontColor = Color.DarkBlue;

			 //PointPairList list2 = new PointPairList();
			 if (z.GraphPane.CurveList.Count != 0)
			 {
				 z.GraphPane.CurveList.Clear();
			 }

             pplist.Clear();
			 for (int i = 0; i < yData.Length; i++)
			 {
				 xData[i] = i;
                 pplist.Add(xData[i], yData[i]);
			 }

			 LineItem myCurve2;

             myCurve2 = z.GraphPane.AddCurve(legendName, pplist, color, symbol);
			 myCurve2.Line.Width = lineWidth;
			 myCurve2.Line.IsVisible = isCurve;
			 myCurve2.Symbol.Fill = new Fill(color);
			 z.AxisChange();
			 z.Refresh();
			 z.IsShowPointValues = IsShowPointsValue;
			 z.IsShowCursorValues = false;
			 z.IsAutoScrollRange = true;
			 return true;
		 }

         public static bool DrawBarItem(ZedGraphControl z,
                                                    double[] xData,
                                                    double[] yData,
                                                    Color color)
         {
             if (xData == null)
             {
                 return false;
             }

             z.GraphPane.Title.FontSpec.FontColor = Color.DarkBlue;

             PointPairList list2 = new PointPairList();
             for (int i = 0; i < xData.Length; i++)
             {
                 list2.Add(xData[i], yData[i]);
             }

             BarItem myCurve2;
             myCurve2 = z.GraphPane.AddBar("", list2, color);
             z.AxisChange();
             z.Refresh();
             z.IsShowCursorValues = false;
             z.IsAutoScrollRange = true;
             z.IsShowPointValues = true;
             return true;
         }

         public static void Clear(ZedGraphControl z)
         {
             z.GraphPane.CurveList.Clear();
             z.Refresh();
         }

         public static bool IsNeedLogMode(double[] valArr, double limitVal)
         {
             if (valArr != null && valArr.Length > 1)
            {
                double Ymin = valArr.Min();
                double Ymax = valArr.Max();
                if (Ymin != 0 && Ymax != 0 && Math.Sign(Ymax) == Math.Sign(Ymin))
                {
                    double aMax = Math.Max(Math.Abs(Ymax), Math.Abs(Ymin));
                    double aMin = Math.Min(Math.Abs(Ymax), Math.Abs(Ymin));
                    if (aMax / aMin > limitVal)
                    {
                        return true;
                    }
                }
            }
             return false;
         }


    }
}
