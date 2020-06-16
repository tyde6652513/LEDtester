using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using System.Drawing.Design;
using System.ComponentModel;

using MPI.Tester.Data;

namespace MPI.Windows.Forms
{
	public class BinGradeColor
	{
		#region >>> Private Field <<<
		private List<Color> _colorItems;
		private List<float> _valueItem;
		#endregion

		#region >>> Public Field <<<
		public float Min;
		public float Max;
		public float Step;

		public Color ColorMax;
		public Color ColorMin;

		public string Title;
		public string KeyName;
		#endregion

		public string DisplayFormat;
		public BinGradeColor()
		{
			Min = 0.0f;
			Max = 1.0f;
			ColorMax = Color.Green;
			ColorMin = Color.Red;
			DisplayFormat = "0.0000";

			_colorItems = new List<Color>( ColorSettingData.MAX_COLOR_LEVEL );
			_valueItem = new List<float>( ColorSettingData.MAX_COLOR_LEVEL );
		}

		#region >>> Private Method <<<
		private Color parseColor( string value )
		{
			if ( value == null || value.Length == 0 )
				return Color.Empty;

			return Color.FromArgb( ( int ) Convert.ToUInt32( value, 16 ) );
		}

		private string colorToHexStr( ref Color color )
		{
			return ( ( uint ) color.ToArgb() ).ToString( "x" );
		}

		#endregion

		#region >>> Internal Method <<<
		internal void innerReadFromXml( XmlElement item )
		{
			/*
				<item name="VFc" title="VFc">
					<value min="0" step="0.1" max="2" format="F4"/>
			  		<out-of-range  minColor="" maxColor=""/>
					<color value="0.000">ffff0000</color>
					 ...
			 */

			this.KeyName = item.GetAttribute( "name" );
			this.Title = item.GetAttribute( "title" );

			XmlElement ele = item["value"];
			this.DisplayFormat = ele.GetAttribute( "format" );
			this.Min = float.Parse( ele.GetAttribute( "min" ) );
			this.Max = float.Parse( ele.GetAttribute( "max" ) );
			this.Step = float.Parse( ele.GetAttribute( "step" ) );

			ele = item["out-of-range"];
			this.ColorMax = this.parseColor( ele.GetAttribute( "maxColor" ) );
			this.ColorMin = this.parseColor( ele.GetAttribute( "minColor" ) );

			XmlNodeList xnl = item.SelectNodes( "color" );
			foreach ( XmlElement ci in xnl )
			{
				_colorItems.Add( this.parseColor( ci.InnerText ) );
				_valueItem.Add( float.Parse( ci.GetAttribute( "value" ) ) );
			}

			while ( _colorItems.Count < ColorSettingData.MAX_COLOR_LEVEL )
			{
				_colorItems.Add( Color.Empty );
				_valueItem.Add( float.NaN );
			}
		}

		internal void ReadFromTestResultData( ColorSettingData item )
		{
			this.KeyName = item.KeyName;
			this.Title = item.Name;
			this.DisplayFormat = item.Formate;
			this.Min = item.Min;
			this.Max = item.Max;
			this.Step = item.Step;
			this.ColorMax = this.parseColor( item.MaxColor );
			this.ColorMin = this.parseColor( item.MinColor );

			if ( item.ColorLevelList.Count != 32 )
			{
				this.Step = 1;
				this.Min = 0;
				this.Max = ColorSettingData.MAX_COLOR_LEVEL;
				this.ColorMin = Color.White;
				this.ColorMax = Color.Purple;

				item.ColorLevelList.Clear();

				int R = 256;
				int G = 0;
				int B = 0;

				for ( int i = 0; i < ColorSettingData.MAX_COLOR_LEVEL; i++ )
				{
					if ( i > 0 && i <= 8 )
					{
						G += 32;
					}
					else if ( i > 9 && i <= 16 )
					{
						R -= 32;
					}
					else if ( i > 16 && i <= 24 )
					{
						B += 32;
					}
					else if ( i > 24 && i <= 32 )
					{
						G -= 32;
					}

					Color color = Color.FromArgb( ( R - 1 ) > 0 ? ( R - 1 ) : R,
												( G - 1 ) > 0 ? ( G - 1 ) : G,
												( B - 1 ) > 0 ? ( B - 1 ) : B
												);

					string strColor = ColorSettingData.ParseColor( color );

					item.ColorLevelList.Add( new ColorLevel( i, strColor ) );
				}
			}

			foreach ( ColorLevel cl in item.ColorLevelList )
			{
				_colorItems.Add( this.parseColor( cl.Levelcolor ) );
				_valueItem.Add( cl.LevelValue );
			}

			while ( _colorItems.Count < ColorSettingData.MAX_COLOR_LEVEL )
			{
				_colorItems.Add( Color.Empty );
				_valueItem.Add( float.NaN );
			}
		}

