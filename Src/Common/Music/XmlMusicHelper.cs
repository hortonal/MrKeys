using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;

namespace Common.Music
{
    public class XmlMusicHelper
    {

        public static int GetLineOffsetFromC(Note note)
        {
            switch (note.Pitch.Step)
            {
                case 'C': return 0;
                case 'D': return 1;
                case 'E': return 2;
                case 'F': return 3;
                case 'G': return 4;
                case 'A': return 5;
                case 'B': return 6;
                default: throw new ArgumentException("Note Step not parsable/not acceptable value : " + note);
            }
        }

        public static int GetMidiIdOffsetFromC(Note note)
        {
            switch (note.Pitch.Step)
            {
                case 'C': return 0;
                case 'D': return 2;
                case 'E': return 4;
                case 'F': return 5;
                case 'G': return 7;
                case 'A': return 9;
                case 'B': return 11;
                default: throw new ArgumentException("Note Step not parsable/not acceptable value : " + note);
            }
        }

        public static SongNote ConvertXmlNoteToSongNote(double noteTime, Note xmlNote)
        {
            var songNote = new SongNote();
            songNote.NoteTime = noteTime;
            songNote.PitchId = (xmlNote.Pitch.Octave + 1) * 12 + GetMidiIdOffsetFromC(xmlNote);
            songNote.Velocity = 100;
            songNote.Duration = xmlNote.Duration;
            
            return songNote;
        }
    }
}
