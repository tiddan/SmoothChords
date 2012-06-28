using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SmoothChords.Model;
using PianoLibrary.Model;
using PianoLibrary.Model.Chords;
using System.Windows.Media;

namespace SmoothChords.Tests
{
    [TestFixture]
    public class ChordTests
    {
        [Test]
        public void LostFocusCommandExecute_DeselectsChord()
        {
            Chord c = new Chord();
            c.IsSelected = true;
            c.LostFocusCommandExecute();
            Assert.IsTrue(c.IsSelected == false, "Chord should have been deselected.");
        }

        [Test]
        public void LostFocusCommandExecute_EditModeFalse()
        {
            Chord c = new Chord();
            c.IsSelected = true;
            c.LostFocusCommandExecute();
            Assert.IsTrue(c.IsEditMode == false, "Chord should not been in Edit mode.");
        }

        [Test]
        public void NameChanged_ValidChord_SetsBorderColorBlack()
        {
            Chord c = new Chord();
            c.Name = "C";
            Assert.IsTrue(c.BorderColor == Brushes.Black, "Border should have been Black.");
        }

        [Test]
        public void NameChanged_ValidChord_AddsNotes()
        {
            Chord c = new Chord();
            c.Name = "C";
            Assert.IsTrue(c.Notes.Count > 0, "There should have been items in the Notes collection.");
        }

        [Test]
        public void Transpose_UpAndDownSameAsOriginal_Simple()
        {
            Chord c = new Chord();
            var chordName = "Cm";
            c.Name = chordName;
            c.Transpose(ChordDefinition.TransposeType.Up);
            c.Transpose(ChordDefinition.TransposeType.Down);
            Assert.IsTrue(c.Name == chordName, "Chord should be equal to the original chord.");
        }

        [Test]
        public void Transpose_UpUpExample()
        {
            Chord c = new Chord();
            c.Name = "C";
            c.Transpose(ChordDefinition.TransposeType.Up);
            c.Transpose(ChordDefinition.TransposeType.Up);
            Assert.IsTrue(c.Name == "D", "'C' double up transposed should be equal to 'D'.");
        }
    }
}
