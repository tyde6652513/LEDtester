using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using MPI.Drawing;
using MPI.Tester.Tools;

namespace MPI.UCF.Forms.Domain
{
    //TODO: pad section display
    public class GenericGridMap : GridMapBase
    {
        private const int DEFAULT_PADDING = 3; //unit in row/col
        private int _padding;
        private int _cellGap;
        private bool _autoScale;

        #region >>> Constructor / Disposor <<<

        public GenericGridMap()
        {
            _gradeRender = new WebColorRender(null); //dummy
            _padding = DEFAULT_PADDING;
            _cellGap = 1;
            _autoScale = true;
        }

        #endregion

        [Browsable(false)]
        public int CellGap
        {
            get
            {
                return _cellGap;
            }

            set
            {
                if (_cellGap < 0)
                    _cellGap = 0;
                else
                    _cellGap = value;
            }
        }

        #region >>> Protected / Override Method <<<

        protected override bool innerOwnerDraw(Graphics ownerGraphics, Rectangle graphArea, ref TCanvasInfo srcCanvas)
        {
            if (_coordData2UI == null)
                return false;

            graphArea.Offset(_scrollX, _scrollY);
            //_coordData2UI.ToMine(ref graphArea); // change to Row/Column area
            Rectangle graphAreaData = _coordData2UI.I_TransCoord(graphArea); // change to Row/Column area
            graphAreaData.Inflate(1, 1);

            IntPtr hdc = srcCanvas.Hdc;
            IntPtr hObject = GDI32.SelectObject(hdc, GDI32.GetStockObject(EStockObject.DC_BRUSH));
            GDI32.SelectObject(hdc, GDI32.GetStockObject(EStockObject.DC_PEN));

            int row_from = (int)Math.Min(graphAreaData.Top, graphAreaData.Bottom);
            int row_to = (int)Math.Max(graphAreaData.Top, graphAreaData.Bottom);
            int col_from = (int)Math.Min(graphAreaData.Left, graphAreaData.Right);
            int col_to = (int)Math.Max(graphAreaData.Left, graphAreaData.Right);

            int count = 0;
            for (int row = row_from; row <= row_to; row++)
            {
                for (int col = col_from; col <= col_to; col++)
                {
                    float x = col;
                    float y = row;
                    //_coordData2UI.ToWorld(ref x, ref y);
                    _coordData2UI.TransCoord(ref x, ref y);

                    int px = (x > 0) ? (int)Math.Floor(x) : -(int)Math.Ceiling(-x);
                    int py = (y > 0) ? (int)Math.Floor(y) : -(int)Math.Ceiling(-y);

                    px -= _scrollX;
                    py -= _scrollY; // point.Offset( -_scrollX, -_scrollY );

                    if ((px < 0 && py < 0) || (px > srcCanvas.Width && py > srcCanvas.Height))
                        continue;

                    Color color = _gradeRender.GetGradeColor(row, col);
                    if (color.IsEmpty)
                        continue;

                    uint bg = (uint)ColorTranslator.ToWin32(color);
                    if (bg != _backColor)
                    {
                        this.drawCell(hdc, px, py, _cellSize, bg);
                        _gradeRender.OwnerDraw(ownerGraphics, new Point(px, py), _cellSize, row, col);
                        count++;
                    }

                }
            }

            GDI32.SelectObject(hdc, hObject);
            base.Refresh();
            //Draw();
            //ResetScale();
            return true;
        }

        protected override void paintToImage(Bitmap image)
        {
            Graphics graphics = Graphics.FromImage(image);
            IntPtr hdc = graphics.GetHdc();
            IntPtr hObject = GDI32.SelectObject(hdc, GDI32.GetStockObject(EStockObject.DC_BRUSH));
            GDI32.SelectObject(hdc, GDI32.GetStockObject(EStockObject.DC_PEN));

            Rectangle area = _boundary;
            for (int row = area.Top; row <= area.Bottom; row++)
            {
                for (int col = area.Left; col <= area.Right; col++)
                {
                    //Point point = _coordData2UI.ToWorld(col, row);

                    Point point = _coordData2UI.TransCoord2P(col, row);
                    if (point.X > 0 && point.Y > 0)
                    {
                        uint bg = (uint)ColorTranslator.ToWin32(_gradeRender.GetGradeColor(row, col));
                        if (bg != _backColor)
                            base.drawCell(hdc, point.X, point.Y, _cellSize, bg);
                    }
                }
            }

            hObject = GDI32.SelectObject(hdc, hObject);
            graphics.ReleaseHdc();
            graphics.Dispose();
            graphics = null;
        }

