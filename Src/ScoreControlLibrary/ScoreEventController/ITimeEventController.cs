﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IO;
using ScoreControlLibrary.SongEventParser;
using Common.Events;


namespace ScoreControlLibrary.ScoreEventController
{
    public interface ISongEventController
    {
        event SongEventHandler SongNoteEvent;
        event SongFinishedHandler Finished;

        void UpdateSong(Song songNoteList);
        void Play();
        void Pause();
        void Resume();
        void Stop();
        
    }

    public delegate void SongEventHandler(object sender, SongEventArgs e);
    public delegate void SongFinishedHandler(object sender, EventArgs e);

    public class SongEventArgs: EventArgs
    {
        public PianoKeyStrokeEventArgs NoteKeyStrokeEvenArguments { get; private set; }
        public double NextNoteTime { get; private set; }
        public double NoteTime { get; private set; }

        public SongEventArgs(PianoKeyStrokeEventArgs noteEventArgs, double noteTime, double nextNoteTime)
        {
            NoteKeyStrokeEvenArguments = noteEventArgs;
            NextNoteTime = nextNoteTime;
            NoteTime = noteTime;
        }

    }
}
