using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SmoothChords.Model;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using SmoothChords.View;
using SmoothChords.Helpers;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using SmoothChords.Player;
using System.Windows.Documents;
using SmoothChords.Components;

namespace SmoothChords.ViewModel
{
    public class DocumentViewModel : MyViewModelBase
    {
        // -------------------------------------
        // [VARIABLES]
        // -------------------------------------

        private ObservableCollection<ChordLine> _chordLines = null;
        private ICollectionView _chordLinesView = null;
        private string _title = "Untitled.chords";

        //private int _topOffset = 25;

        private RelayCommand<MouseButtonEventArgs> _mouseDoubleClickCommand = null;
        private RelayCommand<MouseEventArgs> _mouseMoveCommand = null;
        private RelayCommand<MouseEventArgs> _mouseLeftDownCommand = null;
        private RelayCommand<MouseEventArgs> _mouseLeftUpCommand = null;
        private RelayCommand<MouseEventArgs> _mouseRightDownCommand = null;
        private RelayCommand<RoutedEventArgs> _closeButtonClicked = null;

        private RelayCommand<RoutedEventArgs> _cm_AddLine = null;
        private RelayCommand<RoutedEventArgs> _cm_DeleteLine = null;

        private RelayCommand _playCommand = null;
        private RelayCommand _stopCommand = null;
        private RelayCommand _pauseCommand = null;

        protected ChordView _selectedChordView = null;
        protected Chord _selectedChord = null;

        protected ChordLineView _selectedChordLineView = null;
        protected ChordLine _selectedChordLine = null;

        private IMainWindowModel _mainWindowModel = null;

        private DocumentPlayer _player = null;

        private string _documentPath = "";
        private string _tempo = "";
        private int _currentMeasure = 1;
        private int _currentBeat = 1;

        private string _markerMargin = "0";

        // -------------------------------------
        // [PROPERTIES]
        // -------------------------------------

        public ObservableCollection<ChordLine> ChordLines
        {
            get
            {
                if (_chordLines == null)
                {
                    _chordLines = new ObservableCollection<ChordLine>();
                }

                return _chordLines;
            }
            set
            {
                _chordLines = value;
                OnPropertyChanged("ChordLines");
            }
        }

        public ICollectionView ChordLinesView
        {
            get
            {
                if (_chordLinesView == null)
                {
                    _chordLinesView = CollectionViewSource.GetDefaultView(ChordLines);
                }
                return _chordLinesView;
            }
        }

        // --

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string DocumentPath
        {
            get
            {
                return _documentPath;
            }
            set
            {
                _documentPath = value;
                OnPropertyChanged("DocumentPath");
            }
        }

        public string Tempo
        {
            get
            {
                if(_tempo=="") _tempo = "120";
                return _tempo;
            }
            set
            {
                _tempo = value;
                OnPropertyChanged("Tempo");
            }
        }

        public int TempoInt
        {
            get
            {
                return Int32.Parse(Tempo);
            }
        }

        // --

        public int CurrentMeasure
        {
            get
            {
                return _currentMeasure;
            }
            set
            {
                _currentMeasure = value;
                OnPropertyChanged("CurrentMeasure");
            }
        }

        public int CurrentBeat
        {
            get
            {
                return _currentBeat;
            }
            set
            {
                _currentBeat = value;
                System.Media.SystemSounds.Asterisk.Play();
                OnPropertyChanged("CurrentBeat");
            }
        }

        public int NumberOfMeasures
        {
            get
            {
                return (ChordLines.Count * 4);
            }
        }

        // -- Marker

        public string MarkerMargin
        {
            get
            {
                return _markerMargin;
            }
            set
            {
                _markerMargin = value;
                OnPropertyChanged("MarkerMargin");
            }
        }

        // -- Player

