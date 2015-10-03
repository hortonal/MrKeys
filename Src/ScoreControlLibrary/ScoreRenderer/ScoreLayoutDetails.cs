using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary
{
    internal class ScoreLayoutDetails
    {
        //Vertical layout defaults
        private static double _lineSpacing_Y = 10; //e.g. space between E and G lines
        public static double LineSpacing_Y { get { return _lineSpacing_Y; } }
        public static double OffsetPerNote_Y { get { return _lineSpacing_Y / 2.0; } } //e.g. space between E and F
        public static double CenterStaffs_Y { get { return _lineSpacing_Y * 15.0; } }
        public static double NumberLinesBetweenCenterAndStaffs { get { return 3.0; } }
        public static double DefaultMargin_X { get { return 10; } }
        public static double DefaultNoteHeight { get { return LineSpacing_Y - 1.3; ; } }
        //public static double DefaultNoteWidth { get { return LineSpacing_Y; } }
        public static double DefaultNoteSeparation { get { return LineSpacing_Y * 4; } }

        public static int NumberOfLines { get { return 5; } }

        public static double Staff1_FristLineY //Generally E line of trebble staff
        {
            get { return CenterStaffs_Y - NumberLinesBetweenCenterAndStaffs * LineSpacing_Y; }
        }
        public static double Staff2_FristLineY //Generally E line of trebble staff
        {
            get { return CenterStaffs_Y + (NumberLinesBetweenCenterAndStaffs + NumberOfLines - 1) * LineSpacing_Y; }
        }
    }
}
