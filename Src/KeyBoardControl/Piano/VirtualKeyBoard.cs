using Common.Events;
using Common.IO;
using System.Windows.Controls;
using System;
using System.Linq;

namespace KeyBoardControlLibrary
{
    public class VirtualKeyBoard: IVirtualKeyBoard
    {
        private KeyRange _keyRange;
        private KeyDictionary _keyDictionary;
        private IInputEvents _inputEvents;
        private IMidiInput _midiInput;
        public event PianoKeyStrokeEvent KeyPressEvent = (o, a) => { };

        public VirtualKeyBoard(IInputEvents inputEvents, IMidiInput midiInput)
        {
            _inputEvents = inputEvents;
            _midiInput = midiInput;
            Initialise();
        }

        private void Initialise()
        {
            //Give the keyboard a set of virtual keys
            _keyDictionary = new KeyDictionary();

            _keyRange = new KeyRange();
            _keyRange.BottomKeyId = _keyDictionary.Keys.Min();
            _keyRange.TopKeyId = _keyDictionary.Keys.Max();
            
            foreach (var virtualKey in _keyDictionary.Values)
            { 
                //Wrap up each key event in our single VirtualKeyboard key press event.
                virtualKey.KeyPressEvent += (o, a) => KeyPressEvent(o, a);
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
    }
}
