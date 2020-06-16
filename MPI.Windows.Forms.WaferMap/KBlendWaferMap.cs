using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using MPI.UCF.Forms.Domain;

namespace MPI.Windows.Forms
{
    public enum EGrowthDirection
    {
        None = -1,
        Downward = 0,
        Upward = 1,
        Rightward = 2,
        Leftward = 3
    }

    public class KBlendWaferMap : MPI.UCF.Forms.Domain.GenericGridMap
    {
        int _lastRow, _lastCol;

        internal class BinGradeRender : GradeRenderBase
        {
            internal WaferDatabase WaferDB;
            public string SymbolId;

            public override Color GetGradeColor(float value)
            {
                BinGradeColor bgc = BinGradeColorSet.GetColorItem(SymbolId);
                if (bgc == null)
                    return Color.Empty;

                return bgc.GetColor(value);
            }

            public override EGradeColorMethod GradeMethod
            {
                get
                {
                    return EGradeColorMethod.Customized;
                }
                set
                {

                }
            }

            public override void OwnerDraw(Graphics g, Point ptDraw, int cellSize, int row, int column)
            {

            }

            public override Color GetGradeColor(int row, int column)
            {
                float value = WaferDB[row, column, SymbolId];
                if (float.IsNaN(value))
                    return Color.Empty;

                return this.GetGradeColor(value);
            }
        }
        #region >>> Private Field <<<

        private int fLabelMinWidth;

        private BinGradeRender _render;
        private IWaferDb _wfdb;
        #endregion

        public KBlendWaferMap()
        {
            _render = new BinGradeRender();
            this.SetGradeRender(_render);
        }

        #region >>> Public Property / Event <<<

        public string SymbolId
        {
            get
            {
                return _render.SymbolId;
            }

            set
            {
                _render.SymbolId = value;
            }
        }

        public bool ScrollBarEnabled
        {
            get;
            set;
        }

        public bool AutoBoundary
        {
            get;
            set;
        }

        public bool UseAutoScale
        {
            get;
            set;
        }

        public bool Redraw
        {
            get;
            set;
        }

        public double MinScale
        {
            get;
            set;
        }

        public double MaxScale
        {
            get;
            set;
        }

        public Color EraseDieColor
        {
            get;
            set;
        }

        public bool FocusBoxEnabled
        {
            get
            {
                return base.FocusBox;
            }
            set
            {
                base.FocusBox = value;
            }
        }

        public bool SelectWindowEnabled
        {
            get
            {
                return base.Selectable;
            }

            set
            {
                base.Selectable = value;
            }
        }

        public bool Seamless
        {
            get
            {
                return (base.CellGap == 0);
            }

            set
            {
                base.CellGap = (value == true) ? 0 : 1;
            }
        }

        public bool IsSeamless
        {
            get;
            set;
        }

        public EGrowthDirection GrowthDirection
        {
            get;
            set;
        }

        public bool DynamicMode
        {
            get;
            set;
        }

        public EGradeColorMethod GradeMethod
        {
            get;
            set;
        }

        public bool BlendBar
        {
            get;
            set;
        }

        public EDieStatus InvalidStatus
        {
            get;
            set;
        }

        public Color MaxLevelColor
        {
            get
            {
                return _render.MaxLevelColor;
            }
            set
            {
                _render.MaxLevelColor = value;
            }
        }

        public float MaxLevelValue
        {
            get
            {
                return _render.MaxLevelValue;
            }
            set
            {
                _render.MaxLevelValue = value;
            }
        }

        public Color MinLevelColor
        {
            get
            {
                return _render.MinLevelColor;
            }
            set
            {
                _render.MinLevelColor = value;
            }
        }

        public float MinLevelValue
        {
            get
            {
                return _render.MinLevelValue;
            }

            set
            {
                _render.MinLevelValue = value;
            }
        }

        public bool ScaledChip
        {
            get;
            set;
        }
        #endregion

        #region >>> Private Method <<<

        #endregion

        #region >>> Protected / Overrided <<<

        #endregion

        #region >>> Public Method <<<

        public void SetDatabase(WaferDatabase db)
        {
            _wfdb = db;
            _render.WaferDB = db;
        }

        public void Start(bool dumy)
        {
            this.Seamless = true;
			this.ScaleToContent = true;
			this.Draw();
        }

        public void Stop()
        {

        }

        public void Clear()
        {
            this.Draw();
        }

        public void AddWaferDie(int row, int col, int index, Dictionary<string, float> values)
        {
            values.Add("_INDEX_", index);
            _wfdb.AddItem(row, col, new FieldValue(values));
            _lastRow = row;
            _lastCol = col;
        }

        public void DrawAllWaferDie()
        {
			this.ResetScale();
        }

        public void DrawWaferDie()
        {
            //this.RedrawOne( _lastRow, _lastCol );
            this.DrawOne(_lastRow, _lastCol);
        }

        public void DrawWaferDie(int row, int col)
        {
            //this.RedrawOne( row, col );
            this.DrawOne(row, col);
        }

        public void Close()
        {
            this.Dispose();
        }

		//public void ResetScale()
		//{
		//    this.AutoScale();
		//}

        #endregion

    }
}
