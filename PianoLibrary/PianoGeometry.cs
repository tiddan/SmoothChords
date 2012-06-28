using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace PianoLibrary
{
    public class PianoGeometry
    {
        private static Polygon NormalKey()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Points.Add(new Point(0, 0));
            p.Points.Add(new Point(10, 0));
            p.Points.Add(new Point(10, 62));
            p.Points.Add(new Point(0, 62));
            p.Fill = Brushes.White;
            return p;
        }

        private static Polygon BlackKey()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Points.Add(new Point(0, 0));
            p.Points.Add(new Point(6, 0));
            p.Points.Add(new Point(6, 39));
            p.Points.Add(new Point(0, 39));
            p.Fill = Brushes.Black;
            return p;
        }

        private static Polygon LeftKey()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Points.Add(new Point(0, 0));
            p.Points.Add(new Point(7, 0));
            p.Points.Add(new Point(7, 39));
            p.Points.Add(new Point(10, 39));
            p.Points.Add(new Point(10, 62));
            p.Points.Add(new Point(0, 62));
            p.Fill = Brushes.White;
            return p;
        }

        private static Polygon MiddleKey()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Points.Add(new Point(0, 39));
            p.Points.Add(new Point(2, 39));
            p.Points.Add(new Point(2, 0));
            p.Points.Add(new Point(8, 0));
            p.Points.Add(new Point(8, 39));
            p.Points.Add(new Point(10, 39));
            p.Points.Add(new Point(10, 62));
            p.Points.Add(new Point(0, 62));
            p.Fill = Brushes.White;
            return p;
        }

        private static Polygon G()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Points.Add(new Point(0, 39));
            p.Points.Add(new Point(2, 39));
            p.Points.Add(new Point(2, 0));
            p.Points.Add(new Point(7, 0));
            p.Points.Add(new Point(7, 39));
            p.Points.Add(new Point(10, 39));
            p.Points.Add(new Point(10, 62));
            p.Points.Add(new Point(0, 62));
            p.Fill = Brushes.White;
            return p;
        }

        private static Polygon A()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Points.Add(new Point(0, 39));
            p.Points.Add(new Point(3, 39));
            p.Points.Add(new Point(3, 0));
            p.Points.Add(new Point(8, 0));
            p.Points.Add(new Point(8, 39));
            p.Points.Add(new Point(10, 39));
            p.Points.Add(new Point(10, 62));
            p.Points.Add(new Point(0, 62));
            p.Fill = Brushes.White;
            return p;
        }

        private static Polygon RightKey()
        {
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Points.Add(new Point(0, 39));
            p.Points.Add(new Point(4, 39));
            p.Points.Add(new Point(4, 0));
            p.Points.Add(new Point(10, 0));
            p.Points.Add(new Point(10, 62));
            p.Points.Add(new Point(0, 62));
            p.Fill = Brushes.White;
            return p;
        }

        public static Polygon GetKeyShape(PianoKey.KeyTypes keyType, bool isLeftEdge, bool isRightEdge)
        {
            // White 10 width
            // Black 6 with
            // black 4 inside the white in the outside. (...?)

            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;

            if (isLeftEdge)
            {
                return LeftKey();
            }
            if (isRightEdge)
            {
                return NormalKey();
            }

            switch (keyType)
            {
                case PianoKey.KeyTypes.C:
                    {
                        return LeftKey();
                    }
                case PianoKey.KeyTypes.D:
                    {
                        return MiddleKey();
                    }
                case PianoKey.KeyTypes.E:
                    {
                        return RightKey();
                    }
                case PianoKey.KeyTypes.F:
                    {
                        return LeftKey();
                    }
                case PianoKey.KeyTypes.G:
                    {
                        return G();
                    }
                case PianoKey.KeyTypes.A:
                    {
                        return A();
                    }
                case PianoKey.KeyTypes.B:
                    {
                        return RightKey();
                    }
                default:
                    {
                        return BlackKey();
                    }

            }


        }

        public static int GetKeyOffset(PianoKey.KeyTypes keyType)
        {
            switch (keyType)
            {
                case PianoKey.KeyTypes.A:
                    {
                        return 0;
                    }
                case PianoKey.KeyTypes.ASharp:
                    {
                        return 8;
                    }
                case PianoKey.KeyTypes.B:
                    {
                        return 10;
                    }
                case PianoKey.KeyTypes.C:
                    {
                        return 20;
                    }
                case PianoKey.KeyTypes.CSharp:
                    {
                        return 26;
                    }
                case PianoKey.KeyTypes.D:
                    {
                        return 30;
                    }
                case PianoKey.KeyTypes.DSharp:
                    {
                        return 38;
                    }
                case PianoKey.KeyTypes.E:
                    {
                        return 40;
                    }
                case PianoKey.KeyTypes.F:
                    {
                        return 50;
                    }
                case PianoKey.KeyTypes.FSharp:
                    {
                        return 56;
                    }
                case PianoKey.KeyTypes.G:
                    {
                        return 60;
                    }
                case PianoKey.KeyTypes.GSharp:
                    {
                        return 67;
                    }
                default:
                    {
                        return 10000;
                    }
            }
        }
    }
}
