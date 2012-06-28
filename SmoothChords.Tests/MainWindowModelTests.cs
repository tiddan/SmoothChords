using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SmoothChords.ViewModel;
using SmoothChords.Data;
using SmoothChords.Model;

namespace SmoothChords.Tests
{
    [TestFixture]
    public class MainWindowModelTests
    {
        [Test]
        public void MainWindowVM_CloseDocument_ClosesDocument()
        {
            var model = new MainWindowModel();
            var document = new DocumentViewModel(model);
            model.Documents.Add(document);
            model.CloseDocument(document);
            Assert.IsTrue(!model.Documents.Contains(document),
                "The Documents collection should not contain this document now.");
        }

        [Test]
        public void MainWindowVM_NewDocument_AddsAndSelectsNewDocument()
        {
            var model = new MainWindowModel();
            model.NewDocument.Execute(null);
            var doc = model.Documents[0];
            var added = model.Documents.Count > 0;
            var selected = (model.DocumentsView.CurrentItem == doc);
            Assert.IsTrue(added, "The document should be added to the collection.");
            Assert.IsTrue(selected, "The document should be selected.");
        }

        [Test]
        public void MainWindowVM_OpenDocument_OpensAndSelectsDocument()
        {
            FakeMainWindowModel model = new FakeMainWindowModel();
            model.OpenDocument.Execute(null);
            var doc = model.Documents[0];
            var opened = (doc.Title == "TestDocument.chords");
            var selected = ((model.DocumentsView.CurrentItem as DocumentViewModel).Title == "TestDocument.chords");
            Assert.IsTrue(opened, "The document should be opened");
            Assert.IsTrue(selected, "The document should be selected");
        }

        [Test]
        public void MainWindowVM_SaveDocument_RunsDataStoreSaveMethod()
        {
            FakeMainWindowModel model = new FakeMainWindowModel();
            model.SaveDocument.Execute(null);
            var saveDidRun = (model.GetDataStore() as FakeDataStore).SaveWasRun;
            Assert.IsTrue(saveDidRun, "Save method should have been runned");
        }
        
        [Test]
        public void MainWindowVM_SaveAsDocument_RunsDataStoreSaveAsMethod()
        {
            FakeMainWindowModel model = new FakeMainWindowModel();
            model.SaveAsDocument.Execute(null);
            var saveAsDidRun = (model.GetDataStore() as FakeDataStore).SaveAsWasRun;
            Assert.IsTrue(saveAsDidRun, "SaveAs method should have been runned");
        }

        [Test]
        public void MainWindowVM_Insert_InsertsChordLine()
        {
            MainWindowModel model = new MainWindowModel();
            var doc = new DocumentViewModel();
            model.Documents.Add(doc);
            model.InsertCommand.Execute(null);
            var selectedDoc = (model.DocumentsView.CurrentItem as DocumentViewModel);
            var lineAdded = (selectedDoc.ChordLines.Count == 1);
            Assert.IsTrue(lineAdded, "The line should have been added.");
        }

        [Test]
        public void MainWindowVM_DeleteChord_RunsDocumentDeleteChordMethod()
        {
            MainWindowModel model = new MainWindowModel();
            var doc = new FakeDocumentViewModel(model);
            model.Documents.Add(doc);
            model.DeleteChord.Execute(null);
            Assert.IsTrue(doc.DeleteSelectedChordWasRun,
                "The method on DocumentVM should have run.");
        }

        [Test]
        public void MainWindowVM_TransposeUp_AllChordsWereTransposed()
        {
            var model = new MainWindowModel();
            var doc = new DocumentViewModel(model);
            var chordLine = new ChordLine();
            var chordA = new StubbChord();
            var chordB = new StubbChord();
            chordLine.Chords.Add(chordA);
            chordLine.Chords.Add(chordB);
            doc.ChordLines.Add(chordLine);
            model.Documents.Add(doc);
            model.TransposeUp.Execute(null);
            var wasTransposed = (chordA.WasTransposedUp && chordB.WasTransposedUp);
            Assert.IsTrue(wasTransposed, "All chords should have been transposed");
        }

        [Test]
        public void MainWindowVM_TransposeDown_AllChordsWereTransposed()
        {
            var model = new MainWindowModel();
            var doc = new DocumentViewModel(model);
            var chordLine = new ChordLine();
            var chordA = new StubbChord();
            var chordB = new StubbChord();
            chordLine.Chords.Add(chordA);
            chordLine.Chords.Add(chordB);
            doc.ChordLines.Add(chordLine);
            model.Documents.Add(doc);
            model.TransposeDown.Execute(null);
            var wasTransposed = (chordA.WasTransposedDown && chordB.WasTransposedDown);
            Assert.IsTrue(wasTransposed, "All chords should have been transposed");
        }

        internal class StubbChord : Chord
        {
            public bool WasTransposedUp = false;
            public bool WasTransposedDown = false;

            public override void Transpose(PianoLibrary.Model.Chords.ChordDefinition.TransposeType mode)
            {
                if (mode == PianoLibrary.Model.Chords.ChordDefinition.TransposeType.Up)
                {
                    WasTransposedUp = true;
                }
                else
                {
                    WasTransposedDown = true;
                }
            }
        }

        internal class FakeMainWindowModel : MainWindowModel
        {

            public FakeMainWindowModel()
            {
                this._dataStore = new FakeDataStore();
            }

            public DataStore GetDataStore()
            {
                return _dataStore;
            }
        }

        internal class FakeDocumentViewModel : DocumentViewModel
        {
            public bool DeleteSelectedChordWasRun = false;

            public override void DeleteSelectedChord()
            {
                DeleteSelectedChordWasRun = true;
            }

            public FakeDocumentViewModel(MainWindowModel model) : base(model) { }
        }

        internal class FakeDataStore : DataStore
        {
            public bool SaveWasRun = false;
            public bool SaveAsWasRun = false;

            public override void Open(ref DocumentViewModel document)
            {
                document = new DocumentViewModel();
                document.DocumentPath = "C:\\TestDocument.chords";
            }

            public override void Save(DocumentViewModel document)
            {
                SaveWasRun = true;
            }

            public override void SaveAs(DocumentViewModel document)
            {
                SaveAsWasRun = true;
            }
        }


    }
}
