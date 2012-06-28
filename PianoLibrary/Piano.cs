using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PianoLibrary
{
    public class Piano
    {
        public static int OFFSET_3RD = 4;
        public static int OFFSET_5TH = 7;
        public static int OFFSET_6TH = 9;
        public static int OFFSET_7TH = 10;
        public static int OFFSET_9TH = 2;
        public static int OFFSET_11TH = 5;
        public static int OFFSET_13TH = 9;

        public PianoKey[] Keys = new PianoKey[88];

        public Piano()
        {
            GeneratePiano();
        }

        protected virtual void GeneratePiano()
        {
            PianoKey.KeyTypes currentKey = PianoKey.KeyTypes.A;
            short octave = 0;

            for (int i = 0; i < 88; i++)
            {
                if (currentKey == PianoKey.KeyTypes.A) { octave++; }
                Keys[i] = new PianoKey(currentKey, octave);
                currentKey = PianoKey.NextKey(currentKey);
            }
        }

        public static int GetOffset(PianoKey.KeyTypes key)
        {
            IList<PianoKey.KeyTypes> keys = Enum.GetValues(typeof(PianoKey.KeyTypes)).Cast<PianoKey.KeyTypes>().ToList<PianoKey.KeyTypes>();
            return keys.ToList<PianoKey.KeyTypes>().IndexOf(key);
        }

        public static int GetOffsetFromTonic(int extention)
        {
            switch (extention)
            {
                case 5:
                    {
                        return Piano.OFFSET_5TH;
                    }
                case 6:
                    {
                        return Piano.OFFSET_6TH;
                    }
                case 7:
                    {
                        return Piano.OFFSET_7TH;
                    }
                case 9:
                    {
                        return Piano.OFFSET_9TH;
                    }
                case 11:
                    {
                        return Piano.OFFSET_11TH;
                    }
                case 13:
                    {
                        return Piano.OFFSET_13TH;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }


        
    }
}
