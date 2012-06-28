using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace PianoLibrary
{
    public class PianoKey
    {
        public enum KeyTypes  
        {
            A,
            ASharp,
            B,
            C,
            CSharp,
            D,
            DSharp,
            E,
            F,
            FSharp,
            G,
            GSharp
        }

        public KeyTypes KeyType { get; protected set; }
        
        public short Octave { get; protected set; }

        public Polygon Geometry { get; protected set; }


        public PianoKey(KeyTypes keyType, short octave)
        {
            KeyType = keyType;
            Octave = octave;
            Geometry = GenerateGeometry();
        }

        private Polygon GenerateGeometry()
        {
            bool isLeftEdge = false;
            bool isRightEdge = false;

            if(KeyType==KeyTypes.A && Octave == 1) isLeftEdge = true;
            if(KeyType==KeyTypes.C && Octave == 8) isRightEdge = true;

            Polygon p = PianoGeometry.GetKeyShape(KeyType, isLeftEdge, isRightEdge);
            p = p.Offset(PianoGeometry.GetKeyOffset(KeyType));
            p = p.Offset((Octave - 1) * 70);

            return p;
        }

        public static KeyTypes NextKey(KeyTypes key)
        {
            IList<KeyTypes> keys = Enum.GetValues(typeof(KeyTypes)).Cast<KeyTypes>().ToList<KeyTypes>();
            int index = keys.ToList<KeyTypes>().IndexOf(key);
            if (index == keys.Count - 1) return keys.ElementAt<KeyTypes>(0);
            else return keys.ElementAt<KeyTypes>(index + 1);
        }

        public static KeyTypes PrevKey(KeyTypes key)
        {
            IList<KeyTypes> keys = Enum.GetValues(typeof(KeyTypes)).Cast<KeyTypes>().ToList<KeyTypes>();
            int index = keys.ToList<KeyTypes>().IndexOf(key);
            if (index == 0) return keys.ElementAt<KeyTypes>(keys.Count-1);
            else return keys.ElementAt<KeyTypes>(index - 1);
        }

        public void ResetColor()
        {
            if (KeyType == KeyTypes.ASharp ||
                KeyType == KeyTypes.CSharp ||
                KeyType == KeyTypes.DSharp ||
                KeyType == KeyTypes.FSharp ||
                KeyType == KeyTypes.GSharp)
            {
                Geometry.Fill = Brushes.Black;
            }
            else
            {
                Geometry.Fill = Brushes.White;
            }
        }
    }
}
