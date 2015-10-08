
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.SongEventParser
{
    /// <summary>
    /// A dictionary of collection of notes. The key is the 'noteTime' associated with the notes. If multiple notes
    /// occur at the same time - i.e. a chord - we collect these together in the collection (makes the 
    /// timer logic a bit more efficient)
    /// </summary>
    public class Song : SortedDictionary<double, SongNoteEventCollections>
    {
        /// <summary>
        /// Beats per minute
        /// </summary>
        public int Tempo { get; set; }

        public Song() : base() { }

        private Song(SortedDictionary<double, SongNoteEventCollections> song, IComparer<double> comparer) : base(song, comparer) { }

        /// <summary>
        /// Makes a shallow copy of the dictionary elements. 
        /// Value types are immutable copies
        /// All other reference types are reference copies
        /// </summary>
        /// <returns></returns>
        public Song MakeCopy()
        {
            var Song = new Song(this, Comparer);
            Song.Tempo = Tempo;
            return Song;
        }
    }
}