		internal void innerWriteToXml( XmlWriter writer )
		{
			/*
				<item name="VFc" title="VFc">
					<value min="0" step="0.1" max="2" format="F4"/>
					<out-of-range  minColor="" maxColor=""/>
					<color value="0.000">ffff0000</color>
				 ...
			 */
			writer.WriteStartElement( "item" );
			writer.WriteAttributeString( "name", this.KeyName );
			writer.WriteAttributeString( "title", this.Title );
			{
				writer.WriteStartElement( "value" );
				{
					writer.WriteAttributeString( "min", this.Min.ToString( this.DisplayFormat ) );
					writer.WriteAttributeString( "step", this.Step.ToString( this.DisplayFormat ) );
					writer.WriteAttributeString( "max", this.Max.ToString( this.DisplayFormat ) );
					writer.WriteAttributeString( "format", this.DisplayFormat );
				}
				writer.WriteEndElement();

				writer.WriteStartElement( "out-of-range" );
				{
					writer.WriteAttributeString( "minColor", colorToHexStr( ref this.ColorMin ) );
					writer.WriteAttributeString( "maxColor", colorToHexStr( ref this.ColorMax ) );
				}
				writer.WriteEndElement();

				for ( int i = 1; i <= ColorSettingData.MAX_COLOR_LEVEL; i++ )
				{
					writer.WriteStartElement( "color" );
					{
						writer.WriteAttributeString( "value", _valueItem[i - 1].ToString( this.DisplayFormat ) );
						writer.WriteString( ( ( uint ) _colorItems[i - 1].ToArgb() ).ToString( "x" ) );
					}
					writer.WriteEndElement();
				}
			}

			writer.WriteEndElement();
		}

		internal int innerGetValueIndex( float value )
		{
			// PAUL
			if ( value >= this.Max )
				return int.MaxValue;

			if ( value < this.Min )
				return int.MinValue;

			int idx = ( int ) ( ( value - this.Min ) / this.Step );

			if ( idx >= ColorSettingData.MAX_COLOR_LEVEL )
				return ColorSettingData.MAX_COLOR_LEVEL - 1;

			if ( idx < 0 )
				return 0;

			return idx;
		}

		public Color[] GetColorItems()
		{
			return _colorItems.ToArray();
		}

		public bool SetColorItems(Color[] colorAry)
		{
			_colorItems = new List<Color>( colorAry.Length );

			foreach ( Color objColor in colorAry )
			{
				_colorItems.Add( objColor );
			}

			return true;
		}

		internal float[] GetValueItems()
		{
			List<float> items = new List<float>( ColorSettingData.MAX_COLOR_LEVEL );

			float step = 1.0f / ( float ) ColorSettingData.MAX_COLOR_LEVEL;
			for ( int i = 0; i < ColorSettingData.MAX_COLOR_LEVEL - 1; i++ )
				items.Add( step * i );

			items.Add( 1.0f );
			return items.ToArray();
		}

		#endregion

		#region >>> Public Method <<<
		public Color GetColor( float value )
		{
			int idx = this.innerGetValueIndex( value );

			if ( idx == int.MaxValue )
			{
				if ( BinGradeColorSet.UseNGColor )
					return this.ColorMax;

				return _colorItems[ColorSettingData.MAX_COLOR_LEVEL - 1];
			}

			if ( idx == int.MinValue )
			{
				if ( BinGradeColorSet.UseNGColor )
					return this.ColorMin;

				return _colorItems[0];
			}

			if ( idx < ColorSettingData.MAX_COLOR_LEVEL )
				return _colorItems[idx];

			return Color.Empty;
		}

