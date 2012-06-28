using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothChords.ViewModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;

namespace SmoothChords.Player
{
    public class DocumentPlayer
    {
        protected bool _isPlaying = false;
        protected bool _isPaused = false;

        protected int _measure = 1;
        protected int _beat = 1;

        double _beatDouble;
        protected TimeSpan _beatPeriod;
        protected TimeSpan _beatSpan;
        DateTime _now;

        public int Measure
        {
            get
            {
                return _measure;
            }
        }

        public int Beat
        {
            get
            {
                return _beat;
            }
        }

        private DocumentViewModel _document = null;
        private BackgroundWorker _bgw = null;

        public DocumentPlayer(DocumentViewModel document)
        {

            
            _document = document;
            _bgw = new BackgroundWorker();
            _bgw.WorkerReportsProgress = true;
            _bgw.ProgressChanged += new ProgressChangedEventHandler(_bgw_ProgressChanged);
            _bgw.DoWork += new DoWorkEventHandler(_bgw_DoWork);
        }

        void _bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _document.UpdateProgress(e.UserState as PlayerUpdate);
        }

        protected virtual void _bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime prev = now;

            while (_isPlaying)
            {
                now = DateTime.Now;
                WorkerCycle(now, prev);
                prev = now;
            }
        }

        protected virtual void WorkerCycle(DateTime now, DateTime prev)
        {
            PlayerUpdate update = new PlayerUpdate();

            // Update time sync.
            _beatSpan = _beatSpan - (now - prev);

            // Update speed.
            _beatDouble = GetBeatDurationAsSeconds(_document.TempoInt);
            _beatPeriod = TimeSpan.FromSeconds(_beatDouble);

            // If beat time has ellapsed
            if (_beatSpan <= TimeSpan.Zero)
            {
                // Add another period.
                _beatSpan += _beatPeriod;

                if (_beat == 4)
                {
                    _beat = 1;
                    _measure += 1;
                }
                else _beat++;
            }

            // Prepare update message.
            update.BeatCompleteness = (double)(((double)_beatPeriod.Ticks - (double)_beatSpan.Ticks) / (double)_beatPeriod.Ticks);
            update.Beat = _beat;
            update.Measure = _measure;

            _bgw.ReportProgress(0, update);
            Thread.Sleep(5);
        }

        public virtual void Play()
        {
            if (!_isPaused)
            {
                _beatDouble = GetBeatDurationAsSeconds(_document.TempoInt);
                _beatPeriod = TimeSpan.FromSeconds(_beatDouble);
                _beatSpan = TimeSpan.FromSeconds(_beatDouble);
            }

            _isPlaying = true;
            _isPaused = false;

            _bgw.RunWorkerAsync();
        }

        public virtual void Stop()
        {
            _isPlaying = false;
            _beat = 1;
            _measure = 1;
            _beatSpan = TimeSpan.Zero;
            _beatPeriod = TimeSpan.Zero;
            _isPaused = false;
        }

        public virtual void Pause()
        {
            _isPlaying = false;
            _isPaused = true;
        }

        public double GetBeatDurationAsSeconds(int BPM)
        {
            return (double)((double)60 / (double)BPM);
        }
    }
}
