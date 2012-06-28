using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SmoothChords.Player;
using SmoothChords.ViewModel;
using SmoothChords.Model;

namespace SmoothChords.Tests
{
    [TestFixture]
    public class DocumentPlayerTests
    {
        [Test]
        public void DocumentPlayer_WorkerThread_UpdatesCorrectly()
        {
            DocumentViewModel document = new DocumentViewModel();
            document.ChordLines.Add(new ChordLine());
            document.Tempo = "120";

            StubbDocumentPlayer player = new StubbDocumentPlayer(document);
            player.RunTime = new TimeSpan(0, 0, 0, 1,100);
            player.IsPlaying = true;
            player.BeatSpan = TimeSpan.FromSeconds(player.GetBeatDurationAsSeconds(document.TempoInt));
            player.BeatPeriod = TimeSpan.FromSeconds(player.GetBeatDurationAsSeconds(document.TempoInt));
            player.ExecuteDoWork();
            Assert.AreEqual(1, player.Measure);
            Assert.AreEqual(3, player.Beat);
        }

        [Test]
        public void DocumentPlayer_GetBeatDurationAsSeconds_120bpm()
        {
            DocumentViewModel document = new DocumentViewModel();
            DocumentPlayer player = new DocumentPlayer(document);
            var result = player.GetBeatDurationAsSeconds(120);
            Assert.AreEqual(result, 0.5,
                "120 bps should correspond to 0.5sec pr beat");
        }

        [Test]
        public void DocumentPlayer_GetBeatDurationAsSeconds_80bpm()
        {
            DocumentViewModel document = new DocumentViewModel();
            DocumentPlayer player = new DocumentPlayer(document);
            var result = player.GetBeatDurationAsSeconds(80);
            Assert.AreEqual(result, 0.75,
                "120 bps should correspond to 0.75sec pr beat");
        }

        [Test]
        public void DocumentPlayer_Play_SetsIsPlayingTrue()
        {
            DocumentViewModel document = new DocumentViewModel();
            FakeDocumentPlayer player = new FakeDocumentPlayer(document);
            player.Play();
            Assert.AreEqual(player.IsPlaying, true,
                "IsPlaying should be 'true'.");
        }

        [Test]
        public void DocumentPlayer_Play_SetsIsPausedFalse()
        {
            DocumentViewModel document = new DocumentViewModel();
            FakeDocumentPlayer player = new FakeDocumentPlayer(document);
            player.Play();
            Assert.AreEqual(player.IsPaused, false,
                "IsPlused should be 'false'.");
        }

        [Test]
        public void DocumentPlayer_Stop_SetsIsPlayingAndIsPausedToFalse()
        {
            DocumentViewModel document = new DocumentViewModel();
            FakeDocumentPlayer player = new FakeDocumentPlayer(document);
            player.Stop();

            Assert.AreEqual(player.IsPaused, false,
                "IsPlused should be 'false'.");
            Assert.AreEqual(player.IsPlaying, false,
                "IsPlaying should be 'false'.");
        }

        [Test]
        public void DocumentPlayer_Pause_SetsIsPausedTrue()
        {
            DocumentViewModel document = new DocumentViewModel();
            FakeDocumentPlayer player = new FakeDocumentPlayer(document);
            player.Pause();

            Assert.AreEqual(player.IsPaused, true,
                "IsPlused should be 'true'.");
        }

        [Test]
        public void DocumentPlayer_Pause_SetsIsPlayingFalse()
        {
            DocumentViewModel document = new DocumentViewModel();
            FakeDocumentPlayer player = new FakeDocumentPlayer(document);
            player.Pause();

            Assert.AreEqual(player.IsPlaying, false,
                "IsPlaying should be 'false'.");
        }

        [Test]
        public void DocumentPlayer_WorkerThread_ReportsToDocument()
        {
            StubbDocumentViewModel document = new StubbDocumentViewModel();
            StubbDocumentPlayer player = new StubbDocumentPlayer(document);
            player.IsPlaying = true;
            player.BeatSpan = TimeSpan.FromSeconds(player.GetBeatDurationAsSeconds(document.TempoInt));
            player.BeatPeriod = TimeSpan.FromSeconds(player.GetBeatDurationAsSeconds(document.TempoInt));
            player.ExecuteDoWork();
            Assert.IsTrue(document.HasBeenUpdated, "Document should have been updated.");
        }

    }

    internal class StubbDocumentPlayer : DocumentPlayer
    {
        public TimeSpan RunTime = new TimeSpan();
        public DateTime Stamp;

        public bool IsPlaying
        {
            set
            {
                _isPlaying = value;
            }
        }

        public TimeSpan BeatPeriod
        {
            set
            {
                _beatPeriod = value;
            }
        }

        public TimeSpan BeatSpan
        {
            set
            {
                _beatSpan = value;
            }
        }

        protected override void WorkerCycle(DateTime now, DateTime prev)
        {
            if (DateTime.Now - Stamp >= RunTime)
            {
                _isPlaying = false;
                _isPaused = true;
            }

            base.WorkerCycle(now, prev);
        }

        public void ExecuteDoWork()
        {
            _bgw_DoWork(this, null);
        }

        protected override void _bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Stamp = DateTime.Now;
            base._bgw_DoWork(sender, e);
        }

        public StubbDocumentPlayer(DocumentViewModel doc)
            : base(doc)
        {
        }
            

    }

    internal class FakeDocumentPlayer : DocumentPlayer
    {
        protected override void _bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            return;
        }

        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
        }

        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
        }

        public FakeDocumentPlayer(DocumentViewModel doc) : base(doc) { }
    }

    internal class StubbDocumentViewModel : DocumentViewModel
    {
        public bool HasBeenUpdated = false;

        public override void UpdateProgress(PlayerUpdate update)
        {
            HasBeenUpdated = true;
            Player.Pause();
        }
    }
}