        protected virtual DocumentPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new DocumentPlayer(this);
                }
                return _player;

            }
        }
       
        // -- Document

        // -------------------------------------
        // [COMMANDS]
        // -------------------------------------

        public ICommand MouseDoubleClickCommand
        {
            get
            {
                if (_mouseDoubleClickCommand == null)
                {
                    _mouseDoubleClickCommand = new RelayCommand<MouseButtonEventArgs>(MouseDoubleClickCommandExecute);
                }
                return _mouseDoubleClickCommand;
            }
        }

        public ICommand MouseMoveCommand
        {
            get
            {
                if (_mouseMoveCommand == null)
                {
                    _mouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMoveCommandExecute);
                }
                return _mouseMoveCommand;
            }
        }

        public ICommand MouseLeftDownCommand
        {
            get
            {
                if (_mouseLeftDownCommand == null)
                {
                    _mouseLeftDownCommand = new RelayCommand<MouseEventArgs>(MouseLeftDownCommandExecute);
                }
                return _mouseLeftDownCommand;
            }
        }

        public ICommand MouseLeftUpCommand
        {
            get
            {
                if (_mouseLeftUpCommand == null)
                {
                    _mouseLeftUpCommand = new RelayCommand<MouseEventArgs>(MouseLeftUpCommandExecute);
                }
                return _mouseLeftUpCommand;
            }
        }

        public ICommand MouseRightDownCommand
        {
            get
            {
                if (_mouseRightDownCommand == null)
                {
                    _mouseRightDownCommand = new RelayCommand<MouseEventArgs>(MouseRightDownCommandExecute);
                }
                return _mouseRightDownCommand;
            }
        }

        public ICommand CloseButtonClicked
        {
            get
            {
                if (_closeButtonClicked == null)
                {
                    _closeButtonClicked = new RelayCommand<RoutedEventArgs>(CloseButtonClickedExecute);
                }
                return _closeButtonClicked;
            }
        }

        // --

        public ICommand CM_AddLine
        {
            get
            {
                if (_cm_AddLine == null)
                {
                    _cm_AddLine = new RelayCommand<RoutedEventArgs>(CM_AddLineExecute);
                }
                return _cm_AddLine;
            }
        }

        public ICommand CM_DeleteLine
        {
            get
            {
                if (_cm_DeleteLine == null)
                {
                    _cm_DeleteLine = new RelayCommand<RoutedEventArgs>(CM_DeleteLineExecute);
                }
                return _cm_DeleteLine;
            }
        }

        // --

        public ICommand PlayCommand
        {
            get
            {
                if (_playCommand == null)
                {
                    _playCommand = new RelayCommand(PlayCommandExecute);
                }
                return _playCommand;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new RelayCommand(StopCommandExecute);
                }
                return _stopCommand;
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                if (_pauseCommand == null)
                {
                    _pauseCommand = new RelayCommand(PauseCommandExecute);
                }
                return _pauseCommand;
            }
        }

        // -------------------------------------
        // [CONSTRUCTOR]
        // -------------------------------------

        public DocumentViewModel(IMainWindowModel mainWindowModel)
        {
            _mainWindowModel = mainWindowModel;
            ChordLinesView.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ChordLinesView_CollectionChanged);
        }

        public DocumentViewModel()
        {
            ChordLinesView.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ChordLinesView_CollectionChanged);
        }

        protected virtual void ChordLinesView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ChordLines.Count > 0)
            {
                foreach (ChordLine line in ChordLines)
                {
                    line.FirstLine = false;
                }
                ChordLines[0].FirstLine = true;
            }
        }

        // -------------------------------------
        // [METHODS]
        // -------------------------------------

        // -- Mouse Methods (Not testable)

        private void MouseDoubleClickCommandExecute(MouseButtonEventArgs param)
        {
            // Get Mouse position.
            Point p = param.GetPosition(param.Source as Control);

            // Find highest level control that was hit.
            var res = VisualTreeHelper.HitTest(param.Source as Control, p);

            // If we did hit something.
            if (res.VisualHit is DependencyObject && res.VisualHit != null)
            {
                var result = Ancestors.FindAncestorOfType<ChordLineView>(res.VisualHit as DependencyObject);
                if (result != null)
                {
                    var chord_result = Ancestors.FindAncestorOfType<ChordView>(res.VisualHit as DependencyObject);
                    if (chord_result == null)
                    {
                        Chord newChord = new Chord();
                        newChord.Margin = (int)(Math.Round(p.X) - 15) + ",0,0,0";

                        ((result as ChordLineView).DataContext as ChordLine).Chords.Add(newChord);
                        newChord.IsEditMode = true;
                        newChord.IsSelected = true;
                    }
                    else
                    {
                        ((chord_result as ChordView).DataContext as Chord).IsEditMode = true;
                        (chord_result as ChordView).FocusAndSelect();
                    }
                }
            }
        }

        private void MouseMoveCommandExecute(MouseEventArgs param)
        {
            if (param.LeftButton == MouseButtonState.Pressed)
            {
                if (_selectedChord != null)
                {
                    Point p = param.GetPosition(param.Source as UIElement);

                    var res = VisualTreeHelper.HitTest(param.Source as UIElement, p);
                    var line = Ancestors.FindAncestorOfType<ChordLineView>(res.VisualHit as UIElement);
                    if (line == null || ((line as ChordLineView).DataContext as ChordLine).Chords.Contains(_selectedChord))
                    {
                        Console.WriteLine(p.Y);

                        double NewX = (Math.Round(p.X) - 15);
                        NewX = Math.Max(NewX, 130);
                        NewX = Math.Min(NewX, 740);

                        _selectedChord.Margin = (int)NewX + ",0,0,0";
                    }
                    else
                    {
                        var ancestorList = Ancestors.FindAncestorOfType<ItemsControl>(_selectedChordView);

                        if(ancestorList!=null)
                        {
                            ((ancestorList as ItemsControl).DataContext as ChordLine).Chords.Remove(_selectedChord);
                            ((line as ChordLineView).DataContext as ChordLine).Chords.Add(_selectedChord);
                        }
                    }
                }
            }
        }

        private void MouseLeftDownCommandExecute(MouseEventArgs param)
        {
            Point p = param.GetPosition(param.Source as UIElement);
            var res = VisualTreeHelper.HitTest(param.Source as UIElement, p);
            if (res.VisualHit is DependencyObject && res.VisualHit != null)
            {
                var result = Ancestors.FindAncestorOfType<ChordView>(res.VisualHit as DependencyObject);
                if (result != null)
                {
                    foreach (ChordLine line in ChordLines)
                    {
                        foreach (Chord chord in line.Chords)
                        {
                            if (chord.IsSelected) chord.IsSelected = false;
                        }
                    }

                    // Set selected chord:

                    _selectedChordView = (result as ChordView);
                    _selectedChord = ((result as ChordView).DataContext as Chord);
                    _selectedChord.IsSelected = true;

                    _mainWindowModel.NotesPlayed = _selectedChord.Notes;

                }
                else
                {
                    _selectedChordView = null;
                    if (_selectedChord != null)
                    {
                        _selectedChord.IsSelected = false;
                    }
                    _selectedChord = null;

                    _mainWindowModel.NotesPlayed = null;
                }
            }
            else
            {
                Console.WriteLine();
            }
        }

        private void MouseLeftUpCommandExecute(MouseEventArgs param)
        {
            if (_selectedChordView != null)
            {
                _selectedChordView.FocusAndSelect();
            }
        }

        private void MouseRightDownCommandExecute(MouseEventArgs param)
        {
            Point p = param.GetPosition(param.Source as UIElement);
            var res = VisualTreeHelper.HitTest(param.Source as UIElement, p);
            var chordLine = Ancestors.FindAncestorOfType<ChordLineView>(res.VisualHit);
            if (chordLine != null)
            {
                _selectedChordLineView = chordLine as ChordLineView;
                _selectedChordLine = (_selectedChordLineView.DataContext as ChordLine);
            }
            else
            {
                _selectedChordLineView = null;
                _selectedChordLine = null;
            }

        }

        private void CloseButtonClickedExecute(RoutedEventArgs param)
        {
            _mainWindowModel.CloseDocument(this);
        }

        // -- Context Menu Methods

        private void CM_AddLineExecute(RoutedEventArgs param)
        {
            ChordLines.Add(new ChordLine());
        }

        private void CM_DeleteLineExecute(RoutedEventArgs param)
        {
            if (_selectedChordLine!=null)
            {
                ChordLines.Remove(_selectedChordLine);
            }
        }

        public virtual void DeleteSelectedChord()
        {
            if (_selectedChord != null)
            {
                ChordLine line = ChordLines.First<ChordLine>(f => f.Chords.Contains(_selectedChord));
                if (line != null)
                {
                    line.Chords.Remove(_selectedChord);
                    _selectedChord = null;
                }
            }
        }

        // -- Document Toolbar methods

        private void PlayCommandExecute()
        {
            Player.Play();
        }

        private void StopCommandExecute()
        {
            Player.Stop();
        }

        private void PauseCommandExecute()
        {
            Player.Pause();
        }

        // -- Other methods

        public virtual void UpdateProgress(PlayerUpdate update)
        {
            if (update.Measure > NumberOfMeasures)
            {
                Player.Stop();
            }
            else
            {
                //if (update.Beat != CurrentBeat) CurrentBeat = update.Beat;
                //if (update.Measure != CurrentMeasure) CurrentMeasure = update.Measure;

                //// Update marker position.
                //int leftOffset = 30 + (((update.Measure % 4) - 1) * 160) + (update.Beat * 40) + (int)Math.Round((update.BeatCompleteness * 40));
                //int topOffset = 20 + (update.Measure / 4 ) * 70;
                //MarkerMargin = leftOffset + "," + topOffset + ",0,0";
            }
        }

        public virtual void SetMainWindowModel(IMainWindowModel mainModel)
        {
            _mainWindowModel = mainModel;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "DocumentPath")
            {
                char[] sep = { '\\' };
                string[] parts = DocumentPath.Split(sep);
                Title = parts[parts.Count() - 1];
            }
        }
    }
}
