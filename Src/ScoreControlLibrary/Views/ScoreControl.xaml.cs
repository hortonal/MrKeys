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
using SongPlayer;
using Common.IO;
using Common.Media;

namespace ScoreControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl : UserControl
    {
        public ScoreControl(IUnityContainer container, IOutput output, IMediaService mediaService, XScore musicScore): this()
        {
            new ScoreRenderer(musicScore, ScoreGrid).Render();

            Song _song;
            _song = new MusicParser(musicScore).Parse();
            Player _songPlayer = new Player(output, _song);

            mediaService.CanPlay = true;

            //Initialise our Sequencer
            //Build the sequencer from the xscore (or from a song object?)

            //We need to then become the media service. How do I own media service reference used 
        }

        public ScoreControl()
        {
            InitializeComponent();

        }
    }
}