        protected override void calcGraphicsSize()
        {
            Size wf_size = _boundary.Size;
            int wf_cols = Math.Abs(wf_size.Width);
            int wf_rows = Math.Abs(wf_size.Height);

            int zoom_width = wf_cols * (_cellSize + _cellGap) + _cellGap;
            int zoom_height = wf_rows * (_cellSize + _cellGap) + _cellGap;
            
            if (_autoScale) // fit to content
            {
                Size display_size = this.ClientSize;
                if (zoom_width < display_size.Width)
                    zoom_width = display_size.Width;

                if (zoom_height < display_size.Height)
                    zoom_height = display_size.Height;
            }

            if (_coordData2UI == null)
            {
                Console.WriteLine("[GenericGridMap],calcGraphicsSize()");
                //_coord = new MCoord(_boundary, new Rectangle(0, 0, zoom_width, zoom_height));

                _coordData2UI = new CoordTransferTool(_boundary, new Rectangle(0, 0, zoom_width, zoom_height),  "",  "data2UI");// _coord

                //_coord.PushData(_boundary, new Rectangle(0, 0, zoom_width, zoom_height), chipID: "", remark: "data2UI");
                base.statusOn(EInnerStatus.CoordDetermined);
            }
            else
            {
                _coordData2UI = new CoordTransferTool(_boundary, new Rectangle(0, 0, zoom_width, zoom_height), "", "data2UI");// _coord
                //_coord.ChangeWorldSize(zoom_width, zoom_height);
            }

            SizeF sizeF = new SizeF(1f, 1f);
            //_coordData2UI.ScaleToWorld(ref sizeF);
            float x = 1f,y = 1f;
            //_coordData2UI.TransCoord(ref x, ref y);
            x = (float)Math.Abs(_coordData2UI.Matrix[0, 0]);
            y = (float)Math.Abs(_coordData2UI.Matrix[1, 1]);
            sizeF = new SizeF(x, y);
            
            _cellSize = (int)(Math.Round(Math.Abs(Math.Min(sizeF.Width, sizeF.Height)))) - _cellGap;

            if (_cellSize < MIN_ZOOM_SCALE)
                _cellSize = MIN_ZOOM_SCALE;

            this.SetGraphicsSize(zoom_width, zoom_height);
            base.setScrollChange(_cellSize, _cellSize, _cellSize + 2, _cellSize + 2);

            //var m = _coord.M_Trsf;
            //if (m.M22 < 0)
            //{
            //    m.M22 *= -1;
            //    m.Dy *= -1;
            //}
            //var m = _coord.M_Trsf;
            //m.M22 = Math.Abs(m.M22);
            //_coord.M_Trsf = m;

            //float val = _coord.M_Trsf.M22;
            //_coord.M_Trsf.M22 = val;
        }
        #endregion

        #region >>> Public / Override Method <<<

        public override void SetGraphicsSize(int width, int height)
        {
            base.setScrollBuffer(_cellSize);
            base.SetGraphicsSize(width, height);
        }

        public override void Zoom(int scale)
        {
            if (scale >= 0 && _cellSize != scale)
            {
                _cellSize = scale;
                base.saveScrollPosition();
                this.calcGraphicsSize();
                base.retoreScrollPosition();
                this.Draw();
            }
        }

		public void ResetScale()
		{
			_autoScale = true;

			base.resetScrollPosition();

			_cellSize = 1;
			this.calcGraphicsSize();

			_cellSize = 0;
			base.saveScrollPosition();
			this.calcGraphicsSize();
			this.Draw();
		}

        public override void AutoScale()
        {
            _autoScale = true;
            if (_cellSize != MIN_ZOOM_SCALE)
            {
                _cellSize = MIN_ZOOM_SCALE;
                base.resetScrollPosition();
                this.calcGraphicsSize();
            }
            else
                base.resetScrollPosition();

            this.Draw();
        }

        public void SetLayout(Rectangle bound)
        {
            if (_coordData2UI != null)
            {
                _coordData2UI.Dispose();
                _coordData2UI = null;
            }

            _cellSize = 1;
            int pad_x = _padding, pad_y = _padding;

            //bound.Inflate(pad_x, pad_y);

            int x = bound.Left - pad_x;
            int y = bound.Top + pad_y;
            int width = bound.Width + pad_x * 2;
            int height = bound.Height;

            if (height < 0)
            {
                y = bound.Top + pad_y;
                height = bound.Height - pad_y * 2;
            }
            else 
            {
                y = bound.Top - pad_y;
                height += pad_y * 2;
            }

             Rectangle tBound = new Rectangle(x, y, width, height);


            //int xLength = Math.Abs(bound.Width);
            //int yLength = Math.Abs(bound.Height);
            //int maxSide = Math.Max(xLength, yLength);

            //if (yLength < xLength)
            //{
            //    if (bound.Height < 0)
            //    {
            //        bound.Height = -xLength;
            //    }
            //}

            //_boundary = bound;
            _boundary = tBound;

            this.calcGraphicsSize();

            base.clearCanvas();

        }

