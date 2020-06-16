using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MPI.UCF.Forms.Domain
{
	public delegate Color GradeColorCallback( float value );
	public delegate Color RowColumnColorCallback( int row, int column, out EDieStatus status );
	public delegate void SelectionEventHandler( int rowStart, int colStart, int rowEnd, int colEnd );
	public delegate void ChipFocusEventHandler( int row, int column );

}
