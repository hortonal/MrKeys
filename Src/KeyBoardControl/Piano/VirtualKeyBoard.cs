using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Events;
using Common.IO;
using System.Windows.Controls;
using System.Windows;

namespace KeyBoardControlLibrary
{
    public class VirtualKeyBoard: IVirtualKeyBoard
    {
        private KeyDictionary _keyDictionary;
        private IInputEvents _inputEvents;
        public event PianoKeyStrokeEvent KeyPressEvent = (o, a) => { };

        public VirtualKeyBoard(IInputEvents inputEvents)
        {
            _inputEvents = inputEvents;
            Initialise();
        }

        private void Initialise()
        {
            //Give the keyboard a set of virtual keys
            _keyDictionary = new KeyDictionary();
            foreach (var virtualKey in _keyDictionary.Keys.Values)
            { 
                //Wrap up each key event in our single VirtualKeyboard key press event.
                virtualKey.KeyPressEvent += (o, a) => KeyPressEvent(o, a);
            }

            //Allow the global input collection to access the virtual keyboard as an input
            KeyPressEvent += (o, e) => _inputEvents.HandleInputEvent(o, e);
        }

        public void DrawKeys(Panel parentControl)
        {
            foreach (var key in _keyDictionary.Keys.Values)
            {
                parentControl.Children.Add(key);
            }
        }

        public void HandleIncomingMessage(object sender, PianoKeyStrokeEventArgs e)
        {
            if (e == null) return;

            if (_keyDictionary.Keys.ContainsKey(e.midiKeyId))
            {
                switch (e.KeyStrokeType)
                {
                    case KeyStrokeType.KeyPress:
                        _keyDictionary.Keys[e.midiKeyId].SetKeyPressedColour(e.KeyVelocity);
                        break;
                    case KeyStrokeType.KeyRelease:
                        _keyDictionary.Keys[e.midiKeyId].SetDefaultKeyColour();
                        break;
                }
            }
        }
    }
}
