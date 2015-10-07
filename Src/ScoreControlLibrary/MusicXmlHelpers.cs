using System;
using System.Collections.Generic;
using System.Linq;
using MusicXml;
using System.Text;

namespace ScoreControlLibrary
{
    internal class MusicXmlHelpers
    {

        public static Part SelectPianoPart(XScore score)
        {
            return score.Parts.Last();
        }

        public static int GetNumberOfStaves(Part part)
        {
            //_score.Parts.Any(x => x.Measures.Any(y => y.Attributes.Staves > 1))
            return 2;
        }
    }
}
