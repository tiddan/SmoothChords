using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Media;
using SmoothChords.Helpers;
using SmoothChords.View;

namespace SmoothChords.Model
{
    public class ChordLine :  ModelBase
    {
        // -------------------------------------
        // [VARIABLES]
        // -------------------------------------

        private ObservableCollection<Chord> _chords = null;
        private Boolean _firstLine = true;

        // -------------------------------------
        // [PROPERTIS]
        // -------------------------------------

        public ObservableCollection<Chord> Chords
        {
            get
            {
                if (_chords == null)
                {
                    _chords = new ObservableCollection<Chord>();
                }
                return _chords;
            }
            set
            {
                _chords = value;
                OnPropertyChanged("Chords");
            }
        }

        public bool FirstLine
        {
            get
            {
                return _firstLine;
            }
            set
            {
                _firstLine = value;
                OnPropertyChanged("FirstLine");
            }
        }
    }
}
