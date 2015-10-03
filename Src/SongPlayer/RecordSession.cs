using System;
using System.Threading;
using Sanford.Multimedia.Midi;
using Common.Events;
using Common.Media;
using Common.Infrastructure;
using Common.IO;
using KeyBoardControlLibrary;

namespace SongPlayer
{
  /// <summary>
  /// Records input from Midi keyboard
  /// </summary>
    public class RecordSession : ObservableObject, IMediaService
    {
        // Module Level Vars
        private IMidiInput _midiInput;
        private IOutput _output;
        private RecordingSession m_Session;
        private IVirtualKeyBoard _virtualKeyboard;

        public delegate void MessageReceivedHandler();
        public event PianoKeyStrokeEvent MessageReceived;
        
        #region ctor
        /// <summary>
        /// Creates a new recording session object
        /// </summary>
        /// <param name="midiInput">Expected to be injected</param>
        public RecordSession(IMidiInput midiInput, IOutput output, IVirtualKeyBoard virtualKeyboard)
        {
            _midiInput = midiInput;
            _output = output;
            _virtualKeyboard = virtualKeyboard;
            Init();
        }

        /// <summary>
        /// Init method
        /// </summary>
        private void Init()
        {
            IsRecording = false;
            this.Clock = new MidiInternalClock();
            m_Session = new RecordingSession(this.Clock);
            this.Sequencer = new Sequencer();

            Sequencer.ChannelMessagePlayed += HandlePlayerMessage;
            Sequencer.PlayingCompleted += HandlePlayingCompleted;
            

            //If midi input isn't initialised, try and initiailise it.
            if (!_midiInput.IsInitialised) _midiInput.Initialise();
            if (_midiInput.IsInitialised)
            {
                CanRecord = true;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Sequencer property responsible for playing midi information
        /// </summary>
        public Sequencer Sequencer
        {
            get;
            set;
        }

        /// <summary>
        ///  Midi Clock
        /// </summary>
        public MidiInternalClock Clock { get; set; }

        /// <summary>
        /// Number of messages received in the current session
        /// </summary>
        public int NumMsgsReceived { get; set;  }

        /// <summary>
        /// True if a recording is currently in progress
        /// </summary>
        public bool IsRecording { get; set; }

        /// <summary>
        /// True if a recording is currently in progress
        /// </summary>
        public bool IsPlaying { get; set; }

        /// <summary>
        /// True if playback is paused
        /// </summary>
        public bool IsPaused { get; set; }
        #endregion // Properties

        #region Public



        /// <summary>
        ///  Clears and resets the current session
        /// </summary>
        public void Clear()
        {
            // Remove the recording session
            m_Session.Clear();
            //#TODO: Un comment this? this.Sequencer.Sequence = null;
            this.NumMsgsReceived = 0;
        }

        #endregion

        #region Private
        private void HandlePlayingCompleted(object o, EventArgs e)
        {
            IsPlaying = false;
            CanPlay = true;
        }

        private void HandlePlayerMessage(object o, ChannelMessageEventArgs e)
        {
            var pianoEventArgs = SanfordUtils.ConvertChannelMessageToKeyStrokeEventArgs(e.Message);
            if(_output != null) _output.Send(this, pianoEventArgs);
            if(_virtualKeyboard != null) _virtualKeyboard.HandleIncomingMessage(o, pianoEventArgs);
        }

        /// <summary>
        /// Handles the Midi message received
        /// Only called if in recording mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleChannelMessageReceived(object sender, PianoKeyStrokeEventArgs e)
        {

            m_Session.Record(SanfordUtils.ConvertKeyStrokeEventArgsToChannelMessage(e));

            //_keyboard.HandleIncomingMessage(null, new PianoKeyStrokeEventArgs(_testKeyId, KeyStrokeType.KeyRelease, 100));

            this.NumMsgsReceived++;
        } 

        #endregion

        #region MediaServiceImplementation
        public bool CanPlay { get; set; }

        public bool CanStop { get; set; }

        public bool CanPause { get; set; }

        public bool CanRecord { get; set; }

        private void StopRecording()
        {
            _midiInput.MessageReceived -= HandleChannelMessageReceived;

            // Stop recording
            try
            {
                this.Clock.Stop();
                _midiInput.StopRecording();
                
                IsRecording = false;
                //Build track
                BuildPlayback();
            }
            catch (Exception ex)
            {
                Exceptions.ErrHandler(ex);
            }
        }


        private void BuildPlayback()
        {
            // Build a track object from
            // the current session
            m_Session.Build();
            Track trk = m_Session.Result;

            // Create a new sequence
            // from the current track
            var sequence = new Sequence();
            sequence.Add(trk);

            // Load the sequence into
            // the sequencer
            Sequencer.Sequence = sequence;
        }

        private bool RecordingExists()
        {
            return Sequencer.Sequence.Count > 0;
        }

        public void Play()
        {
            if(IsRecording)
            {
                StopRecording();
            }

            if(IsPaused)
            {
                Sequencer.Continue();
            }
            else
            {
                Sequencer.Start();
            }

            IsPlaying = true;
            CanPlay = false;
            CanPause = true;            
        }

        public void Stop()
        {
            if (IsRecording)
            {
                StopRecording();
            }

            Sequencer.Stop();
            
            CanRecord = true;
            CanPlay = RecordingExists();
        }

        public void Pause()
        {
            if (IsPaused)
            {
                Sequencer.Continue();
                IsPaused = false;
                CanPause = true;
                CanPlay = false;
            }
            else
            {
                Sequencer.Pause();
                IsPaused = true;
                CanPause = false;
                CanPlay = true;
            }
        }

        public void Record()
        {
            if (IsRecording) return;
            if (IsPlaying) Stop();

            // Clear current sequence if any
            Clear();

            // Start recording
            try
            {
                Clock.Start();
                _midiInput.StartRecording();
                _midiInput.MessageReceived += HandleChannelMessageReceived;
                
                IsRecording = true;
                CanPlay = true;
                CanRecord = false;
                CanStop = true;
            }
            catch (Exception ex)
            {
                Exceptions.ErrHandler(ex);
            }
        }


        #endregion
        ~RecordSession()
        {
            _midiInput.MessageReceived -= HandleChannelMessageReceived;
            Sequencer.ChannelMessagePlayed -= HandlePlayerMessage;
            Sequencer.PlayingCompleted -= HandlePlayingCompleted;

            Clock.Dispose();
            m_Session.Clear();
            Sequencer.Dispose();
        }

    } // End Class
} // End Namespace
