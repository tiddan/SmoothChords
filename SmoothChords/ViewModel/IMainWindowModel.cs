using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SmoothChords.ViewModel
{
    public interface IMainWindowModel
    {
        void CloseDocument(DocumentViewModel doc);
        ObservableCollection<int> NotesPlayed { get; set; }
    }
}
