using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Media;
using PianoLibrary;
using System.Collections.ObjectModel;
using PianoLibrary.Model;
using PianoLibrary.Model.Chords;
using PianoLibrary.Model.Chords.Voicings;

namespace SmoothChords.Model
{
    public class Chord : ModelBase
    {
        // -------------------------------------
        // [VARIABLES]
        // -------------------------------------

        private PianoKey.KeyTypes _chordTonic;
        private ChordDefinition.Modes _chordMode;
        private ObservableCollection<int> _notes = null;

        private bool _isSelected = false;
        private bool _isEditMode = false;
        private string _name = "C";
        private string _margin = "0";
        private RelayCommand _lostFocusCommand;

        private Brush _borderColor;

        // -------------------------------------
        // [PROPERTIES]
        // -------------------------------------

        public PianoKey.KeyTypes ChordTonic
        {
            get
            {
                return _chordTonic;
            }
            set
            {
                _chordTonic = value;
                OnPropertyChanged("ChordTonic");
            }
        }

        public ChordDefinition.Modes ChordMode
        {
            get
            {
                return _chordMode;
            }
            set
            {
                _chordMode = value;
                OnPropertyChanged("ChordMode");
            }
        }

        public ObservableCollection<int> Notes
        {
            get
            {
                if (_notes == null)
                {
                    _notes = new ObservableCollection<int>();
                }
                return _notes;
            }
            set
            {
                _notes = value;
                OnPropertyChanged("Notes");
            }
        }

        // --

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public bool IsEditMode
        {
            get
            {
                return _isEditMode;
            }
            set
            {
                _isEditMode = value;
                OnPropertyChanged("IsEditMode");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
                OnPropertyChanged("Margin");
            }
        }

        public double MarginLeft
        {
            get
            {
                char[] sep = { ',' };
                string firstParam = Margin.Split(sep)[0];
                return Double.Parse(firstParam);
            }
        }

        public Brush BorderColor
        {
            get
            {
                if (_borderColor == null)
                {
                    _borderColor = Brushes.Black;
                }
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                OnPropertyChanged("BorderColor");
            }
        }

        // -------------------------------------
        // [COMMANDS]
        // -------------------------------------

        public ICommand LostFocusCommand
        {
            get
            {
                if (_lostFocusCommand == null)
                {
                    _lostFocusCommand = new RelayCommand(LostFocusCommandExecute);
                }
                return _lostFocusCommand;
            }

        }

        // -------------------------------------
        // [METHODS]
        // -------------------------------------

        public void LostFocusCommandExecute()
        {
            IsSelected = false;
            IsEditMode = false;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Position" || propertyName == "Measure")
            {
                OnPropertyChanged("Margin");
            }

            if (propertyName == "Name")
            {
                if (Name.Count() > 0)
                {
                    ChordComposition comp = ChordComposition.DecomposeChord(Name);
                    if (comp.IsValidNotation())
                    {
                        Notes = comp.AvailableVoicings["Default voicing"].Notes;
                        BorderColor = Brushes.Black;
                    }
                    else
                    {
                        BorderColor = Brushes.Red;
                    }
                }
            }
        }

        public virtual void Transpose(ChordDefinition.TransposeType mode)
        {
            var chord = Name;
            
            var comp = ChordComposition.DecomposeChord(chord);
            for (int i = 0; i < comp.Chords.Count(); i++)
            {
                comp.Transpose(mode);
            }

            Name = comp.ToString();
        }
    }

    
}
