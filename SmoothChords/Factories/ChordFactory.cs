using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothChords.Model;
using PianoLibrary;
using System.Collections.ObjectModel;

namespace SmoothChords.Factories
{
    public class ChordFactory
    {
        public static ObservableCollection<int> Create_DefaultVoicing(PianoKey.KeyTypes keyType, PianoLibrary.Model.Chords.ChordDefinition.Modes mode)
        {
            ObservableCollection<int> notes = new ObservableCollection<int>();

            int offset = Piano.GetOffset(keyType);

            // Add Base (1 at Octave 3)
            notes.Add((12 * 2) + offset);

            // Chord root
            notes.Add((12 * 3 ) + offset);

            // Chord 3rd
            if (mode == PianoLibrary.Model.Chords.ChordDefinition.Modes.Major)
            {
                notes.Add((12 * 3)+ offset + 4);
            }
            else
            {
                notes.Add((12 * 3) + offset + 3);
            }

            // Chord 5th
            notes.Add((12 * 3) + offset + 7);

            return notes;
        }
    }
}