		public void AddColorItem( float value, Color color )
		{
			if ( _valueItem.Contains( value ) == false )
			{
				_valueItem.Add( value );
				_colorItems.Add( color );
			}
		}

		#endregion

		#region >>> Override Method <<<
		public override string ToString()
		{
			return Title;
		}

		#endregion
	}

	public class BinGradeColorSet
	{
		private Dictionary<string, ColorSettingData> _colorSettingData;
		private Dictionary<string, BinGradeColor> fColorBag;
		private Dictionary<string, BinGradeColor> fBigColorBag;
		private static List<Dictionary<string, float>> dieDatabase;
		private string fGroupName;
		private static MapData _mapData;
		private const string OUT_OF_MIN_KEY = "OUT.OF.MIN";
		private const string OUT_OF_MAX_KEY = "OUT.OF.MAX";

		#region >>> Private Field <<<

		private string _tablePath;
		private Dictionary<string, BinGradeColor> _colorBag;

		private Dictionary<string, List<int>> _itemCounter;
		private Dictionary<string, int> _outMinCounter;
		private Dictionary<string, int> _outMaxCounter;

		private static BinGradeColorSet self;
		private static bool _ready;
		#endregion

		#region >>> Constructor / Disposor <<<

		public BinGradeColorSet()
		{
			fColorBag = new Dictionary<string, BinGradeColor>();
			fBigColorBag = new Dictionary<string, BinGradeColor>();
			dieDatabase = new List<Dictionary<string, float>>();
		}

		private bool loadEachItem( XmlElement root )
		{
			_colorBag = new Dictionary<string, BinGradeColor>( ColorSettingData.MAX_COLOR_LEVEL );
			_itemCounter = new Dictionary<string, List<int>>( ColorSettingData.MAX_COLOR_LEVEL );
			_outMinCounter = new Dictionary<string, int>();
			_outMaxCounter = new Dictionary<string, int>();

			XmlNodeList xnl = root.SelectNodes( "item" );
			foreach ( XmlElement item in xnl )
			{
				BinGradeColor bgc = new BinGradeColor();
				bgc.innerReadFromXml( item );
				if ( _colorBag.ContainsKey( bgc.KeyName ) == false )
				{
					_colorBag.Add( bgc.KeyName, bgc );
					_itemCounter.Add( bgc.KeyName, new List<int>( new int[ColorSettingData.MAX_COLOR_LEVEL] ) );
					_outMinCounter.Add( bgc.KeyName, 0 );
					_outMaxCounter.Add( bgc.KeyName, 0 );
				}
			}


			return true;
		}

		private bool loadEachItem()
		{
			if (_mapData == null || _mapData.ColorSetting.DataList.Count == 0)
			{
				_ready = false;

				return false;
			}

			this._colorSettingData = new Dictionary<string, ColorSettingData>();

			this._colorBag = new Dictionary<string, BinGradeColor>( ColorSettingData.MAX_COLOR_LEVEL );

			this._itemCounter = new Dictionary<string, List<int>>( ColorSettingData.MAX_COLOR_LEVEL );

			this._outMinCounter = new Dictionary<string, int>();

			this._outMaxCounter = new Dictionary<string, int>();

			foreach (var item in _mapData.ColorSetting.DataList)
			{
				if (!this._colorSettingData.ContainsKey(item.KeyName))
					this._colorSettingData.Add(item.KeyName, item);

				BinGradeColor bgc = new BinGradeColor();

				bgc.ReadFromTestResultData( item );

				if ( _colorBag.ContainsKey( bgc.KeyName ) == false )
				{
					_colorBag.Add( bgc.KeyName, bgc );

					_itemCounter.Add( bgc.KeyName, new List<int>( new int[ColorSettingData.MAX_COLOR_LEVEL] ) );

					_outMinCounter.Add( bgc.KeyName, 0 );

					_outMaxCounter.Add( bgc.KeyName, 0 );
				}
			}

			UseNGColor = _mapData.ColorSetting.EnableColorOutOfRange;

			return true;
		}

