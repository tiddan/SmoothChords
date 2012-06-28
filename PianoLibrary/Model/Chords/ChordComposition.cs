using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PianoLibrary;
using System.Collections;
using PianoLibrary.Model.Chords.Voicings;

namespace PianoLibrary.Model.Chords
{
    public class ChordComposition
    {
        private Dictionary<string, ChordVoicing> _availableVoicings = null;

        public ChordDefinition[] Chords;

        public string Chord = null;

        public Dictionary<string, ChordVoicing> AvailableVoicings
        {
            get
            {
                if (_availableVoicings == null)
                {
                    _availableVoicings = new Dictionary<string, ChordVoicing>();
                    _availableVoicings.Add("Default voicing", new DefaultVoicing(this));
                }
                return _availableVoicings;
            }
        }

        public bool IsValidNotation()
        {
            foreach (ChordDefinition definition in Chords)
            {
                if (!definition.IsValidNotation()) return false;
            }
            return true;
        }

        public string Transpose(ChordDefinition.TransposeType transposeMode)
        {
            string result = Chord;
            foreach (ChordDefinition chordDef in Chords)
            {
                chordDef.Transpose(transposeMode);
            }
            return ToString();
        }

        public static ChordComposition DecomposeChord(string chord)
        {
            ChordComposition comp = new ChordComposition();

            comp.Chord = chord;

            // Split the chord into left hand and right hand if slash notation in used.
            char[] overToken = { '/' };
            string[] chords = chord.Split(overToken);

            // Find number of chords.
            int numChords = chords.Count();

            // If there is more than two chords then return empty set.
            if (chords.Count() > 2) return comp;

            comp.Chords = new ChordDefinition[numChords];

            for (int i = 0; i < numChords; i++)
            {
                comp.Chords[i] = new ChordDefinition();
                comp.Chords[i].Chord = chords[i];
                comp.Chords[i].DefineChord();
            }

            return comp;

        }

        public override string ToString()
        {
            if (Chords.Count() == 2)
            {
                return Chords[0].ToString() + "/" + Chords[1].ToString();
            }
            else
            {
                return Chords[0].ToString();
            }

        }
    }
}    


