using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Services;
using ScoreControlLibrary.Views;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using MusicXml;

namespace ScoreControlLibrary
{
    public class BasicTestControl: ITestControlService
    {
        UserControl _control;

        public BasicTestControl(IUnityContainer container)
        {
            XScore testMusicScore = new XScore(@".\..\..\ThirdParty\MusicXML.Net\Promenade_Example.xml");
            //XScore testMusicScore = new XScore(@"C:\Users\Alex\Desktop\elite.xml");
            _control = new ScoreControl(container, testMusicScore);
        }

        public UserControl Control
        {
            get { return _control; }
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