		private BinGradeColorSet( string path )
		{
			//WaferMap.BinColor.xml
			XmlDocument xml = new XmlDocument();
			try
			{
				xml.Load( path );
			}
			catch ( XmlException xe )
			{
				Console.Error.WriteLine( "[BinGradeColorSet] " + xe.Message );
				xml = null;
				return;
			}

			XmlElement root = xml.DocumentElement;
			_ready = this.loadEachItem( root );

			if ( _ready )
			{
				//<setting>
				//   <out-of-range>true</out-of-range>		
				//</setting>
				XmlElement ele = root["setting"];
				UseNGColor = "true".Equals( ele["out-of-range"].InnerText, StringComparison.OrdinalIgnoreCase );
				_tablePath = path;
			}
		}

		private BinGradeColorSet(MapData mapData)
		{
			_ready = this.loadEachItem();
		}

		#endregion

		#region >>> Private Method <<<
		private bool saveAll()
		{
			//Save to xml
			using ( FileStream fs = new FileStream( _tablePath, FileMode.Create ) )
			{
				using ( XmlTextWriter writer = new XmlTextWriter( fs, Encoding.UTF8 ) )
				{
					writer.Formatting = System.Xml.Formatting.Indented;
					writer.Indentation = 1;
					writer.IndentChar = '\t';

					writer.WriteStartElement( "WaferMapColor" );
					{
						writer.WriteStartElement( "setting" );
						{
							writer.WriteStartElement( "out-of-range" );
							{
								writer.WriteString( "true" );
							}
							writer.WriteEndElement();
						}
						writer.WriteEndElement();

						foreach ( KeyValuePair<string, BinGradeColor> item in _colorBag )
							item.Value.innerWriteToXml( writer );
					}
					writer.WriteEndElement();

					writer.Close();
				}
				fs.Close();
			}


			return true;
		}

		private bool SaveAll()
		{
			foreach ( var item in _colorBag )
			{
				if (!this._colorSettingData.ContainsKey(item.Key))
					continue;

				ColorSettingData data = this._colorSettingData[item.Key];

				BinGradeColor setData = ( item.Value as BinGradeColor );

				data.Step = setData.Step;
				data.Min = setData.Min;
				data.Max = setData.Max;
				data.MinColor = ColorSettingData.ParseColor( setData.ColorMin );
				data.MaxColor = ColorSettingData.ParseColor( setData.ColorMax );

				Color[] colorItems = setData.GetColorItems();
				float[] valueItems = setData.GetValueItems();

				for ( int i = 0; i < ColorSettingData.MAX_COLOR_LEVEL; i++ )
				{
					data.ColorLevelList[i].Levelcolor = ColorSettingData.ParseColor( colorItems[i] );
					data.ColorLevelList[i].LevelValue = valueItems[i];
				}
			}

			_mapData.ColorSetting.EnableColorOutOfRange = UseNGColor;

			return true;
		}

		private bool makeNewItem( BinGradeColor item )
		{
			if ( _colorBag.ContainsKey( item.KeyName ) )
				return false;

			_colorBag.Add( item.KeyName, item );
			return this.saveAll();
		}

		private void countItem( string name, float value )
		{
			BinGradeColor bgc = _colorBag[name];
			int idx = bgc.innerGetValueIndex( value );

			if ( idx == int.MinValue )
			{
				lock ( _outMinCounter )
					_outMinCounter[name]++;

				return;
			}

			if ( idx == int.MaxValue )
			{
				lock ( _outMaxCounter )
					_outMaxCounter[name]++;

				return;
			}

			List<int> counter = _itemCounter[name];
			lock ( counter )
			{
				counter[idx]++;
			}
		}

		private int[] getOutRangeCounter( string name )
		{
			if ( _outMinCounter.ContainsKey( name ) == false )
				return _empty;

			return new int[2] { _outMinCounter[name], _outMaxCounter[name] };
		}

		#endregion

		#region >>> Public Static Method <<<

		public static Color ShowColorPicker( Control control )
		{
			return ColorEditorService.ShowEditor( control );
		}

		public static bool Initialize( string tablePath )
		{
			if ( self != null )
				return true;

			if ( File.Exists( tablePath ) == false )
				return false;

			self = new BinGradeColorSet( tablePath );
			return _ready;
		}

