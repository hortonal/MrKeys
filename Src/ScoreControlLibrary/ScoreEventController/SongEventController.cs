using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoreControlLibrary.SongEventParser;
using Sanford.Multimedia.Timers;
using Sanford.Multimedia.Midi;

namespace ScoreControlLibrary.ScoreEventController
{
    class SongEventController : ISongEventController, IDisposable
    {
        //Notify others of events raised from the controller
        public event SongEventHandler SongNoteEvent;

        //Used for thread safety (ideally...)
        private readonly object _lockObject = new object();

        //Meat of the hard work done in this clock
        private MidiInternalClock _clock;

        private bool _disposed = false;
        

        public SongEventController()
        {
            _clock = new MidiInternalClock();

            //Need to do something else here??
            //Pulses per quarter note... 16 sounds reasonable.
            //Trade off between accuracy and CPU
            _clock.Ppqn = 16;


            /* // Setup clock tick event handling
            _clock.Tick += delegate (object sender, EventArgs e)
            {
                lock (_lockObject)
                {
                    if (!playing)
                    {
                        return;
                    }

                    foreach (IEnumerator<int> enumerator in enumerators)
                    {
                        enumerator.MoveNext();
                    }
                }
            };*/

        }

        #region ISongEventController
        public void AddNoteList(SongNoteList songNoteList)
        {
            //Set the tempo??
            var metaMessage = new MetaMessage(MetaType.Tempo, new byte[] { 120 });
            _clock.Process(metaMessage);

            foreach (var note in songNoteList)
            {
                //metaMessage = new MetaMessage(MetaType., new byte[] { 120 } );


                _clock.Process(metaMessage);
            }
            
            throw new NotImplementedException();
        }

        public void Pause()
        {
            Stop();
        }

        public void Play()
        {
            lock(_lockObject)
            {
                if (_clock.IsRunning) _clock.Stop();
                _clock.Start();
            }
        }

        public void Resume()
        {
            lock (_lockObject)
            {
                if (_clock.IsRunning) return;
                _clock.Continue();
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (_clock.IsRunning)
                {
                    _clock.Stop();
                }
            }
        }
        #endregion ISongEventController


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
    }
}
