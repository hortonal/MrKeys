using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace ScoreControlLibrary
{
    internal class ScoreLayoutDetails
    {
        //Vertical layout defaults
        private static double _lineSpacing_Y = 10; //e.g. space between E and G lines
        public static double LineSpacing_Y                     { get { return _lineSpacing_Y; } }
        public static double OffsetPerNote_Y                   { get { return _lineSpacing_Y / 2.0; } } //e.g. space between E and F
        public static double CenterStaffs_Y                    { get { return _lineSpacing_Y * 20.0; } }
        public static double NumberLinesBetweenCenterAndStaffs { get { return 4.0; } }
        public static double DefaultMargin_X                   { get { return 10; } }
        public static double DefaultNoteHeight                 { get { return LineSpacing_Y - 3;; } }
        public static double DefaultNoteWidth                  { get { return LineSpacing_Y; } }
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

    public class ScoreRenderer
    {
        private Grid scorePanel;
        private XScore score;
        

        public ScoreRenderer(XScore score, Grid scorePanel)
        {
            this.score = score;
            this.scorePanel = scorePanel;
        }

        public void Render()
        {
            var renderHelper = new RenderHelper(scorePanel);
            double currentNoteTime = 0;
            double currentDevisions = 0;


            var staffs = new Staffs(renderHelper);
                
            staffs.Add(new Staff(renderHelper, ScoreLayoutDetails.LineSpacing_Y, ScoreLayoutDetails.Staff1_FristLineY));
            staffs.Add(new Staff(renderHelper, ScoreLayoutDetails.LineSpacing_Y, ScoreLayoutDetails.Staff2_FristLineY));
            staffs.AddBarLine(currentNoteTime);

            foreach (Part part in score.Parts)
                foreach (Measure measure in part.Measures)
                {
                    MeasureAttributes attributes = measure.Attributes;
                    if (attributes != null)
                    {
                        if (attributes.Divisions != -1) currentDevisions = attributes.Divisions;

                        UpdateMeasureAttributes(attributes, staffs, currentNoteTime);
                    }

                    double lastNoteDuration = 0;

                    foreach (Note note in measure.Notes)
                    {
                        //If the current note is part of a chord, we need to revert to the previous NoteTime
                        if (note.IsChord) currentNoteTime -= lastNoteDuration / currentDevisions;

                        Staff staff = GetStaffFromNote(staffs, note);
                        if (staff != null && note.IsDrawableEntity)
                        {
                            staff.AddNote(note, currentDevisions, currentNoteTime);
                        }
                        
                        //After current note drawn, handle keeping track of noteTime in the piece
                        switch (note.NoteType)
                        {
                            case Note.NoteTypes.Backup: currentNoteTime -= note.Duration / currentDevisions;
                                break;
                            case Note.NoteTypes.Forward: currentNoteTime += note.Duration / currentDevisions;
                                break;
                            default: currentNoteTime += note.Duration / currentDevisions;
                                break;
                        }

                        lastNoteDuration = note.Duration;
                    }

                    staffs.AddBarLine(currentNoteTime);
                }

            double finalX = renderHelper.RenderItems(ScoreLayoutDetails.DefaultMargin_X);
            foreach (var staff in staffs) staff.DrawStaffLines(ScoreLayoutDetails.DefaultMargin_X, finalX);
        }

        private void UpdateMeasureAttributes(MeasureAttributes attributes, List<Staff> staffs, double noteTime)
        {
            
            //For each staff, refresh timings/clefs etc.
            for (int i = 0; i < staffs.Count; i++)
            {                
                Staff staff = staffs[i];
                Clef xmlClef = attributes.Clef(i + 1);  //xml attrubutes are base 1, not base 0...

                //Check the Clefs not been updated
                if (xmlClef != null && !staff.ClefEquivalentToCurrent(xmlClef))
                {
                    staff.StaffClef.Line = xmlClef.Line;
                    staff.StaffClef.Sign = xmlClef.Sign;
                    staff.AddCleffChange(noteTime);
                }

                Time xmlTime = attributes.Time;
                //Check the timing signature hasn't chagned.
                if (xmlTime != null)
                    if(xmlTime.Beats != staff.Timing.Numerator || xmlTime.Mode != staff.Timing.Denominator)
                    {
                        staff.Timing.Numerator = xmlTime.Beats;
                        staff.Timing.Denominator = xmlTime.Mode;
                    
                        staff.AddTimingChange(noteTime);   
                    }   
            }
        }

        private Staff GetStaffFromNote(List<Staff> staffs, Note note)
        {
            int staffId = note.Staff;
            if (staffId > 0 && staffId <= staffs.Count) return staffs[staffId-1];
            return null;
        }
    }
}
