using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SmoothChords.Data;
using SmoothChords.ViewModel;
using SmoothChords.Model;

namespace SmoothChords.Tests
{
    [TestFixture]
    public class DataStoreTests
    {
        [Test]
        public void DataStore_Save_WithPathSavesFile()
        {
            DocumentViewModel document = new DocumentViewModel();
            document.DocumentPath = "C:\\MyDocument.chords";

            StubbDataStore dataStore = new StubbDataStore();
            dataStore.Save(document);
            Assert.IsTrue(dataStore.DidSerialize, 
                "File should have been saved, but was not.");
        }

        [Test]
        public void DataStore_Save_WithoutPath_RunsSaveAsAndSaves()
        {
            DocumentViewModel document = new DocumentViewModel();
            StubbDataStore dataStore = new StubbDataStore();
            dataStore.Save(document);
            Assert.IsTrue(dataStore.SaveAsRun && dataStore.DidSerialize,
                "Both 'SaveAsRun' and 'DidSerialize' should be true.");
        }

        [Test]
        public void DataStore_SaveAs_SavesFile()
        {
            DocumentViewModel document = new DocumentViewModel();
            StubbDataStore dataStore = new StubbDataStore();
            dataStore.SaveAs(document);
            Assert.IsTrue(dataStore.DidSerialize,
                "File should have been saved, but was not.");
        }

        [Test]
        public void DataStore_Open_OpensTestDocument()
        {
            DocumentViewModel document = new DocumentViewModel();
            StubbDataStore dataStore = new StubbDataStore();
            dataStore.Open(ref document);
            Assert.IsTrue(document.ChordLines.Count == 1,
                "There should be 1 line in the document");
        }
    }

    internal class StubbDataStore : DataStore
    {
        public bool DidSerialize = false;
        public bool SaveAsRun = false;

        protected override void SerializeAsFile(ViewModel.DocumentViewModel document)
        {
            DidSerialize = true;
        }

        public override void SaveAs(DocumentViewModel document)
        {
            base.SaveAs(document);
            SaveAsRun = true;
        }

        public override void Open(ref DocumentViewModel document)
        {
            var line = new ChordLine();
            line.Chords.Add(new Chord{ Name = "C"});
            line.Chords.Add(new Chord{ Name = "F"});
            line.Chords.Add(new Chord{ Name = "G"});
            document.ChordLines.Add(line);
            document.DocumentPath = "C:\\MyDocument.chords";
        }

        public StubbDataStore()
        {
            TEST_MODE = true;
        }
    }
}
