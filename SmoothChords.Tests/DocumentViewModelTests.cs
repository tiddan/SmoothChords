using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SmoothChords.ViewModel;
using SmoothChords.Model;
using SmoothChords.View;
using System.Windows.Input;
using SmoothChords.Player;

namespace SmoothChords.Tests
{
    [TestFixture]
    public class DocumentViewModelTests
    {
        [Test]
        public void DocumentVM_ChordLinesCollectionChanged_UpdatesNewFirstLine()
        {
            DocumentViewModel doc = new DocumentViewModel();
            var line1 = new ChordLine();
            var line2 = new ChordLine();
            var line3 = new ChordLine();
            doc.ChordLines.Add(line1);
            doc.ChordLines.Add(line2);
            doc.ChordLines.Add(line3);

            Assert.IsTrue(line1.FirstLine == true, "Line 1 should now be 'FirstLine'.");
            doc.ChordLines.Remove(line1);
            doc.ChordLines.Remove(line2);
            Assert.IsTrue(line3.FirstLine == true, "Line 3 should now be 'FirstLine'.");
        }

        [Test]
        public void DocumentVM_CM_AddLine_AddsChordLine()
        {
            DocumentViewModel document = new DocumentViewModel();
            document.CM_AddLine.Execute(null);
            Assert.AreEqual(document.ChordLines.Count, 1, "There should be 1 line now.");
        }

        [Test]
        public void DocumentVM_CM_DeleteLine_RemovesChordLine()
        {
            FakeDocumentViewModel document = new FakeDocumentViewModel();
            var line = new ChordLine();
            document.ChordLines.Add(line);
            document.SelectedChordLine = line;
            document.CM_DeleteLine.Execute(null);
            Assert.AreEqual(document.ChordLines.Count, 0, "There should be 0 lines now.");
        }
        
        [Test]
        public void DocumentVM_DeleteSelectedChord_DeletesTheChord()
        {
            FakeDocumentViewModel document = new FakeDocumentViewModel();
            var line = new ChordLine();
            var chord = new Chord();
            line.Chords.Add(chord);
            document.ChordLines.Add(line);
            document.SelectedChord = chord;
            document.DeleteSelectedChord();
            Assert.IsNull(document.SelectedChord, "SelectedChord should be null.");
            Assert.AreEqual(line.Chords.Count, 0, "There should be 0 chords in the line now.");
        }

        [Test]
        public void DocumentVM_PlayCommand_RunsPlayerPlayMetohd()
        {
            FakeDocumentViewModel document = new FakeDocumentViewModel();
            document.PlayCommand.Execute(null);
            var success = (document.GetPlayer() as StubbDocumentPlayer).PlayWasRun;
            Assert.IsTrue(success, "The PlayWasRun should have been true.");
        }

        [Test]
        public void DocumentVM_StopCommand_RunsPlayerStopMetohd()
        {
            FakeDocumentViewModel document = new FakeDocumentViewModel();
            document.StopCommand.Execute(null);
            var success = (document.GetPlayer() as StubbDocumentPlayer).StopWasRun;
            Assert.IsTrue(success, "The StopWasRun should have been true.");
        }

        [Test]
        public void DocumentVM_PauseCommand_RunsPlayerPauseMetohd()
        {
            FakeDocumentViewModel document = new FakeDocumentViewModel();
            document.PauseCommand.Execute(null);
            var success = (document.GetPlayer() as StubbDocumentPlayer).PauseWasRun;
            Assert.IsTrue(success, "The PauseWasRun should have been true.");
        }

        [Test]
        public void DocumentVM_UpdateProgress_SendsStopIfFinished()
        {
            PlayerUpdate update = new PlayerUpdate
            {
                Beat = 4,
                Measure = 5,
                BeatCompleteness = 0
            };
            FakeDocumentViewModel document = new FakeDocumentViewModel();
            document.ChordLines.Add(new ChordLine());
            document.UpdateProgress(update);
            var stopDidRun = (document.GetPlayer() as StubbDocumentPlayer).StopWasRun;
            Assert.IsTrue(stopDidRun, "Stop should have been run, since the player was done playing.");
        }

        [Test]
        public void DocumentVM_SetsTitleWhenDocumentPathIsUpdated()
        {
            DocumentViewModel document = new DocumentViewModel();
            document.Title = "";
            document.DocumentPath = "C:\\MyDocument.chords";
            Assert.AreEqual(document.Title, "MyDocument.chords",
                "Title should be equal to the file name.");
        }

        internal class FakeDocumentViewModel : DocumentViewModel
        {
            StubbDocumentPlayer _player2;

            protected override DocumentPlayer Player
            {
                get
                {
                    if (_player2 == null) _player2 = new StubbDocumentPlayer(this);
                    return _player2;
                }
            }

            public DocumentPlayer GetPlayer() { return Player; }

            public Chord SelectedChord
            {
                get
                {
                    return _selectedChord;
                }
                set
                {
                    _selectedChord = value;
                }
            }

            public ChordLine SelectedChordLine
            {
                get
                {
                    return _selectedChordLine;
                }
                set
                {
                    _selectedChordLine = value;
                }
            }
        }

        internal class StubbDocumentPlayer : DocumentPlayer
        {
            public bool PlayWasRun = false;
            public bool StopWasRun = false;
            public bool PauseWasRun = false;

            public override void Play()
            {
                PlayWasRun = true;
            }

            public override void Stop()
            {
                StopWasRun = true;
            }

            public override void Pause()
            {
                PauseWasRun = true;
            }

            public StubbDocumentPlayer(DocumentViewModel model) : base(model) { }
        }
    }
}
