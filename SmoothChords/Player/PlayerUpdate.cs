using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmoothChords.Player
{
    public class PlayerUpdate
    {
        public int Measure { get; set; }
        public int Beat { get; set; }
        public double BeatCompleteness { get; set; }

        public override string ToString()
        {
            return "Measure: " + Measure + " - Beat: " + Beat + " - BeatPercent: " + BeatCompleteness ;
        }
    }
}
