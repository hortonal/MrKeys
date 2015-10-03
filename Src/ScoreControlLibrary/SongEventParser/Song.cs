
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.SongEventParser
{
    public class SongNoteList : List<SongNote>
    {
        public int Tempo { get; set; }
    }
}
