﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;
using Common.Music;
using System.Collections.ObjectModel;

namespace ScoreControlLibrary.SongEventParser
{
    public class XScoreNoteEventParser
    {
        private XScore _score;

        public XScoreNoteEventParser(XScore score)
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

                    double lastXmlNoteDuration = 0;

                    foreach (Note note in measure.Notes)
                    {
                        double noteDuration = note.Duration / currentDevisions;
                        //If the current note is part of a chord, we need to revert to the previous NoteTime
                        if (note.IsChord) currentNoteTime -= lastXmlNoteDuration;
                        
                        //After current note drawn, handle keeping track of noteTime in the piece
                        switch (note.NoteType)
                        {
                            case Note.NoteTypes.Note:
                                
                                AddSongNoteToSong(currentNoteTime, song, note, noteDuration);
                                currentNoteTime += noteDuration;
                                break;
                            case Note.NoteTypes.Backup:
                                currentNoteTime -= noteDuration;
                                break;
                            case Note.NoteTypes.Forward:
                                currentNoteTime += noteDuration;
                                break;
                            default:
                                currentNoteTime += noteDuration;
                                break;
                        }
                        lastXmlNoteDuration = noteDuration;
                    }
                }

            return song;
        }

        public static void AddSongNoteToSong(double noteTime, Song song, Note xmlNote, double duration)
        {

            var songNote = new SongNote();
            songNote.NoteTime = noteTime;
            songNote.PitchId = (xmlNote.Pitch.Octave + 1) * 12 + XmlMusicHelper.GetMidiIdOffsetFromC(xmlNote) + xmlNote.Pitch.Alter;
            songNote.Velocity = 100;
            
            songNote.Duration = duration;

            ICollection<SongNote> noteList;

            if (!song.TryGetValue(noteTime, out noteList))
            {
                noteList = new Collection<SongNote>();
                song.Add(noteTime, noteList);
            };

            noteList.Add(songNote);   
        }
    }
}
