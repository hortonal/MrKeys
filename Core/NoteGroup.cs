using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace MrKeys
{
  
  /// <summary>
  /// Class to hold a group of NoteItems
  /// and related methods
  /// </summary>
  class NoteGroup
  {

    /// <summary>
    /// Import Track of Midi Events - will import the Notes only
    /// </summary>
    /// <param name="Track">Midi Track object</param>
    public void ImportTrack(Track Track)
    {

      // Init vars
      int EventIndex;
      int EventCount;
      MidiEvent EventItem;
      ChannelMessage Msg;
      string Message;

      // Get info
      EventCount = Track.Count;

      // Go through the list of events
      for (EventIndex = 0; EventIndex <EventCount; EventIndex++)
      {
        EventItem = Track.GetMidiEvent(EventIndex);
        Message = EventItem.MidiMessage.ToString();
        Console.WriteLine(EventItem.MidiMessage.ToString());
      }
        

      } // End Proc

  }
}
