using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Windows.Forms;

namespace MPI.Tester.Data
{
	[Serializable]
	public class MapData
	{
		#region >>> Private Property <<<

		private object _lockObj;

		private ColorSetting _colorSetting;
		
		private List<string> _weferMapShowItem;

		private string _mapBackColor;

		private AutoColorFilterSpec _filterSpec;

		#endregion

		#region >>> Constructor / Disposor <<<

		public MapData()
		{
			this._lockObj = new object();

			this._colorSetting = new ColorSetting();

			this._weferMapShowItem = new List<string>();

			this._mapBackColor = "ff000000";

			this._filterSpec = new AutoColorFilterSpec();
		}

		#endregion

		#region >>> Public Property <<<

		public ColorSetting ColorSetting
		{
			get { return this._colorSetting; }
			set { lock (this._lockObj) { this._colorSetting = value; } }
		}

		public List<string> WeferMapShowItem
		{
			get { return this._weferMapShowItem; }
			set { lock (this._lockObj) { this._weferMapShowItem = value; } }
		}

		public string MapBackColor
		{
			get { return this._mapBackColor; }
			set { lock (this._lockObj) { this._mapBackColor = value; } }
		}

		public AutoColorFilterSpec FilterSpec
		{
			get { return this._filterSpec; }
			set { lock (this._lockObj) { this._filterSpec = value; } }
		}

		#endregion
	}
}
