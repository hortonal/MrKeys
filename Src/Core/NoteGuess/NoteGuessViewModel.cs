using Common.Infrastructure;
using System;
using System.Windows.Input;
using Common.IO;
using Common.Events;
using KeyBoardControlLibrary;

namespace MrKeys.NoteGuess
{
    class NoteGuessViewModel: NotifyPropertyBasic
    {
        private bool _testRunning;
        private int _testKeyId;
        private IOutput _outputDevice;

        public NoteGuessViewModel(IOutput outputDevice, IInputEvents inputDevice,
            IVirtualKeyBoard keyBoard)
        {
            _testRunning = false;
            _outputDevice = outputDevice;
            //Subscribe to input messages.
            inputDevice.MessageReceived += (o, e) => HandleInputDeviceInput(o, e);
        }
        
        private void HandleInputDeviceInput(object o, PianoKeyStrokeEventArgs e)
        {
            if (_testRunning)
            {
                if (e.KeyStrokeType == KeyStrokeType.KeyPress && e.midiKeyId == _testKeyId)
                    Result = "Nailed it - nice one " + e.midiKeyId;
                else
                {
                    Result = "You suck " + e.midiKeyId;
                }
                
                TestRunning = false;
            }
        }

        private string _result;
        public string Result
        {
            get { return _result; }
            set {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        public bool TestRunning
        {
            get { return _testRunning; }
            set
            {
                _testRunning = value;
                RaisePropertyChanged("TestRunning");
            }
        }

        public ICommand GoButton
        {
            get { return new RelayCommand(StartTest, () => true); }
        }

        private void StartTest()
        {
            TestRunning = true;
            Random rnd = new Random();
            _testKeyId = rnd.Next(20, 100);

            //Do this async...
            _outputDevice.Send(new PianoKeyStrokeEventArgs(_testKeyId, KeyStrokeType.KeyPress, 100));
            System.Threading.Thread.Sleep(1000);
            _outputDevice.Send(new PianoKeyStrokeEventArgs(_testKeyId, KeyStrokeType.KeyRelease, 100));
            
            //now just wait for keyboard input event
        }
    }
    
}
