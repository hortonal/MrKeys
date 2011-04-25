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
        public static PianoKeyStrokeEventArgs ConvertChannelMessageToKeyStrokeEventArgs(ChannelMessage channelMessage)
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

        public static ChannelMessage ConvertKeyStrokeEventArgsToChannelMessage(PianoKeyStrokeEventArgs keyStrokeEventArgs)
        {
            var channelCommand = new ChannelCommand();
            int data1 = keyStrokeEventArgs.midiKeyId;
            int data2 = keyStrokeEventArgs.KeyVelocity; 

            //If the key stroke is neither a press or a release, return a null object
            switch (keyStrokeEventArgs.keyStrokeType)
            {
                case KeyStrokeType.KeyPress:
                    channelCommand = ChannelCommand.NoteOn;
                    break;
                case KeyStrokeType.KeyRelease:
                    channelCommand = ChannelCommand.NoteOn;
                    data2 = 0;
                    break;
                default: return null;
            }

            return new ChannelMessage(channelCommand, 1, data1, data2);
        }
    }
}