        public void Redraw()
        {
            if (_coordData2UI != null)
            {
                _coordData2UI.Dispose();
                _coordData2UI = null;
            }

            this.calcGraphicsSize();
            this.Draw();
        }

        /// <summary>
        /// Set layout with padding
        /// </summary>
        /// <param name="bound">row / column range</param>
        /// <param name="padding">unit in row/col</param>
        public void SetLayout(Rectangle bound, int padding)
        {
            _padding = padding;
            this.SetLayout(bound);
        }

        #endregion

        public void DynamicDraw(int r, int c, Rectangle gridArea, bool redraw)
        {
            IntPtr hdc = _canvas.Hdc;
            IntPtr hObject = GDI32.SelectObject(hdc, GDI32.GetStockObject(EStockObject.DC_BRUSH));
            GDI32.SelectObject(hdc, GDI32.GetStockObject(EStockObject.DC_PEN));

            if (gridArea.Width == 0)
                gridArea.Width = 1;

            if (gridArea.Height == 0)
                gridArea.Height = 1;

            Graphics canvas_g = Graphics.FromHdcInternal(_canvas.Hdc);
            Console.WriteLine("[GenericGridMap],DynamicDraw()");
            //MCoord coord = new MCoord(gridArea, this.ClientRectangle);

            CoordTransferTool coord = new CoordTransferTool(gridArea, this.ClientRectangle);

            
            //int cell_size = coord.ScaleToWorld(1) - _cellGap;

            double m11 = coord.Matrix[0, 0];
            int cell_size = (int)m11 - _cellGap;

            if (cell_size < MIN_ZOOM_SCALE)
                cell_size = MIN_ZOOM_SCALE;

            if (redraw == false)
            {
                gridArea = new Rectangle(c, r, 1, 1);
            }

            int row_from = gridArea.Top;
            int row_to = gridArea.Bottom;
            int col_from = gridArea.Left;
            int col_to = gridArea.Right;

            int ptx = 0, pty = 0;

            if (redraw)
                TCanvasInfo.Clear(ref _canvas, _backColor);

            for (int row = row_from; row <= row_to; row++)
            {
                pty = row;
                for (int col = col_from; col <= col_to; col++)
                {
                    ptx = col;
                    //coord.ToWorld(ref ptx, ref pty);
                    coord.TransCoord(ref ptx, ref pty);

                    Color bg = _gradeRender.GetGradeColor(row, col);
                    if (bg.IsEmpty == false)
                    {
                        base.drawCell(_canvas.Hdc, ptx, pty, cell_size, (uint)ColorTranslator.ToWin32(bg));
                    }

                    _gradeRender.OwnerDraw(canvas_g, new Point(ptx, pty), cell_size, row, col);
                    pty = row; // restore 
                }
            }

            cell_size += 3;

            // draw focus, if enabled
            if (_cellFocus.IsEmpty == false)
            {
                Pen pen = new Pen(Color.Empty);
                //Point point = coord.ToWorld(_cellFocus.X, _cellFocus.Y);
                Point point = coord.TransCoord2P(_cellFocus.X, _cellFocus.Y);
                pen.Color = ColorTranslator.FromWin32((int)_focusColor);
                canvas_g.DrawRectangle(pen, point.X - 2, point.Y - 2, cell_size, cell_size);
                pen.Dispose();
                pen = null;
            }

            // draw hover, if enabled
            if (_cellHover.IsEmpty == false)
            {
                Pen pen = new Pen(Color.Empty);
                //Point point = coord.ToWorld(_cellHover.X, _cellHover.Y);
                Point point = coord.TransCoord2P(_cellHover.X, _cellHover.Y);
                pen.Color = ColorTranslator.FromWin32((int)_hoverColor);
                canvas_g.DrawRectangle(pen, point.X - 2, point.Y - 2, cell_size, cell_size);
                pen.Dispose();
                pen = null;
            }


            coord.Dispose();
            coord = null;
            canvas_g.Dispose();
            GDI32.SelectObject(hdc, hObject);
            //_coordData2UI.ToWorld(ref gridArea);

            gridArea = _coordData2UI.TransCoord( gridArea);
            //gridArea.Offset( _scrollX, _scrollY );
            this.Invalidate(gridArea);
        }

