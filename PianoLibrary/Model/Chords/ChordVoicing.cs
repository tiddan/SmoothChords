using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PianoLibrary.Model.Chords
{
    public abstract class ChordVoicing
    {
        public ObservableCollection<int> _notes = null;

        public ObservableCollection<int> Notes
        {
            get
            {
                if (_notes == null)
                {
                    _notes = Compose();
                }
                return _notes;
            }
            set
            {
                _notes = value;
            }
        }

        public ChordComposition Composition = null;

        public ChordVoicing(ChordComposition composition)
        {
            Composition = composition;
        }

        public abstract ObservableCollection<int> Compose();
    }
}
