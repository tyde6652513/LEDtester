using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class LayerDescriptor
	{
		#region >>> Const <<<

		private enum Items
		{
			Layers = 0,
			Row,
			Col,
		}

		#endregion

		#region >>> Private field <<<

		private CmdPropertyBased _command;

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		internal LayerDescriptor(CmdPropertyBased cmd)
		{
			_command = cmd;
		}

		#region >>> Public property <<<

		/// <summary>
		/// Total existed layers count.
		/// </summary>
		public int Layers
		{
			get { return this.GetValue<int>(Items.Layers, 0); }
			set
			{
				if (value < 0) return;

				this.SetValue<int>(Items.Layers, value);
			}
		}

		/// <summary>
		/// Retrieve Row/Column information for all existed layers.
		/// Which <see cref="Point"/>.X is Row and <see cref="Point"/>.Y is Column.
		/// </summary>
		public Point[] Infos
		{
			get
			{
				int nLayers = this.Layers;

				List<Point> layers = new List<Point>(nLayers);
				
				for (int nIndex = 0; nIndex < nLayers; ++nIndex)
				{
					layers.Add(this.Get(nIndex));
				}

				return layers.ToArray();
			}
		}

		#endregion

		#region >>> Public method <<<

		/// <summary>
		/// Set the Row/Column for the specified layer.
		/// </summary>
		/// <param name="nLayerIndex">The layer index specified.(0-based; It should be greater than 0 and less then <see cref="Layers"/>)</param>
		/// <param name="pt">The Row/Col specified. <paramref name="pt"/>.X is Row and <paramref name="pt"/>.Y is Column.</param>
		public void Set(int nLayerIndex, Point pt)
		{
			ValidateLayerIndex(nLayerIndex);

			this.SetValue<int>(this.GetRowFormatName(nLayerIndex), pt.X);
			this.SetValue<int>(this.GetColFormatName(nLayerIndex), pt.Y);
		}

		/// <summary>
		/// Get the Row/Colum for the specified layer.
		/// </summary>
		/// <param name="nLayerIndex">The layer index specified.(0-based; It should be greater than 0 and less then <see cref="Layers"/>)</param>
		/// <returns>The existed Row/Column struct which <see cref="Point"/>.X is Row and <see cref="Point"/>.Y is Column.</returns>
		public Point Get(int nLayerIndex)
		{
			ValidateLayerIndex(nLayerIndex);

			return new Point(this.GetValue<int>(this.GetRowFormatName(nLayerIndex), 0), 
							 this.GetValue<int>(this.GetColFormatName(nLayerIndex), 0));
		}

		#endregion

		#region >>> Private method <<<

		private void ValidateLayerIndex(int nLayerIndex)
		{
			int nTotalLayers = this.Layers;

			if (nLayerIndex < 0 || nLayerIndex >= nTotalLayers)
				throw new IndexOutOfRangeException(String.Format("Index:{0} is out of boundary. {1}", nLayerIndex, (nTotalLayers == 0) ? "No testing result is available." : String.Format("It should be between:0 ~ {1}.", nTotalLayers - 1)));
		}



		private T GetValue<T>(Items item, T defaultValue)
		{
			return this.GetValue<T>(FormatName(item), defaultValue);
		}

		private void SetValue<T>(Items item, T value)
		{
			this.SetValue<T>(FormatName(item), value);
		}

		private string GetRowFormatName(int nLayerIndex)
		{
			return String.Format("{0}#{1}", this.FormatName(Items.Row), nLayerIndex);
		}

		private string GetColFormatName(int nLayerIndex)
		{
			return String.Format("{0}#{1}", this.FormatName(Items.Col), nLayerIndex);
		}

		private string FormatName(Items item)
		{
			return item.GetHashCode().ToString();  // return item.ToString();
		}

		private T GetValue<T>(string name, T defaultValue)
		{
			dynamic value = _command.GetProperty(name);

			if (value is T) return value;

			try
			{
				object o = Convert.ChangeType(value, typeof(T));

				return o != null ? (T)o : defaultValue;
			}
			catch
			{
				return defaultValue;
			}
		}

		private void SetValue<T>(string name, T value)
		{
			_command.SetProperty(name, value);
		}

		#endregion
	}
}
