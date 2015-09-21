using Common.Infrastructure;
using System;
using System.Windows.Input;
using Common.IO;
using Common.Events;
using KeyBoardControlLibrary;
using System.Threading;
using System.ComponentModel;


namespace MrKeys.NoteGuess
{
    class NoteGuessViewModel: NotifyPropertyBasic
    {
        private bool _testActive;
        private int _testKeyId;
        private IOutput _outputDevice;
        private IVirtualKeyBoard _keyboard;

        public NoteGuessViewModel(IOutput outputDevice, IInputEvents inputDevice,
            IVirtualKeyBoard keyBoard)
        {
            _testActive = false;
            _outputDevice = outputDevice;
            _keyboard = keyBoard;
            //Subscribe to input messages.
            inputDevice.MessageReceived += (o, e) => HandleInputDeviceInput(o, e);
        }
        
        private void HandleInputDeviceInput(object o, PianoKeyStrokeEventArgs e)
        {
            if (_testActive)
            {
                if(e.KeyStrokeType == KeyStrokeType.KeyPress)
                {
                    int keyDifference = Math.Abs(_testKeyId - e.midiKeyId);

                    if (keyDifference == 0)
                        Result = "Nailed it - you're a hero";
                    else if (keyDifference % 12 == 0)
                    {
                        Result = "Hmm, try another octave";
                    }
                    else if (keyDifference > 11)
                    {
                        Result = "Seriously??! Do your ears even work?";
                    }
                    else if (keyDifference > 3)
                    {
                        Result = "Not bad, for a deaf beach boy";
                    }
                    else
                    {
                        Result = "Only marginally disappointing";
                    }

                    var worker = new BackgroundWorker();
                    worker.DoWork += (ob, arg) => { Thread.Sleep(1000); };

                    _keyboard.HandleIncomingMessage(null, new PianoKeyStrokeEventArgs(_testKeyId, KeyStrokeType.KeyPress, 100));

                    //I need to be a bit more careful here
                    //If this method is called again too quickly, the lift key event won't fire
                    worker.RunWorkerCompleted += (ob, arg) =>
                    {
                        _keyboard.HandleIncomingMessage(null, new PianoKeyStrokeEventArgs(_testKeyId, KeyStrokeType.KeyRelease, 100));
                        if(!_testActive) Result = "...go again..";
                    };
                    
                    worker.RunWorkerAsync();
                    
                    TestActive = false;
                }
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

        public bool TestActive
        {
            get { return _testActive; }
            set
            {
                _testActive = value;
                RaisePropertyChanged("TestActive");
            }
        }

        public ICommand GoButton
        {
            get { return new RelayCommand(StartTest, () => true); }
        }

        private void StartTest()
        {
            TestActive = true;

            Random rnd = new Random();
            KeyRange keyRange = _keyboard.GetkeyRange();

            _testKeyId = rnd.Next(keyRange.BottomKeyId, keyRange.TopKeyId);

            Result = "Good luck!";

            _outputDevice.Send(null, new PianoKeyStrokeEventArgs(_testKeyId, KeyStrokeType.KeyPress, 110));

            var worker = new BackgroundWorker();
            worker.DoWork += (o, e) => {Thread.Sleep(1000);};

            worker.RunWorkerCompleted += (o, e) =>
            {
                _outputDevice.Send(null, new PianoKeyStrokeEventArgs(_testKeyId, KeyStrokeType.KeyRelease, 0));
            };

            worker.RunWorkerAsync();
            
        }
    }
    
}
