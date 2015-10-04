using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;
using System.Windows.Controls;


namespace ScoreControlLibrary
{
    
    public class ScoreRenderer
    {
        private Grid _scorePanel;
        private XScore _score;
        private RenderHelper _renderHelper;

        public ScoreRenderer(XScore score, Grid scorePanel)
        {
            this._score = score;
            this._scorePanel = scorePanel;
        }

        public void Render()
        {
            _renderHelper = new RenderHelper(_scorePanel);

            double currentNoteTime = 0;
            double currentDevisions = 0;
            
            var staffs = new Staffs(_renderHelper);
            
            staffs.Add(new Staff(_renderHelper, ScoreLayoutDetails.LineSpacing_Y, ScoreLayoutDetails.Staff1_FristLineY));

            if (_score == null) throw new Exception("Score is empty, doh..");

            //Only add a second stave if nec.
            if (_score.Parts.Any(x => x.Measures.Any(y => y.Attributes.Staves > 1)))
            {
                staffs.Add(new Staff(_renderHelper, ScoreLayoutDetails.LineSpacing_Y, ScoreLayoutDetails.Staff2_FristLineY));
            }
            
            staffs.AddBarLine(currentNoteTime);

            foreach (Part part in _score.Parts)
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

            double finalX = _renderHelper.RenderItems(ScoreLayoutDetails.DefaultMargin_X);
            foreach (var staff in staffs) staff.DrawStaffLines(ScoreLayoutDetails.DefaultMargin_X, finalX);
        }

        public double GetHorizontalOffsetForNoteTime(double noteTime)
        {
            return _renderHelper.GetHorizontalOffsetForNoteTime(noteTime);
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
