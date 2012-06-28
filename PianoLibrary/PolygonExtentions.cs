using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace PianoLibrary
{
    public static class PolygonExtentions
    {
        public static Polygon Offset(this Polygon poly, int xoffset)
        {
            for (int i = 0; i < poly.Points.Count; i++)
            {
                poly.Points[i] = new Point(poly.Points[i].X + xoffset, poly.Points[i].Y);
            }
            return poly;
        }
    }
}
