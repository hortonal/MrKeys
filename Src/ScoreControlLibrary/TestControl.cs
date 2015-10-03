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
using SongPlayer;

namespace ScoreControlLibrary
{
    public class BasicTestControl: ITestControlService
    {
        UserControl _control;

        public BasicTestControl(IUnityContainer container, IOutput output)
        {
            XScore testMusicScore = new XScore(@".\ScoreRenderer\Examples\Scales-C-2-Hands.xml");
            //XScore testMusicScore = new XScore(@".\ScoreRenderer\Examples\Promenade_Example.xml");

            //Don't like the param overrides approach... Maybe allow only 1 active score and register the instance??
            _control = container.Resolve<ScoreControl>(new ParameterOverrides { { "musicScore", testMusicScore } });
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