        internal void DrawToGraphics(Graphics graphics, Rectangle gridArea, Rectangle clientRect)
        {
            Console.WriteLine("[GenericGridMap],DrawToGraphics()");
            //MCoord coord = new MCoord(gridArea, clientRect);

            CoordTransferTool coord = new CoordTransferTool(gridArea, clientRect);
            Pen pen = new Pen(Color.Empty);

            //int cell_size = coord.ScaleToWorld(1) - _cellGap;

            double m11 = coord.Matrix[0, 0];
            int cell_size = (int)m11 - _cellGap;

            if (cell_size < MIN_ZOOM_SCALE)
                cell_size = MIN_ZOOM_SCALE;

            int row_from = gridArea.Top;
            int row_to = gridArea.Bottom;
            int col_from = gridArea.Left;
            int col_to = gridArea.Right;

            int ptx = 0, pty = 0;

            graphics.Clear(this.BackColor);

            for (int row = row_from; row <= row_to; row++)
            {
                pty = row;
                for (int col = col_from; col <= col_to; col++)
                {
                    ptx = col;
                    //coord.ToWorld(ref ptx, ref pty);

                    coord.TransCoord(ref ptx, ref pty);

                    if ((ptx < 0 && pty < 0) || (ptx > clientRect.Width && pty > clientRect.Height))
                        continue;

                    Color bg = _gradeRender.GetGradeColor(row, col);
                    if (bg.IsEmpty == false)
                    {
                        pen.Color = bg;
                        Rectangle rect = Rectangle.FromLTRB(ptx, pty, ptx + cell_size - 1, pty + cell_size - 1);
                        graphics.FillRectangle(pen.Brush, rect);
                        pen.Color = ColorTranslator.FromWin32(0x999999);
                        graphics.DrawRectangle(pen, Rectangle.FromLTRB(ptx, pty, ptx + cell_size - 1, pty + cell_size - 1));
                    }

                    _gradeRender.OwnerDraw(graphics, new Point(ptx, pty), cell_size, row, col);
                    pty = row; // restore 
                }
            }

            cell_size += 3;

            // draw focus, if enabled
            if (_cellFocus.IsEmpty == false)
            {
                //Point point = coord.ToWorld(_cellFocus.X, _cellFocus.Y);
                Point point = coord.TransCoord2P(_cellFocus.X, _cellFocus.Y);
                pen.Color = ColorTranslator.FromWin32((int)_focusColor);
                graphics.DrawRectangle(pen, point.X - 2, point.Y - 2, cell_size, cell_size);
            }

            // draw hover, if enabled
            if (_cellHover.IsEmpty == false)
            {

                //Point point = coord.ToWorld(_cellHover.X, _cellHover.Y);
                Point point = coord.TransCoord2P(_cellHover.X, _cellHover.Y);
                pen.Color = ColorTranslator.FromWin32((int)_hoverColor);
                graphics.DrawRectangle(pen, point.X - 2, point.Y - 2, cell_size, cell_size);
            }

            coord.Dispose();
            coord = null;
            pen.Dispose();
            pen = null;
        }

        public void GetDebugInfo(ListDictionary param)
        {
#if DEBUG
			if ( statusTest( EInnerStatus.CoordDetermined ) )
				param.Add( "focus.point", _coord.ToWorld( _cellFocus ) );
			else
				param.Add( "focus.point", Point.Empty );

			param.Add( "scroll.point", new Point( _scrollX, _scrollY ) );
			int w, h;

			param.Add( "scroll.size", new Size( _hScroll.Maximum, _vScroll.Maximum ) );
			param.Add( "graphics.size", new Size( _graphicsWidth, _graphicsHeight ) );
#endif
        }

        public bool ScaleToContent
        {
            get
            {
                return _autoScale;
            }
            set
            {
                _autoScale = value;
            }
        }

        public void Rotate(int angle)
        {
            if (_coordData2UI != null)
            {
                _coordData2UI.Dispose();
                _coordData2UI = null;
            }

            this.calcGraphicsSize();
            //base._coordData2UI.Rotate(angle);//20190424 David ¥ýµù¸Ñ±¼

            this.Draw();
        }

        public Rectangle Boundary
        {
            get
            {
                return _boundary;
            }
        }
    }
}
