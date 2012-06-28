using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using SmoothChords.Model;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using SmoothChords.Data;
using SmoothChords.Dialogs;

namespace SmoothChords.ViewModel
{
    public class MainWindowModel : MyViewModelBase, IMainWindowModel
    {
        // -------------------------------------
        // [VARIABLES]
        // -------------------------------------

        private ObservableCollection<DocumentViewModel> _documents = null;
        private ObservableCollection<int> _notesPlayed = null;
        private ICollectionView _documentsView = null;

        private RelayCommand _newDocument = null;
        private RelayCommand _openDocument = null;
        private RelayCommand _saveDocument = null;
        private RelayCommand _saveAsDocument = null;
        private RelayCommand _exit = null;
        private RelayCommand _print = null;
        private RelayCommand _insertCommand = null;
        private RelayCommand _deleteChord = null;

        private RelayCommand _transposeUp = null;
        private RelayCommand _transposeDown = null;

        protected DataStore _dataStore = new DataStore();

        // -------------------------------------
        // [PROPERTIES]
        // -------------------------------------

        public ObservableCollection<DocumentViewModel> Documents
        {
            get
            {
                if (_documents == null)
                {
                    _documents = new ObservableCollection<DocumentViewModel>();
                }
                return _documents;
            }
        }

        public ObservableCollection<int> NotesPlayed
        {
            get
            {
                if (_notesPlayed == null)
                {
                    _notesPlayed = new ObservableCollection<int>();
                }
                return _notesPlayed;
            }
            set
            {
                _notesPlayed = value;
                OnPropertyChanged("NotesPlayed");
            }
        }

        public ICollectionView DocumentsView
        {
            get
            {
                if (_documentsView == null)
                {
                    _documentsView = CollectionViewSource.GetDefaultView(Documents);
                }
                return _documentsView;
            }
        }

        // -------------------------------------
        // [CONSTRUCTOR]
        // -------------------------------------

        public MainWindowModel()
        {
        }

        // -------------------------------------
        // [COMMANDS]
        // -------------------------------------

        public ICommand NewDocument
        {
            get
            {
                if (_newDocument == null)
                {
                    _newDocument = new RelayCommand(NewDocumentExecute);
                }
                return _newDocument;
            }
        }

        public ICommand OpenDocument
        {
            get
            {
                if (_openDocument == null)
                {
                    _openDocument = new RelayCommand(OpenDocumentExecute);
                }
                return _openDocument;
            }
        }

        public ICommand SaveDocument
        {
            get
            {
                if (_saveDocument == null)
                {
                    _saveDocument = new RelayCommand(SaveDocumentExecute);
                }
                return _saveDocument;
            }
        }

        public ICommand SaveAsDocument
        {
            get
            {
                if (_saveAsDocument == null)
                {
                    _saveAsDocument = new RelayCommand(SaveAsDocumentExecute);
                }
                return _saveAsDocument;
            }

        }

        public ICommand Exit
        {
            get
            {
                if (_exit == null)
                {
                    _exit = new RelayCommand(ExitExecute);
                }
                return _exit;
            }
        }
        
        public ICommand Print
        {
            get
            {
                if (_print == null)
                {
                    _print = new RelayCommand(PrintExecute);
                }
                return _print;
            }
        }

        public ICommand InsertCommand
        {
            get
            {
                if (_insertCommand == null)
                {
                    _insertCommand = new RelayCommand(InsertCommandExecute);
                }
                return _insertCommand;
            }
        }

        public ICommand DeleteChord
        {
            get
            {
                if (_deleteChord == null)
                {
                    _deleteChord = new RelayCommand(DeleteChordExecute);
                }
                return _deleteChord;
            }
        }

        public ICommand TransposeUp
        {
            get
            {
                if (_transposeUp == null)
                {
                    _transposeUp = new RelayCommand(TransposeUpExecute);
                }
                return _transposeUp;
            }
        }

        public ICommand TransposeDown
        {
            get
            {
                if (_transposeDown == null)
                {
                    _transposeDown = new RelayCommand(TransposeDownExecute);
                }
                return _transposeDown;
            }
        }

        // -------------------------------------
        // [MEMBERS]
        // -------------------------------------

        public void CloseDocument(DocumentViewModel doc)
        {
            Documents.Remove(doc);
        }

        protected void NewDocumentExecute()
        {
            DocumentViewModel newDocument = new DocumentViewModel(this);
            Documents.Add(newDocument);
            DocumentsView.MoveCurrentTo(newDocument);
        }

        public void OpenDocumentExecute()
        {
            DocumentViewModel newDocument = new DocumentViewModel(this);
            _dataStore.Open(ref newDocument);
            if (newDocument.DocumentPath != "")
            {
                Documents.Add(newDocument);
                newDocument.SetMainWindowModel(this);
                DocumentsView.MoveCurrentTo(newDocument);
            }
        }

        public void SaveDocumentExecute()
        {
            _dataStore.Save(DocumentsView.CurrentItem as DocumentViewModel);
        }

        public void SaveAsDocumentExecute()
        {
            _dataStore.SaveAs(DocumentsView.CurrentItem as DocumentViewModel);
        }

        public void ExitExecute()
        {

        }

        public void PrintExecute()
        {
            PrintWindow dialog = new PrintWindow(DocumentsView.CurrentItem as DocumentViewModel);
            if (dialog.ShowDialog() == true)
            {

            }
        }

        public void InsertCommandExecute()
        {
            (DocumentsView.CurrentItem as DocumentViewModel).CM_AddLine.Execute(null);
        }

        public void DeleteChordExecute()
        {
            var doc = (DocumentsView.CurrentItem as DocumentViewModel);
            doc.DeleteSelectedChord();
        }

        public void TransposeUpExecute()
        {
            var doc = (DocumentsView.CurrentItem as DocumentViewModel);
            foreach (var line in doc.ChordLines)
            {
                foreach (var chord in line.Chords)
                {
                    chord.Transpose(PianoLibrary.Model.Chords.ChordDefinition.TransposeType.Up);
                }
            }
        }

        public void TransposeDownExecute()
        {
            var doc = (DocumentsView.CurrentItem as DocumentViewModel);
            foreach (var line in doc.ChordLines)
            {
                foreach (var chord in line.Chords)
                {
                    chord.Transpose(PianoLibrary.Model.Chords.ChordDefinition.TransposeType.Down);
                }
            }
        }

    }
}