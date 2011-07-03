using MindTouch.Xml;

namespace MusicXml
{
	public class MeasureAttributes
	{
		private readonly XDoc theDocument;

		internal MeasureAttributes(XDoc aDocument)
		{
			theDocument = aDocument;
		}

        public int Divisions
        {
            get { return theDocument["divisions"].AsInt ?? -1; }
        }

		public Key Key
		{
			get
			{
				XDoc key = theDocument["key"];
				return key.IsEmpty ? null : new Key(key);
			}
		}
		public Time Time
		{
			get
			{
				XDoc time = theDocument["time"];
				return time.IsEmpty ? null : new Time(time);
			}
		}
		public Clef Clef(int staffNumber)
		{
            XDoc clef;
            clef = theDocument[string.Format("clef[@number={0}]", staffNumber)];

			return clef.IsEmpty ? null : new Clef(clef);
			
		}
        public int Staves { get { return theDocument["staves"].AsInt ?? 1; } }
	}
}
