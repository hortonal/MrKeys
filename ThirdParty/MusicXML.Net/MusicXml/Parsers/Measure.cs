using System.Collections.Generic;
using MindTouch.Xml;

namespace MusicXml
{
	public class Measure
	{
		private readonly XDoc theDocument;
		private MeasureAttributes theAttributes;

		internal Measure(XDoc aDocument)
		{
			theDocument = aDocument;
		}

		public int Width
		{
			get { return theDocument["@width"].AsInt ?? -1; }
		}

		public IEnumerable<Note> Notes
		{
			get
			{
				List<Note> notes = new List<Note>();
				foreach (XDoc node in theDocument["note|backup"])
				{
                    notes.Add(new Note(node));

					
				}
				return notes;
			}
		}

		public MeasureAttributes Attributes
		{
			get
			{
				if (theAttributes == null)
				{
					XDoc attributes = theDocument["attributes"];
					if (!attributes.IsEmpty)
					{
						theAttributes = new MeasureAttributes(attributes);
					}
				}
				return theAttributes;
			}
		}
	}
}
