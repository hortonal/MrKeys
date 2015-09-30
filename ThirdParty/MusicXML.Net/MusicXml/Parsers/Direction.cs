using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Xml;

namespace MusicXml
{
    public class Direction
    {
        private readonly XDoc theDocument;

        internal Direction(XDoc aDocument)
        {
            theDocument = aDocument;
        }

        public DirectionPlacement Placement
        {
            get
            {
                switch (theDocument["@placement"].AsText)
                {
                    case "below":
                        return DirectionPlacement.Below;
                    default:
                        return DirectionPlacement.Above;
                }
            }
        }

        public int? Tempo
        {
            get { return theDocument["sound/@tempo"].AsInt; }
        }
    }

    public enum DirectionPlacement { Above, Below }
}