		public static bool Initialize(MapData mapData)
		{
			self = null;

			_mapData = mapData;

			self = new BinGradeColorSet(mapData);

			return _ready;
		}

		public static bool Save()
		{
			if ( _ready == false )
				return false;

			return self.SaveAll();
		}

		public static bool UseNGColor;

		public static bool IsReady
		{
			get
			{
				return _ready;
			}
		}

		public static bool NewItem( BinGradeColor item )
		{
			if ( _ready == false )
				return false;

			return self.makeNewItem( item );
		}

		public static BinGradeColor GetColorItem( string name )
		{
			if ( _ready == false || self._colorBag.ContainsKey( name ) == false )
				return null;

			return self._colorBag[name];
		}

		public static Dictionary<string, string> GetNameList()
		{
			if ( _ready == false )
				return null;

			Dictionary<string, string> list = new Dictionary<string, string>();

			foreach ( BinGradeColor bgc in self._colorBag.Values )
				list.Add( bgc.KeyName, bgc.Title );

			return list;
		}

		public static void CounterItem( string name, float value )
		{
			if ( _ready == false || self._itemCounter.ContainsKey( name ) == false )
				return;

			self.countItem( name, value );
		}

		public static void CounterItem( Dictionary<string, float> rowData )
		{
			foreach ( var item in rowData )
			{
				if ( _ready == false || self._itemCounter.ContainsKey( item.Key ) == false )
					continue;

				self.countItem( item.Key, item.Value );
			}

			dieDatabase.Add( rowData );
		}

		public static void Reflash()
		{
			if ( _ready == false )
				return;

			foreach ( KeyValuePair<string, List<int>> item in self._itemCounter )
			{
				item.Value.Clear();
				item.Value.AddRange( new int[ColorSettingData.MAX_COLOR_LEVEL] );

				self._outMaxCounter[item.Key] = 0;
				self._outMinCounter[item.Key] = 0;
			}

			foreach ( var rowData in dieDatabase )
			{
				foreach ( var item in rowData )
				{
					if ( _ready == false || self._itemCounter.ContainsKey( item.Key ) == false )
						continue;

					self.countItem( item.Key, item.Value );
				}
			}
		}

		private static readonly int[] _empty = new int[2];

		public static int[] GetOutOfRangeCounter( string name )
		{
			if ( _ready == false )
				return _empty;

			return self.getOutRangeCounter( name );
		}

		public static int[] GetCounterItem( string name )
		{
			if ( _ready == false || self._itemCounter.ContainsKey( name ) == false )
				return null;

			List<int> counter = self._itemCounter[name];
			int[] list = null;
			lock ( counter )
				list = counter.ToArray();

			return list;
		}

		public static void ResetCounter()
		{
			if ( _ready == false )
				return;

			foreach ( KeyValuePair<string, List<int>> item in self._itemCounter )
			{
				item.Value.Clear();
				item.Value.AddRange( new int[ColorSettingData.MAX_COLOR_LEVEL] );

				self._outMaxCounter[item.Key] = 0;
				self._outMinCounter[item.Key] = 0;
			}

			dieDatabase.Clear();
		}

		#endregion

		#region >>> Old Public Property <<

		public BinGradeColor this[string key]
		{
			get
			{
				return fColorBag[key];
			}

			set
			{
				if ( fColorBag.ContainsKey( key ) )
					fColorBag[key] = value;
			}
		}

		public string GroupName
		{
			get
			{
				return this.fGroupName;
			}

			set
			{
				if ( value == null )
					return;

				this.fGroupName = value;
			}

		}

		#endregion

		#region >>> Old Public Method <<<

		public void Add( string key, BinGradeColor value )
		{
			if ( fColorBag.ContainsKey( key ) )
				return;

			fColorBag.Add( key, value );
		}

		public bool ContainsKey( string key )
		{
			return fColorBag.ContainsKey( key );
		}

		public void Clear()
		{
			fColorBag.Clear();
			fBigColorBag.Clear();
		}

		public Array ToArray()
		{
			BinGradeColor[] bgc = new BinGradeColor[fColorBag.Count];
			fColorBag.Values.CopyTo( bgc, 0 );

			return bgc;
		}

