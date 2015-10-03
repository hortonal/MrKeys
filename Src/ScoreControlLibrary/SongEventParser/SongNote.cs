using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.SongEventParser
{
    public class SongNote
    {
        public double NoteTime { get; set; }
        public double Duration { get; set; }
        public int PitchId { get; set; }
        public int Velocity { get; set; }
    }
}
