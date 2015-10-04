using Common.Events;
using Common.IO;
using System.Windows.Controls;
using System;
using System.Linq;
using Common.Devices;
using System.ComponentModel;
using Common.Logging;

namespace KeyBoardControlLibrary
{
    public class VirtualKeyBoard: IVirtualKeyBoard, IDisposable
    {
        private KeyRange _keyRange;
        private KeyDictionary _keyDictionary;
        private IInputEvents _inputEvents;
        private IMidiInput _midiInput;
        private ILogger _logger;
     
        public event PianoKeyStrokeEvent KeyPressEvent = (o, a) => { };

        public VirtualKeyBoard(IInputEvents inputEvents, IMidiInput midiInput, ILogger logger)
        {
            _inputEvents = inputEvents;
            _midiInput = midiInput;
            _logger = logger;
            Initialise();
        }

        private void Initialise()
        {
            _logger.Log(LogLevel.Info, "Initialising virtual keyboard");

            //Give the keyboard a set of virtual keys
            _keyDictionary = new KeyDictionary();

            _keyRange = new KeyRange();
            _keyRange.BottomKeyId = _keyDictionary.Keys.Min();
            _keyRange.TopKeyId = _keyDictionary.Keys.Max();
            
            foreach (var virtualKey in _keyDictionary.Values)
            { 
                //Wrap up each key event in our single VirtualKeyboard key press event.
                virtualKey.KeyPressEvent += (o, e) => KeyPressEvent(o, e);
            }

            //Make keyboard input show up on screen
            _midiInput.MessageReceived += (o, e) => HandleIncomingMessage(o, e);

            //Allow the global input collection to access the virtual keyboard as an input
            KeyPressEvent += (o, e) => _inputEvents.HandleInputEvent(o, e);
        }

        public void DrawKeys(Panel parentControl)
        {
            foreach (var key in _keyDictionary.Values)
            {
                parentControl.Children.Add(key);
            }
        }

        public void HandleIncomingMessage(object sender, PianoKeyStrokeEventArgs e)
        {
            _logger.Log(this, LogLevel.Debug, "Handling keyboard input event");

            if (e == null) return;

            if (_keyDictionary.ContainsKey(e.midiKeyId))
            {
                switch (e.KeyStrokeType)
                {
                    case KeyStrokeType.KeyPress:
                        _keyDictionary[e.midiKeyId].SetKeyPressedColour(e.KeyVelocity);
                        break;
                    case KeyStrokeType.KeyRelease:
                        _keyDictionary[e.midiKeyId].SetDefaultKeyColour();
                        break;
                }
            }
        }

        public KeyRange GetkeyRange()
        {
            return _keyRange;
        }

        public void Dispose()
        {
            //Make keyboard input show up on screen
            _midiInput.MessageReceived -= HandleIncomingMessage;
            
        }

        #region MidiInput
        public event PianoKeyStrokeEvent MessageReceived;
        public event PropertyChangedEventHandler PropertyChanged;
        
        void IMidiInput.Initialise() { }

        public bool IsInitialised
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                return "Virtual Keybaord";
            }
        }

        public DeviceType DeviceType
        {
            get
            {
                return DeviceType.Input;
            }
        }


        public void StartRecording()
        {
            throw new NotImplementedException();
        }

        public void StopRecording()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