		public void LoadFrom( string path )
		{
			XmlDocument xml = new XmlDocument();

			try
			{
				xml.Load( path );
			}
			catch
			{
				this.applyDefaultBinColor( xml );
				xml.Save( path );
			}

			XmlNode group = xml.SelectSingleNode( string.Format( "/root/group[@name='{0}']", fGroupName ) );
			fBigColorBag.Clear();
			foreach ( XmlElement item in group )
			{
				BinGradeColor bgc = new BinGradeColor();
				string name = item.GetAttribute( "name" );
				bgc.KeyName = name;

				//max & color
				XmlElement ele = item["max"];
				//Color back_color = Color.FromArgb( int.Parse( ele.GetAttribute( "color" ) ) );
				//bgc.ColorMax = back_color;
				bgc.Max = float.Parse( ele.InnerText );


				//min & color
				ele = item["min"];
				//back_color = Color.FromArgb( int.Parse( ele.GetAttribute( "color" ) ) );
				//bgc.ColorMin = back_color;
				bgc.Min = float.Parse( ele.InnerText );


				//title
				bgc.Title = item.GetAttribute( "title" );

				fBigColorBag.Add( bgc.KeyName, bgc );
			}


			foreach ( KeyValuePair<string, BinGradeColor> item in fColorBag )
			{
				BinGradeColor bgc = item.Value;
				if ( fBigColorBag.ContainsKey( item.Key ) )
				{
					bgc.Max = fBigColorBag[item.Key].Max;
					//bgc.ColorMax = fBigColorBag[item.Key].ColorMax;
					bgc.Min = fBigColorBag[item.Key].Min;
					//bgc.ColorMin = fBigColorBag[item.Key].ColorMin;
				}
				else
				{
					fBigColorBag.Add( item.Key, bgc );
				}
				//if (fBigColorBag.ContainsKey(item.Key) == false)
				//{
				//    fBigColorBag.Add(item.Key, bgc);
				//}
			}
		}

		public void SaveTo( string filepath )
		{
			foreach ( KeyValuePair<string, BinGradeColor> item in fColorBag )
			{
				BinGradeColor bgc;
				if ( fBigColorBag.ContainsKey( item.Key ) )
				{
					bgc = item.Value;
					fBigColorBag[item.Key].Max = bgc.Max;
					//fBigColorBag[item.Key].ColorMax = bgc.ColorMax;
					fBigColorBag[item.Key].Min = bgc.Min;
					//fBigColorBag[item.Key].ColorMin = bgc.ColorMin;
				}
			}

			XmlWriterSettings xws = new XmlWriterSettings();
			xws.OmitXmlDeclaration = false;
			xws.CloseOutput = false;
			xws.Indent = true;

			using ( FileStream fs = new FileStream( filepath, FileMode.Truncate ) )
			{
				XmlWriter xwtr = XmlWriter.Create( fs, xws );

				xwtr.WriteStartDocument( true );
				xwtr.WriteStartElement( "root" );
				{
					xwtr.WriteStartElement( "group" );
					{
						xwtr.WriteAttributeString( "name", fGroupName );

						foreach ( KeyValuePair<string, BinGradeColor> item in fBigColorBag )
						{
							BinGradeColor bgc = item.Value;

							xwtr.WriteStartElement( "item" );
							{
								xwtr.WriteAttributeString( "name", bgc.KeyName );
								xwtr.WriteAttributeString( "title", bgc.Title );
								xwtr.WriteStartElement( "max" );
								{
									//xwtr.WriteAttributeString( "color", bgc.ColorMax.ToArgb().ToString() );
									xwtr.WriteString( bgc.Max.ToString() );
									xwtr.WriteEndElement();
								}
								xwtr.WriteStartElement( "min" );
								{
									//xwtr.WriteAttributeString( "color", bgc.ColorMin.ToArgb().ToString() );
									xwtr.WriteString( bgc.Min.ToString() );
								}
								xwtr.WriteEndElement();
							}
							xwtr.WriteEndElement();
						}
					}
					xwtr.WriteEndElement();
				}
				xwtr.WriteEndElement();

				xwtr.WriteEndDocument();

				xwtr.Flush();
				xwtr.Close();

				fs.Close();
			}
		}

