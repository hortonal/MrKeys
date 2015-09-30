using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Services;
using ScoreControlLibrary.Views;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using MusicXml;
using Common.IO;
using Common.Music;


namespace ScoreControlLibrary
{
    public class BasicTestControl: ITestControlService
    {
        UserControl _control;

        public BasicTestControl(IUnityContainer container, IOutput output)
        {
            XScore testMusicScore = new XScore(@".\Examples\Scales-C-2-Hands.xml");
//            XScore testMusicScore = new XScore(@".\Examples\Promenade_Example.xml");
            _control = new ScoreControl(container, testMusicScore);


            Song _song;
            _song = new MusicParser(testMusicScore).Parse();
            SongPlayer _songPlayer = new SongPlayer(output, _song);
            _songPlayer.Play();
        }

        public UserControl Control
        {
            get { return _control; }
        }

        public string Name
        {
            get
            {
                return "Basic Sheet Music Test";
            }
        }

        public void StartTest()
        {
            throw new NotImplementedException();
        }

        public void MarkTest()
        {
            throw new NotImplementedException();
        }

 
    }
}
