using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MPI.UCF.Forms.Domain
{
    public class EllipseBinRange : ICloneable
    {
        public float Angle;
        public float CenterX;
        public float CenterY;
        public float RadiusX;
        public float RadiusY;

        public EllipseBinRange(float angle, float centerX, float centerY, float radiusX, float radiusY)
        {
            Angle = angle;
            CenterX = centerX;
            CenterY = centerY;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
