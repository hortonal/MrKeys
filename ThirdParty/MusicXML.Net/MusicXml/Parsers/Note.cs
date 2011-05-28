using System;
using MindTouch.Xml;

namespace MusicXml
{
	public class Note
	{
		private readonly XDoc theDocument;

		internal Note(XDoc aDocument)
		{
			theDocument = aDocument;
		}

        public bool IsBackup
        {
            get { return (theDocument.Name ?? "") == "backup"; }
        }

		public string Type
		{
			get { return theDocument["type"].AsText ?? String.Empty; }
		}
		public int Voice
		{
			get { return theDocument["voice"].AsInt ?? -1; }
		}
		public int Duration
		{
			get { return theDocument["duration"].AsInt ?? -1; }
		}
		public Lyric Lyric
		{
			get
			{
				XDoc lyric = theDocument["lyric"];
				return lyric.IsEmpty ? null : new Lyric(lyric);
			}
		}
        public int Staff
        {
            get { return theDocument["staff"].AsInt ?? -1; }
        }
        public string Stem
        {
            get { return theDocument["stem"].AsText ?? String.Empty; }
        }
        public double DefaultX
        { 
            get { return theDocument["@default-x"].AsDouble ?? double.NaN; }
        }
        public double DefaultY
        {
            get { return theDocument["@default-y"].AsDouble ?? double.NaN; }
        }

		public Pitch Pitch
		{
			get
			{
				XDoc pitch = theDocument["pitch"];
				return pitch.IsEmpty ? null : new Pitch(pitch);
			}
		}

        public override string ToString()
        {
            return string.Format("Voice={0}, Type={1}, Pitch{2}, Duration={3}, IsBackUp={4}, Staff={5}, x={6}, y={7}", Voice, Type, Pitch, Duration, IsBackup, Staff, DefaultX, DefaultY);
        }
	}
}
