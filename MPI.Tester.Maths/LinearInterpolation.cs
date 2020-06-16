using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
	public static class LinearInterpolation
	{
		public static float Push(float x, float xa, float xb, float ya, float yb)
		{
			return ya + (yb - ya) * (x - xa) / (xb - xa);
		}
	}
}
