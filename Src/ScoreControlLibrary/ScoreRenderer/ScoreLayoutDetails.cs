using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary
{
    internal class ScoreLayoutDetails
    {
        //Vertical layout defaults
        private static double BasicUnit = 14; //e.g. space between E and G lines - forms the basic unit for all rendering
        public static double LineSpacing_Y { get { return BasicUnit; } }
        public static double OffsetPerNote_Y { get { return LineSpacing_Y / 2.0; } } //e.g. space between E and F
        public static double CenterStaffs_Y { get { return LineSpacing_Y * 15.0; } }
        public static double NumberLinesBetweenCenterAndStaffs { get { return 3.0; } }
        public static double DefaultMargin_X { get { return 10; } }
        public static double DefaultNoteHeight { get { return BasicUnit * (1 - 0.13); ; } }
        //public static double DefaultNoteWidth { get { return LineSpacing_Y; } }
        public static double DefaultQuarterNoteSeparation { get { return LineSpacing_Y * 2.5; } }

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
