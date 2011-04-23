using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using KeyBoardControlLibrary;
using Common.Events;

namespace MrKeys
{
    class SanfordUtils
    {
        public static PianoKeyStrokeEventArgs BuildKeyStrokeEventArgs(ChannelMessage channelMessage)
        {
            var pksea = new PianoKeyStrokeEventArgs();

            //If the key stroke is neither a press or a release, return a null object
            switch (channelMessage.Command)
            {
                case ChannelCommand.NoteOn: 
                    pksea.keyStrokeType = KeyStrokeType.KeyPress; 
                    break;
                case ChannelCommand.NoteOff: 
                    pksea.keyStrokeType = KeyStrokeType.KeyRelease; 
                    break; 
                default: return null;
            }

            pksea.midiKeyId = channelMessage.Data1;
            pksea.KeyVelocity = channelMessage.Data2;

            //Velocity 0 is effectively a key release
            if (pksea.KeyVelocity == 0) pksea.keyStrokeType = KeyStrokeType.KeyRelease; 

            return pksea;
        }
    }
}
