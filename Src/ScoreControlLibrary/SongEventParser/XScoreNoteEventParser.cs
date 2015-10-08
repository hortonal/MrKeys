using System;
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

            //foreach (Part part in _score.Parts)
            Part part = MusicXmlHelpers.SelectPianoPart(_score);

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
                            if(note.TieType == TieType.Stop)
                            {
                                ExtendSongNote(currentNoteTime, song, note, noteDuration);
                            } else
                            {
                                AddSongNoteToSong(currentNoteTime, song, note, noteDuration);
                            }
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

            song = BuildKeyReleases(song);

            return song;
        }

        private static Song BuildKeyReleases(Song song)
        {
            var songCopy = song.MakeCopy();
            SongNoteEventCollections noteCollections;
            SongNoteEventCollections releaseNoteCollections;
            double releaseNoteTime;

            foreach(var noteTime in song.Keys)
            {
                noteCollections = songCopy[noteTime];

                foreach (var note in noteCollections.KeyPresses)
                {
                    releaseNoteTime = noteTime + note.Duration;

                    //Now add release event at this time
                    if (!songCopy.TryGetValue(releaseNoteTime, out releaseNoteCollections))
                    {
                        releaseNoteCollections = new SongNoteEventCollections();
                        songCopy.Add(releaseNoteTime, releaseNoteCollections);
                    };

                    releaseNoteCollections.KeyReleases.Add(note);
                }
            }

            return songCopy;
        }

        public static void AddSongNoteToSong(double noteTime, Song song, Note xmlNote, double duration)
        {

            var songNote = new SongNote();
            songNote.NoteTime = noteTime;
            songNote.PitchId = GetPitchIdFromNote(xmlNote);
            songNote.Velocity = 100;
            
            songNote.Duration = duration;

            SongNoteEventCollections noteEventCollections;

            if (!song.TryGetValue(noteTime, out noteEventCollections))
            {
                noteEventCollections = new SongNoteEventCollections();
                song.Add(noteTime, noteEventCollections);
            };

            noteEventCollections.KeyPresses.Add(songNote);   
        }

        public static void ExtendSongNote(double noteTime, Song song, Note xmlNote, double duration)
        {
            int pitchId = GetPitchIdFromNote(xmlNote);
            
            //searching the list backwards will be faster..
            //I'm sure there's nice linq for this, but hey..
            foreach (double itemNoteTime in song.Keys.Reverse())
            {
                foreach(var note in song[itemNoteTime].KeyPresses)
                {
                    if (note.PitchId == pitchId)
                    {
                        note.Duration += duration;
                        return;
                    }
                }
            }
        }

        private static int GetPitchIdFromNote(Note note)
        {
            return (note.Pitch.Octave + 1) * 12 + XmlMusicHelper.GetMidiIdOffsetFromC(note) + note.Pitch.Alter;
        }

    }
}
