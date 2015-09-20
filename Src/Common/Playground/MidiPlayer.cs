using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace Common.Playground
{

  //# CLASS: Item to open up the midi device and play clsNoteItems or clsNoteGroups
  class MidiPlayer:IDisposable
  {

    //# Module vars and consts

    /// <summary>
    /// Get or Set the Output Midi Device to use
    /// </summary>
    public OutputDevice MidiOutDevice
    {
      get;set;
    }

    /// <summary>
    /// Sends a note message to the Midi Device
    /// </summary>
    /// <param name="NoteName"></param>
    private void SendNoteMessage(string NoteName)
    {
    
            // Test operation of playing MIDI note
            using (OutputDevice outDevice = new OutputDevice(0))
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder();

                builder.Command = ChannelCommand.NoteOn;
                builder.MidiChannel = 0;
                builder.Data1 = 60;
                builder.Data2 = 127;
                builder.Build();

                outDevice.Send(builder.Result);              
    
                builder.Command = ChannelCommand.NoteOff;
                builder.Data2 = 0;
                builder.Build();

                outDevice.Send(builder.Result);
     
            } // END of Using
       } // End Proc


    /// <summary>
    /// Tidy up
    /// </summary>
    public void Dispose()
    {

    }



  } // End Class

  


}
