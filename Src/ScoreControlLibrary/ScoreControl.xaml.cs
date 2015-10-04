﻿using System;
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


namespace ScoreControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl : UserControl
    {

        IOutput _output;
        ISongEventController _songEventController;
        IVirtualKeyBoard _virtualKeyboard;
        IMediaServiceHost _mediaServiceHost;
        XScore _musicScore;
        ScoreRenderer _scoreRenderer;
        Timer _updateScrollTimer;

        //Frequency of horizontal update, ms
        private int _scrollTimingPerdiod = 50;
        
        //State variables
        private double _currentHorizontalScrollPosition = 0;
        //song tempo. How long is 1 noteTime in milliseconds
        private double _currentTempo;
        //horizontal units per millisecond
        private double _scrollSpeed = 0;
        private double _scrollOffset = 100;
        private double _lastNoteTime = 0;
        private object _lockObject = new object();

        public ScoreControl()
        {
            InitializeComponent();
        }

        public ScoreControl(IUnityContainer container, IOutput output, IMediaServiceHost mediaServiceHost, 
            IVirtualKeyBoard virtualKeyboard, XScore musicScore): this()
        {
            _output = output;
            _virtualKeyboard = virtualKeyboard;
            _musicScore = musicScore;
            _mediaServiceHost = mediaServiceHost;

            _updateScrollTimer = new Timer(ScrollTimerHandler, null, Timeout.Infinite, _scrollTimingPerdiod);

            _scoreRenderer = new ScoreRenderer(_musicScore, ScoreGrid);
            _scoreRenderer.Render();

            Song _song;
            _song = new XScoreNoteEventParser(_musicScore).Parse();
            
            _songEventController = new SongEventController(_song);

            _currentTempo = _songEventController.CurrentTempo;
            //Hook up all handlers for the song controller events
            _songEventController.SongNoteEvent += Controller_SongNoteEvent;
            _songEventController.Finished += Controller_Finished;
            _songEventController.Starting += Controller_Starting;
            _songEventController.Stopping += Controller_Stopping; ;

            _mediaServiceHost.MediaService = (IMediaService) _songEventController;
        }

        private void Controller_Stopping(object sender, EventArgs e)
        {
            _updateScrollTimer.Change(Timeout.Infinite, _scrollTimingPerdiod);
            _lastNoteTime = 0;
        }

        private void Controller_Starting(object sender, EventArgs e)
        {
            _updateScrollTimer.Change(0, _scrollTimingPerdiod);
        }

        private void Controller_Finished(object sender, EventArgs e)
        {
            _updateScrollTimer.Change(Timeout.Infinite, _scrollTimingPerdiod);
            ResetHorizontalScrollPosition();
        }

        private void Controller_SongNoteEvent(object sender, SongEventArgs e)
        {
            _virtualKeyboard.HandleIncomingMessage(this, e.NoteKeyStrokeEvenArguments);
            _output.Send(this, e.NoteKeyStrokeEvenArguments);

            if (e.NoteKeyStrokeEvenArguments.KeyStrokeType != Common.Events.KeyStrokeType.KeyPress) return;
            //Only handle this once per chord   
            if (e.NoteTime <= _lastNoteTime) return;
            
            //Update scrool speed
            var _lastHorizontalScrollEventPosition = _scoreRenderer.GetHorizontalPositionForNoteTime(e.NoteTime);
            var _nextHorizontalScrollEventPosition = _scoreRenderer.GetHorizontalPositionForNoteTime(e.NextNoteTime);
            var distance = (_nextHorizontalScrollEventPosition - _lastHorizontalScrollEventPosition);
            var timeToNextEvent = (e.NextNoteTime - e.NoteTime) * _currentTempo;

            _currentHorizontalScrollPosition = _lastHorizontalScrollEventPosition;
            UpdateHorizontalScrollPosition();
            var tempScrollSpeed = distance / timeToNextEvent;

            if (tempScrollSpeed >= 0) _scrollSpeed = tempScrollSpeed;
       
            _lastNoteTime = e.NoteTime;
            
            //Debug.WriteLine("   Speed: " + _scrollSpeed + ", lastNote: " + e.NoteTime + ", nextNote: " + e.NextNoteTime + ", lastEvent: " + _lastHorizontalScrollEventPosition + ", NextEvent: " + _nextHorizontalScrollEventPosition);            
        }

        private void ResetHorizontalScrollPosition()
        {
            _currentHorizontalScrollPosition = 0;
            Dispatcher.Invoke(new Action(() =>
            {
                ScoreScrollViewer.ScrollToHorizontalOffset(0);
            }));
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

    }
}