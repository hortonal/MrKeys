using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Practices.Unity;
using MusicXml;
//using SongPlayer;
using Common.IO;
using Common.Media;
using ScoreControlLibrary.ScoreEventController;
using ScoreControlLibrary.SongEventParser;
using KeyBoardControlLibrary;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using Common.Logging;
using Common.Events;

namespace ScoreControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl : UserControl
    {
        IUnityContainer _container;
        IOutput _output;
        IMidiInput _midiInput;
        ISongEventController _songEventController;
        IVirtualKeyBoard _virtualKeyboard;
        IMediaServiceHost _mediaServiceHost;
        XScore _musicScore;
        ScoreRenderer _scoreRenderer;
        Timer _updateScrollTimer;
        ILogger _logger;
        Song _song;
        IInputEvents _intputEvents;

        //Frequency of horizontal update, ms
        private int _scrollTimingPerdiod = 6;
        
        //State variables
        private double _currentHorizontalScrollPosition = 0;
        //song tempo. How long is 1 noteTime in milliseconds
        private double _currentTempo;
        //horizontal units per millisecond
        private double _scrollSpeed = 0;
        private double _scrollOffset = 200;
        private double _lastNoteTime = 0;
        private double _nextNoteTime = 0;

        private object _lockObject = new object();

        public ScoreControl()
        {
            InitializeComponent();
        }

        public ScoreControl(IUnityContainer container, IOutput output, IMidiInput midiInput, IInputEvents inputEvents, IMediaServiceHost mediaServiceHost, 
            IVirtualKeyBoard virtualKeyboard, ILogger logger, XScore musicScore): this()
        {
            _container = container;
            _output = output;
            _intputEvents = inputEvents;
            _midiInput = midiInput;
            _virtualKeyboard = virtualKeyboard;
            _musicScore = musicScore;
            _mediaServiceHost = mediaServiceHost;
            _logger = logger;

            _updateScrollTimer = new Timer(ScrollTimerHandler, null, Timeout.Infinite, _scrollTimingPerdiod);

            _scoreRenderer = new ScoreRenderer(_musicScore, ScoreGrid);
            _scoreRenderer.Render();
            ScoreGrid.Width = _scoreRenderer.GetMaxHorizontalPosition();

            _intputEvents.MessageReceived += HandleInputEvent;

            _midiInput.StartRecording();

            ConfigureSongEventController();
        }

       
        private void ConfigureSongEventController()
        {
            _song = new XScoreNoteEventParser(_musicScore).Parse();

            _songEventController = _container.Resolve<SongEventController>();
            _songEventController.SetSong(_song);

            _currentTempo = _songEventController.CurrentTempo;
            //Hook up all handlers for the song controller events
            _songEventController.SongNoteEvent += Controller_SongNoteEvent;
            _songEventController.Finished += Controller_Finished;
            _songEventController.Starting += Controller_Starting;
            _songEventController.Stopping += Controller_Stopping; ;

            _mediaServiceHost.MediaService = (IMediaService)_songEventController;
        }

        private void Controller_Stopping(object sender, EventArgs e)
        {
            _updateScrollTimer.Change(Timeout.Infinite, _scrollTimingPerdiod);
            _lastNoteTime = 0;
        }

        private void Controller_Starting(object sender, EventArgs e)
        {
            _scoreRenderer.ResetMarking();
            _updateScrollTimer.Change(0, _scrollTimingPerdiod);
            _scrollSpeed = 0;
            _lastNoteTime = 0;
            _nextNoteTime = 0;
        }

        private void Controller_Finished(object sender, EventArgs e)
        {
            _updateScrollTimer.Change(Timeout.Infinite, _scrollTimingPerdiod);
            ResetHorizontalScrollPosition();
        }

        private void Controller_SongNoteEvent(object sender, SongEventArgs e)
        {
            _logger.Log(this, LogLevel.Debug, "Score control handling song note event. " + e);

            _virtualKeyboard.HandleIncomingMessage(this, e.NoteKeyStrokeEvenArguments);
            _output.Send(this, e.NoteKeyStrokeEvenArguments);

            if (e.NoteKeyStrokeEvenArguments.KeyStrokeType != Common.Events.KeyStrokeType.KeyPress) return;
            
            //Only handle this once per chord   
            if (e.NoteTime <= _lastNoteTime) return;

            _lastNoteTime = e.NoteTime;
            _nextNoteTime = e.NextNoteTime;

            //Update scrool speed
            var _lastHorizontalScrollEventPosition = _scoreRenderer.GetHorizontalPositionForNoteTime(_lastNoteTime);
            var _nextHorizontalScrollEventPosition = _scoreRenderer.GetHorizontalPositionForNoteTime(_nextNoteTime);

            SetScrollSpeed(_currentHorizontalScrollPosition, _nextHorizontalScrollEventPosition, _lastNoteTime, _nextNoteTime);
                
            //_currentHorizontalScrollPosition = _lastHorizontalScrollEventPosition;
            //UpdateHorizontalScrollPosition();
           
            _logger.Log(this, LogLevel.Debug, "Score control finished handling song note event. " + e);
        }

        private void ResetHorizontalScrollPosition()
        {
            _currentHorizontalScrollPosition = 0;
            Dispatcher.Invoke(new Action(() =>
            {
                ScoreScrollViewer.ScrollToHorizontalOffset(0);
            }));
        }

        private void SetScrollSpeed(double startPosition, double endPosistion, double startNoteTime, double endNoteTime)
        {
            double scrollSpeed = (endPosistion - startPosition) / ((endNoteTime - startNoteTime) * _currentTempo);

            if(scrollSpeed < 0) _scrollSpeed = 0;
            else if (scrollSpeed < 200) _scrollSpeed = scrollSpeed;
        }

        private void UpdateHorizontalScrollPosition()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                double actualScrollPosition;
                if (_currentHorizontalScrollPosition - _scrollOffset > 0)
                {
                    actualScrollPosition = _currentHorizontalScrollPosition - _scrollOffset;
                }
                else
                {
                    actualScrollPosition = 0;
                }

                ScoreScrollViewer.ScrollToHorizontalOffset(actualScrollPosition);
            }));
        }

        private void ScrollTimerHandler (object o)
        {
            //Debug.WriteLine("Tick");
            _currentHorizontalScrollPosition += (_scrollSpeed * _scrollTimingPerdiod);
            UpdateHorizontalScrollPosition();
        }


        private void HandleInputEvent(object sender, PianoKeyStrokeEventArgs e)
        {
            if (e == null) return;
            if (e.KeyStrokeType != KeyStrokeType.KeyPress) return;

            //Get the current note time from the playing song
            double noteTime = _songEventController.GetCurrentNoteTime();

            //Check if note played is in any of these 3 note times.
            bool inPreviousNotes;
            bool inNextNotes;
            double compareNoteTime = -1;

            inPreviousNotes = _song[_lastNoteTime].KeyPresses.Any(n => n.PitchId == e.midiKeyId);
            inNextNotes = _song[_nextNoteTime].KeyPresses.Any(n => n.PitchId == e.midiKeyId);

            if (inPreviousNotes) compareNoteTime = _lastNoteTime;
            if (inNextNotes) compareNoteTime = _nextNoteTime;

            int markForNote = 0;

            if (compareNoteTime == -1)
            {
                markForNote = 0;
            }
            else
            {
                double accuracy = Math.Abs(noteTime - compareNoteTime);
                if (accuracy < 1.0 / 4)
                {
                    markForNote = 10;
                }
                else if (accuracy < 1.0 / 2)
                {
                    markForNote = 6;
                }
                else if (accuracy < 1.1)
                {
                    markForNote = 4;
                }
                else
                {
                    markForNote = 1;
                }
            }

            _scoreRenderer.MarkNote(compareNoteTime, markForNote, e.midiKeyId);
        }
    }
}
