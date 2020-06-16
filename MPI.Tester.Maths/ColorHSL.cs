using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
	public class ColorHSL
	{
		private object _lockObj;

		private int _alpha;
		private int _hue;
		private double _saturation;
		private double _lightness;

		public ColorHSL()
		{
			this._lockObj = new object();
			
			this._alpha = 255;
			this._hue = 0;			
			this._saturation = 1.0d;
			this._lightness = 1.0d;	 
		}

		public ColorHSL(int R, int G, int B) : base()
		{
			this.RGB2HSL(R, G, B);
		}

		public ColorHSL(int A, int R, int G, int B) : this ( R, G, B )
		{
			this._alpha = A;
		}

		public ColorHSL(int hue, double saturation, double lightness)
		{
			Hue = hue;
			Saturation = saturation;
			Lightness = lightness;
		}

		public int Hue
		{
			get { return _hue; }
			set
			{
				if (value < 0)
				{
					_hue = value + 360;
				}
				else if (_hue > 360)
				{
					_hue = value % 360;
				}
				else
				{
					_hue = value;
				}
			}
		}

		public double Saturation
		{
			get { return _saturation; }
			set
			{
				if (_saturation < 0)
				{
					_saturation = 0;
				}
				else
				{
					_saturation = Math.Min(value, 1d);
				}
			}
		}

		public double Lightness
		{
			get { return _lightness; }
			set
			{
				if (_lightness < 0)
				{
					_lightness = 0;
				}
				else
				{
					_lightness = Math.Min(value, 1d);
				}
			}
		}

		public static bool operator ==(ColorHSL left, ColorHSL right)
		{
			return (left.Hue == right.Hue && left.Lightness == right.Lightness && left.Saturation == right.Saturation);
		}

		public static bool operator !=(ColorHSL left, ColorHSL right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			if (obj == null && !(obj is ColorHSL))
			{
				return false;
			}

			ColorHSL color = (ColorHSL)obj;
			return this == color;
		}

		//public override int GetHashCode()
		//{
		//    return Color.GetHashCode();
		//}

		public override string ToString()
		{
			return string.Format("HSL({0:f2}, {1:f2}, {2:f2})", Hue, Saturation, Lightness);
		}

		private void RGB2HSL(int R, int G, int B)
		{
			double r = ((double)R) / 255;
			double g = ((double)G) / 255;
			double b = ((double)B) / 255;

			double min = Math.Min(Math.Min(r, g), b);
			double max = Math.Max(Math.Max(r, g), b);
			double distance = max - min;

			_lightness = (max + min) / 2;

			if (distance == 0)
			{
				_hue = 0;
				_saturation = 0;
			}
			else
			{
				double hueTmp;
				_saturation = (_lightness < 0.5) ? (distance / (max + min)) : (distance / ((2 - max) - min));
				double tempR = (((max - r) / 6) + (distance / 2)) / distance;
				double tempG = (((max - g) / 6) + (distance / 2)) / distance;
				double tempB = (((max - b) / 6) + (distance / 2)) / distance;

				if (r == max)
				{
					hueTmp = tempB - tempG;
				}
				else if (g == max)
				{
					hueTmp = (0.33333333333333331 + tempR) - tempB;
				}
				else
				{
					hueTmp = (0.66666666666666663 + tempG) - tempR;
				}

				if (hueTmp < 0)
				{
					hueTmp += 1;
				}

				if (hueTmp > 1)
				{
					hueTmp -= 1;
				}

				_hue = (int)(hueTmp * 360);
			}
		}

		//private Color ToRGB()
		//{
		//    byte r;
		//    byte g;
		//    byte b;

		//    if (_saturation == 0)
		//    {
		//        r = (byte)(_lightness * 255);
		//        g = r;
		//        b = r;
		//    }
		//    else
		//    {
		//        double vH = ((double)_hue) / 360;
		//        double v2 =
		//            (_lightness < 0.5) ?
		//            (_lightness * (1 + _saturation)) :
		//            ((_lightness + _saturation) - (_lightness * _saturation));

		//        double v1 = (2 * _lightness) - v2;

		//        r = (byte)(255 * HueToRGB(v1, v2, vH + 0.33333333333333331));
		//        g = (byte)(255 * HueToRGB(v1, v2, vH));
		//        b = (byte)(255 * HueToRGB(v1, v2, vH - 0.33333333333333331));
		//    }

		//    return Color.FromArgb(r, g, b);
		//}

		private double HueToRGB(double v1, double v2, double vH)
		{
			if (vH < 0)
			{
				vH += 1;
			}

			if (vH > 1)
			{
				vH -= 1;
			}

			if ((6 * vH) < 1)
			{
				return v1 + (((v2 - v1) * 6) * vH);
			}

			if ((2 * vH) < 1)
			{
				return v2;
			}

			if ((3 * vH) < 2)
			{
				return v1 + (((v2 - v1) * (0.66666666666666663 - vH)) * 6);
			}

			return v1;
		} 
		
		public void SetRGB( int R, int G, int B )
		{
			this.RGB2HSL(R, G, B);
		}

		public void SetARGB(int A,int R, int G, int B)
		{	
			this._alpha = A;
			this.RGB2HSL(R, G, B);
		}

		public static void HSL2RGB(double h, double sl, double l, out int R, out int G, out int B)
		{
			double v;
			double r, g, b;

			r = l;   // default to gray
			g = l;
			b = l;
			v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

			if (v > 0)
			{
				double m;
				double sv;
				int sextant;
				double fract, vsf, mid1, mid2;

				m = l + l - v;
				sv = (v - m) / v;
				h *= 6.0;
				sextant = (int)h;
				fract = h - sextant;
				vsf = v * sv * fract;
				mid1 = m + vsf;
				mid2 = v - vsf;

				switch (sextant)
				{
					case 0:
						r = v;
						g = mid1;
						b = m;
						break;
					case 1:
						r = mid2;
						g = v;
						b = m;
						break;
					case 2:
						r = m;
						g = v;
						b = mid1;
						break;
					case 3:
						r = m;
						g = mid2;
						b = v;
						break;
					case 4:
						r = mid1;
						g = m;
						b = v;
						break;
					case 5:
						r = v;
						g = m;
						b = mid2;
						break;
				}
			}

			R = Convert.ToInt32(r * 255.0f);
			G = Convert.ToInt32(g * 255.0f);
			B = Convert.ToInt32(b * 255.0f);
		}
	}
}
