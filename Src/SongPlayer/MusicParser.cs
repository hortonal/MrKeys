using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;
using Common.Music;

namespace SongPlayer
{
    public class MusicParser
    {
        private XScore _score;

        public MusicParser(XScore score)
        {
            this._score = score;
        }

        public Song Parse()
        {
            var song = new Song();

            int? tempo = _score.Parts.First().Measures.First().Directions.Where(x => x.Placement == DirectionPlacement.Above).First().Tempo;

            if (tempo == null)
            {
                song.Tempo = 100;
            }
            else
            {
                song.Tempo = (int)tempo;
            }
            

            double currentNoteTime = 0;
            double currentDevisions = 4;

            foreach (Part part in _score.Parts)
                foreach (Measure measure in part.Measures)
                {
                    
                    MeasureAttributes attributes = measure.Attributes;
                    if (attributes != null)
                    {
                        if (attributes.Divisions != -1) currentDevisions = attributes.Divisions;
                    }

                    double lastNoteDuration = 0;

                    foreach (Note note in measure.Notes)
                    {
                        //If the current note is part of a chord, we need to revert to the previous NoteTime
                        if (note.IsChord) currentNoteTime -= lastNoteDuration / currentDevisions;
                        
                        //After current note drawn, handle keeping track of noteTime in the piece
                        switch (note.NoteType)
                        {
                            case Note.NoteTypes.Note:
                                song.Add(ConvertXmlNoteToSongNote(currentNoteTime, note));
                                currentNoteTime += note.Duration / currentDevisions;
                                break;
                            case Note.NoteTypes.Backup:
                                currentNoteTime -= note.Duration / currentDevisions;
                                break;
                            case Note.NoteTypes.Forward:
                                currentNoteTime += note.Duration / currentDevisions;
                                break;
                            default:
                                currentNoteTime += note.Duration / currentDevisions;
                                break;
                        }
                        lastNoteDuration = note.Duration;
                    }
                }

            return song;
        }

        public static SongNote ConvertXmlNoteToSongNote(double noteTime, Note xmlNote)
        {
            var songNote = new SongNote();
            songNote.NoteTime = noteTime;
            songNote.PitchId = (xmlNote.Pitch.Octave + 1) * 12 + XmlMusicHelper.GetMidiIdOffsetFromC(xmlNote);
            songNote.Velocity = 100;
            songNote.Duration = xmlNote.Duration;

            return songNote;
        }

    }
}
