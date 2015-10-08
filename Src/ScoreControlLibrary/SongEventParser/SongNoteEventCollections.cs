using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.SongEventParser
{
    public class SongNoteEventCollections
    {
        public ICollection<SongNote> KeyPresses { get; set; }
        public ICollection<SongNote> KeyReleases { get; set; }

        public SongNoteEventCollections()
        {   
            KeyPresses = new Collection<SongNote>();
            KeyReleases = new Collection<SongNote>();
        }
    }
}
