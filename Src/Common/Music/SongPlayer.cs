using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IO;
using Common.Events;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Common.Music
{
    public class SongPlayer
    {
        private Song _song;
        private IOutput _output;

        public SongPlayer(IOutput output, Song song)
        {
            _song = song;
            _output = output;
        }

        public void Play()
        {
            _song.Sort();


            //Build a dictionary of notes per note time
            //Should really do all this not on the gui thread...
            //var uniqueNoteTimes = _song.ConvertAll(x => x.NoteTime).Distinct();
            
            double lastNoteTime = 0;
            Collection<SongNote> simulatneousSongNotes = new Collection<SongNote>();
            
            //Iterate through song playing notes
            foreach (SongNote note in _song)
            {
                
                if (note.NoteTime == lastNoteTime)
                {
                    simulatneousSongNotes.Add(note);
                }
                else
                {
                    SendNoteEvents(simulatneousSongNotes);

                    //FIX ME!!!! PLEASE!!!
                    Thread.Sleep(CalculateNoteDuration(note));
                    simulatneousSongNotes = new Collection<SongNote>();
                }

                lastNoteTime = note.NoteTime;
            }
            //Finally send the last notes in the song
            //Probably a nicer way of handling this..
            SendNoteEvents(simulatneousSongNotes);

        }

        private void SendNoteEvents(Collection<SongNote> songNotes)
        {
            foreach (var songNote in songNotes)
            {
                var pianoKeyStrokeEventArgs = new PianoKeyStrokeEventArgs(songNote.PitchId, KeyStrokeType.KeyPress, songNote.Velocity);
                var pianoKeyStrokeReleaseEventArgs = new PianoKeyStrokeEventArgs(songNote.PitchId, KeyStrokeType.KeyRelease, songNote.Velocity);

                _output.Send(null, pianoKeyStrokeEventArgs);
                
                //Spark up a background worker to wait for the note to finish and release the key press
                var worker = new BackgroundWorker();
                worker.DoWork += (ob, arg) => { Thread.Sleep(CalculateNoteDuration(songNote)); };
                
                worker.RunWorkerCompleted += (ob, arg) =>
                {
                    _output.Send(null, pianoKeyStrokeReleaseEventArgs);
                };

                worker.RunWorkerAsync();
            }
        }

        private int CalculateNoteDuration(SongNote songNote)
        {
            return (int)songNote.Duration * 1000 * 60 / _song.Tempo; 
         } 
    }
}