		#endregion

		#region >>> Old Private Method <<<

		private void applyDefaultBinColor( XmlDocument xml )
		{
			XmlNode root = xml.AppendChild( xml.CreateElement( "root" ) );
			XmlElement group = ( XmlElement ) root.AppendChild( xml.CreateElement( "group" ) );
			group.SetAttribute( "name", fGroupName );

			foreach ( KeyValuePair<string, BinGradeColor> item in fColorBag )
			{
				BinGradeColor bgc = item.Value;
				XmlElement ele = ( XmlElement ) group.AppendChild( xml.CreateElement( "item" ) );
				ele.SetAttribute( "name", item.Key );
				ele.SetAttribute( "title", bgc.Title );

				ele.AppendChild( xml.CreateElement( "max" ) ).Attributes.Append( xml.CreateAttribute( "color" ) );
				ele.AppendChild( xml.CreateElement( "min" ) ).Attributes.Append( xml.CreateAttribute( "color" ) );

				ele["max"].SetAttribute( "color", Color.Green.ToArgb().ToString() );
				ele["max"].InnerText = "1.0";
				ele["min"].SetAttribute( "color", Color.Red.ToArgb().ToString() );
				ele["min"].InnerText = "0.0";
			}
		}

		#endregion
	}

	internal class ColorEditorService : IServiceProvider, IWindowsFormsEditorService
	{
		private Control _activeControl;
		/// <summary>
		/// Creates the editor service.
		/// </summary>
		/// <param name="editor">The cell container.</param>
		public ColorEditorService( Control control )
		{
			_activeControl = control;
		}

		[DllImport( "user32.dll" )]
		private static extern int MsgWaitForMultipleObjects( int nCount, int pHandles, bool bWaitAll, int dwMilliseconds, int dwWakeMask );

		/// <summary>
		/// Drops the editor control.
		/// </summary>
		/// <param name="ctl">The control to drop.</param>
		public void DropDownControl( Control designer )
		{
			// location of the form
			Point location = _activeControl.Location;
			// location in screen coordinate
			location = _activeControl.Parent.PointToScreen( location );
			location.Y += _activeControl.Height + 1;

			using ( Form form = new Form() )
			{
				form.Visible = false;

				form.StartPosition = FormStartPosition.Manual;
				form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
				form.ShowInTaskbar = false;
				form.ControlBox = false;
				form.MinimizeBox = false;
				form.MaximizeBox = false;
				form.Text = string.Empty;
				form.Location = location;
				form.Size = new Size( 2 + designer.Width, 2 + designer.Height );
				form.Deactivate += new EventHandler( Form_Deactivate );
				form.Controls.Add( designer );
				form.Visible = true;

				designer.Focus();

				// wait for the end of the editing
				while ( _activeControl.Focused == false )
				{
					Application.DoEvents();
					MsgWaitForMultipleObjects( 0, 0, true, 300, 255 );
				}

				// $RIC, designer must be removed from container form first, 
				// or designer will be disposed.
				form.Controls.Clear();
			}
		}

		private void Form_Deactivate( object sender, EventArgs e )
		{
			_activeControl.Focus();
		}

		/// <summary>
		/// Closes the dropped editor.
		/// </summary>
		public void CloseDropDown()
		{
			_activeControl.Focus();
		}

		/// <summary>
		/// Opens a dialog editor.
		/// </summary>
		/// <param name="dialog">The dialog to open.</param>
		public DialogResult ShowDialog( Form dialog )
		{
			dialog.ShowDialog( _activeControl );
			return dialog.DialogResult;
		}

		public object GetService( Type serviceType )
		{
			if ( serviceType == typeof( IWindowsFormsEditorService ) )
				return this;

			return null;
		}

		public static Color ShowEditor( Control actControl )
		{
			Type edtype = typeof( Color );
			UITypeEditor editor = ( UITypeEditor ) TypeDescriptor.GetEditor( edtype, typeof( UITypeEditor ) );
			ColorEditorService edsvc = new ColorEditorService( actControl );
			try
			{
				return ( Color ) editor.EditValue( edsvc, null );


			}
			catch
			{
				return actControl.BackColor;
			}
		}
	}

}
