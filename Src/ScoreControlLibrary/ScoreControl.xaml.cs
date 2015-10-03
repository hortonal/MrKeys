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

        //State variables
        private double _currentHorizontalScrollPosition = 0;


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

            _scoreRenderer = new ScoreRenderer(_musicScore, ScoreGrid);
            _scoreRenderer.Render();

            Song _song;
            _song = new XScoreNoteEventParser(_musicScore).Parse();
            
            _songEventController = new SongEventController(_song);

            //Hook up a handler for the song controller events
            _songEventController.SongNoteEvent += Controller_SongNoteEvent;
            _songEventController.Finished += Controller_Finished;

            _mediaServiceHost.MediaService = (IMediaService) _songEventController;
        }

        private void Controller_Finished(object sender, EventArgs e)
        {
            ResetHorizontalScrollPosition();
        }

        private void Controller_SongNoteEvent(object sender, SongEventArgs e)
        {
            _virtualKeyboard.HandleIncomingMessage(this, e.NoteKeyStrokeEvenArguments);
            _output.Send(this, e.NoteKeyStrokeEvenArguments);

            UpdateHorizontalScrollPosition(e.NoteTime, 200);
        }

        private void ResetHorizontalScrollPosition()
        {
            _currentHorizontalScrollPosition = 0;
            Dispatcher.Invoke(new Action(() =>
            {
                ScoreScrollViewer.ScrollToHorizontalOffset(0);
            }));
        }

        private void UpdateHorizontalScrollPosition(double noteTime, double buffer)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var itemHorizontalPosition = _scoreRenderer.GetHorizontalOffsetForNoteTime(noteTime);
                if (itemHorizontalPosition > _currentHorizontalScrollPosition)
                {
                    _currentHorizontalScrollPosition = itemHorizontalPosition;

                    double actualScrollPosition;
                    if(_currentHorizontalScrollPosition - buffer > 0)
                    {
                        actualScrollPosition = _currentHorizontalScrollPosition - buffer;
                    }
                    else
                    {
                        actualScrollPosition = 0;
                    }

                    ScoreScrollViewer.ScrollToHorizontalOffset(actualScrollPosition);
                }
            }));
        }
    }
}
