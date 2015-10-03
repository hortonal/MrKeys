using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.SongEventParser
{
    public class SongNote : IComparable<SongNote>
    {
        public double NoteTime { get; set; }
        public double Duration { get; set; }
        public int PitchId { get; set; }
        public int Velocity { get; set; }

        public int CompareTo(SongNote songNote)
        {
            return (int)(this.NoteTime * 1000.0 - songNote.NoteTime * 1000.0);
        }
    }
}
