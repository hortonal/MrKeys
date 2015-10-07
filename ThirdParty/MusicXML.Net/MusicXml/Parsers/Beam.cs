using System;
using MindTouch.Xml;

namespace MusicXml
{
    public class Beam
    {
        private readonly XDoc theDocument;

        internal Beam(XDoc aDocument)
        {
            theDocument = aDocument;
        }
        
        public int? Number
        {
            get { return theDocument["@number"].AsInt; }
        }


    }

    public enum BeamType { Begin, End}
}
