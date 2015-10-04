using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoreControlLibrary.SongEventParser;
using Sanford.Multimedia.Timers;
using Sanford.Multimedia.Midi;
using System.Diagnostics;
using Common.Events;
using System.ComponentModel;
using System.Threading;
using Common.Media;
using Common.Logging;

namespace ScoreControlLibrary.ScoreEventController
{
    class SongEventController : ISongEventController, IMediaService, IDisposable
    {
        //Notify others of events raised from the controller
        public event SongEventHandler SongNoteEvent;
        public event SongFinishedHandler Finished;
        public event SongFinishedHandler Starting;
        public event SongFinishedHandler Stopping;

        //Used for thread safety (ideally...)
        private readonly object _lockObject = new object();

        //Meat of the hard work done in this clock
        private MidiInternalClock _clock;

        private bool _disposed = false;

        //State variables
        private Song _songCopy;
        private Song _song;
        private int _nextNoteTick = 0;
        private int _backGroundWorkersRunning = 0;

        private bool _isPaused = false;
        //private bool _isPlaying = true;
        //private bool _isRecording = true;

        private ILogger _logger;

        public SongEventController(ILogger logger)
        {
            _logger = logger;
        }

        private void IntialiseWithSong(Song song)
        {
            _song = song;
            _songCopy = song.MakeCopy();

            PrepareClock();

            if (_songCopy.Any())
            {
                _nextNoteTick = ConvertNoteTimeToTick(_songCopy.Tempo, _songCopy.First().Key);
                CanPlay = true;
            }
        }

        private void PrepareClock()
        {
            //if (_clock != null) _clock.Dispose();
            
            _clock = new MidiInternalClock();

            //Pulses per quarter note... 24 sounds reasonable.
            //Trade off between accuracy and CPU
            _clock.Ppqn = 24;

            //clock tempo in microseconds per beat. Song in bpm
            _clock.Tempo = LengthOfBeatInMicroSeconds(_songCopy.Tempo);
            
            // Setup clock tick event handling
            _clock.Tick += HandleClockTick;
        }

        private void HandleClockTick(object sender, EventArgs e)
        {
            int ticks;
            lock (_lockObject)
            {
                ticks = _clock.Ticks;
            }

            if (ticks >= _nextNoteTick)
            {
                double noteTime = 0;
                double nextNoteTime = 0;
                //Get note list item
                noteTime = _songCopy.First().Key;
                
            
                ICollection<SongNote> noteList = _songCopy.First().Value;

                //Now drop it from the song
                _songCopy.Remove(_songCopy.First().Key);

                if (_songCopy.Any())
                {
                    nextNoteTime = _songCopy.First().Key;
                }
                else
                {
                    nextNoteTime = 0;
                }

                foreach (SongNote note in noteList)
                {
                    _logger.Log(this, LogLevel.Debug, "Controller raising note press for: " + note.ToString());

                    SongNoteEvent(this, new SongEventArgs(
                        new PianoKeyStrokeEventArgs(note.PitchId, KeyStrokeType.KeyPress, note.Velocity),
                        noteTime,
                        nextNoteTime));

                    //Spark up a background worker to wait for the note to finish and release the key press
                    var worker = new BackgroundWorker();

                    var noteCopy = note;
                    worker.DoWork += (ob, arg) =>
                    {
                        _logger.Log(this, LogLevel.Debug, "Controller raising note release for: " + note.ToString());

                        Thread.Sleep(CalculateNoteDurationMilliSeconds(_songCopy.Tempo, noteCopy));
                        SongNoteEvent(this, new SongEventArgs(
                            new PianoKeyStrokeEventArgs(note.PitchId, KeyStrokeType.KeyRelease, 0),
                                                    noteTime,
                                                    nextNoteTime));
                    };

                    worker.RunWorkerCompleted += (ob, arg) =>
                    {
                        _backGroundWorkersRunning--;
                    };

                    _backGroundWorkersRunning++;
                    worker.RunWorkerAsync();
                }

                if (!_songCopy.Any())
                {
                    FinishSong();
                }

                //New first item becomes next thing to watch for
                _nextNoteTick = ConvertNoteTimeToTick(_songCopy.Tempo, nextNoteTime);
            }
        }

        private int CalculateNoteDurationMilliSeconds(int tempo, SongNote note)
        {
            return (int) (LengthOfBeatInMicroSeconds(tempo) * note.Duration / 1000);
        }

        //Tempo in bpm
        private int LengthOfBeatInMicroSeconds(int tempo)
        {
            return (60 * 1000000 / tempo);
        }
        
        private int ConvertNoteTimeToTick(int tempo, double noteTime)
        {
            return (int) (1.0 * noteTime * _clock.Ppqn);
        }

        #region ISongEventController
        public void SetSong(Song song)
        {
            IntialiseWithSong(song);
        }

        private void FinishSong()
        {
            lock (_lockObject)
            {
                _clock.Stop();
                _nextNoteTick = 0;
            }

            while (_backGroundWorkersRunning > 0) { }

            Finished(this, null);

            //Reset clock
            //PrepareClock();

            CanPlay = true;
            CanPause = false;
            CanStop = false;

        }

        public void Pause()
        {
            lock (_lockObject)
            {
                if (_clock.IsRunning)
                {
                    Stopping(this, null);
                    _clock.Stop();
                }
            }

            _isPaused = true;
            CanStop = false;
            CanPlay = true;
            CanPause = false;
        }

        public void Play()
        {
            Starting(this, null);
            if (_isPaused)
            {
                Resume();
                return;
           }

            _clock.SetTicks(0);
            _songCopy = _song.MakeCopy();

            Starting(this, null);

            lock (_lockObject)
            {    
                if (_clock.IsRunning) _clock.Stop();
                _clock.Start();
            }
            CanStop = true;
            CanPlay = false;
            CanPause = true;
        }

        public void Resume()
        {
            _isPaused = false;

            Starting(this, null);
            lock (_lockObject)
            {
                if (_clock.IsRunning) return;
                _clock.Continue();
            }
            
            CanPause = true;
            CanPlay = false;
            CanStop = true;
        }

        public void Stop()
        {
            //FinishSong();
            Stopping(this, null);
            lock (_lockObject)
            {
                _clock.Stop();
                _nextNoteTick = 0;
            }

            //while (_backGroundWorkersRunning > 0) { }

            CanPlay = true;
            CanPause = false;
            CanStop = false;
        }
        #endregion ISongEventController
 
        public bool CanPlay { get; set; }
        public bool CanStop { get; set; }
        public bool CanPause { get; set; }
        public bool CanRecord { get { return false; } set {}}

        public int CurrentTempo
        {
            get
            {
                return LengthOfBeatInMicroSeconds(_songCopy.Tempo) / 1000;
            }
        }

        ~SongEventController()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if (disposing)
                {
                    lock (_lockObject)
                    {
                        Stop();
                        _clock.Dispose();
                        _disposed = true;
                    }
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

 
        public void Record()
        {
            //Not implemented
        }
    }
}
