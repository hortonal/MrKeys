using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using ScoreControlLibrary.Glyphs;

namespace ScoreControlLibrary
{
    internal class NoteRenderHelper
    {
        private RenderHelper _renderHelper;
        private RestYCoords _restYCoords;

        public NoteRenderHelper(RenderHelper renderHelper,RestYCoords restYCoords)
        {
            _renderHelper = renderHelper;
            _restYCoords = restYCoords;
        }

        public void AddNote(Note note, Timing timing, int divisions, double noteTime, double yCoord)
        {
            
            AddAlteration(note, noteTime, yCoord);
            Type glyphType;
            double finalYCoord = yCoord;
            double xDistanceToNextObject = 0;
            var pointStemStart = new System.Windows.Point();
            var pointStemEnd = new System.Windows.Point();

            int pitchId = 0;

            if (note.IsRest)
            {
                glyphType = GetRestGlyphFromNote(note, timing, divisions);
                finalYCoord = GetRestYCoord(glyphType);
            }
            else
            {                
                xDistanceToNextObject = ScoreLayoutDetails.DefaultQuarterNoteSeparation * 2 * note.Duration / divisions;

                if (xDistanceToNextObject < ScoreLayoutDetails.DefaultNoteHeight) xDistanceToNextObject = ScoreLayoutDetails.LineSpacing_Y * 1.2;
                if (xDistanceToNextObject > ScoreLayoutDetails.LineSpacing_Y * 3) xDistanceToNextObject = ScoreLayoutDetails.LineSpacing_Y * 3;

                pitchId = SongEventParser.XScoreNoteEventParser.GetPitchIdFromNote(note);
                //Default 
                glyphType = typeof(BlackNoteHeadGlyph);
                switch (note.Type)
                {
                    case "half":
                        glyphType = typeof(HalfNoteGlyph);
                        break;
                    case "whole":
                        glyphType = typeof(WholeNoteGlyph);
                        break;
                    case "breve":
                        break;
                    case "long":
                        break;
                    case "":
                        break;
                }
            }

            if (note.Stem != "" || glyphType == typeof(BlackNoteHeadGlyph))
            {
                Line line = new Line();

                line.StrokeThickness = 1.2;
                line.Stroke = Brushes.Black;

                double stemHeight = ScoreLayoutDetails.DefaultNoteHeight * 3.0;
                switch(note.Stem)
                {
                    case "down":
                        pointStemStart.X = 0.1;
                        pointStemStart.Y = 0.1;
                        pointStemEnd.X = 0.1;
                        pointStemEnd.Y = stemHeight;
                        break;
                    default:
                        pointStemStart.X = ScoreLayoutDetails.DefaultNoteHeight - 0.1;
                        pointStemStart.Y = -0.1;
                        pointStemEnd.X = ScoreLayoutDetails.DefaultNoteHeight - 0.1;
                        pointStemEnd.Y = -stemHeight;
                        break;
                }

                line.X1 = pointStemStart.X;
                line.X2 = pointStemEnd.X;
                line.Y1 = pointStemStart.Y;
                line.Y2 = pointStemEnd.Y;
                
                _renderHelper.AddItemToRender(noteTime, line, finalYCoord, 0, 0, RenderItemType.NoteStem);
            }
            
            TextBlock tb = _renderHelper.Glyphs.GetGlyph(glyphType, ScoreLayoutDetails.DefaultNoteHeight * 3.3);
            
            _renderHelper.AddItemToRender(noteTime, tb, finalYCoord - tb.BaselineOffset, 
                xDistanceToNextObject, 0, RenderItemType.Note, pitchId);

            //Add placeholder line for beam (need to hook up in the renderer)
            // need to implement
        }

        private void AddAlteration(Note note, double noteTime, double yCoord)
        {
            Pitch pitch = note.Pitch;
            if (pitch == null) return;

            int alter = pitch.Alter;
            
            Type glyphType;
            switch (alter)
            {
                case -2:
                    glyphType = typeof(DoubleFlatGlyph);
                    break;
                case -1:
                    glyphType = typeof(FlatGlyph);
                    break;
                case 1:
                    glyphType = typeof(SharpGlyph);
                    break;
                case 2:
                    glyphType = typeof(DoubleSharpGlyph);
                    break;
                default:
                    return;
            }

            TextBlock tb = _renderHelper.Glyphs.GetGlyph(glyphType, ScoreLayoutDetails.DefaultAlterationScaling);

            _renderHelper.AddItemToRender(noteTime, tb, yCoord - tb.BaselineOffset, ScoreLayoutDetails.DefaultNoteHeight, 0,
                RenderItemType.Alteration, 0);
        }

        private Type GetRestGlyphFromNote(Note note, Timing timing, int divisions)
        {
            Type glyphType = null;
            switch (note.Type)
            {
                case "128th":
                    glyphType = typeof(Rest128thGlyph);
                    break;
                case "64th":
                    glyphType = typeof(Rest64thGlyph);
                    break;
                case "32nd":
                    glyphType = typeof(Rest32ndGlyph);
                    break;
                case "16th":
                    glyphType = typeof(Rest16thGlyph);
                    break;
                case "eighth":
                    glyphType = typeof(RestEighthGlyph);
                    break;
                case "quarter":
                    glyphType = typeof(RestQuarterGlyph);
                    break;
                case "half":
                    glyphType = typeof(RestHalfGlyph);
                    break;
                case "whole":
                    glyphType = typeof(RestWholeGlyph);
                    break;
                case "":
                    //Figure it out from Division
                    glyphType = CalculateRestType(note.Duration, timing, divisions);
                    break;
            }

            return glyphType;
        }

        public void AddLedgerLine(double noteTime, double yCoord)
        {
            var lineWidth = ScoreLayoutDetails.DefaultNoteHeight * 1.9;

            Line line = new Line();

            line.StrokeThickness = 0.6;
            line.Stroke = Brushes.Black;
            
            line.Y1 = 0;
            line.Y2 = 0;
            line.X1 = -lineWidth / 2;
            line.X2 = lineWidth / 2;
            
            _renderHelper.AddItemToRender(noteTime, line, yCoord, 0, (-1.2 * lineWidth / 4), RenderItemType.LedgerLine);
        }
        


        private Type CalculateRestType(int noteDuration, Timing timing, int divisions)
        {
            //Converts the rest duration into a fraction of a bar.  Times by 256 to handle switching and
            //handle the shortest possible rest
            int inverseDuration = Convert.ToInt16(1.0 * (noteDuration * timing.Denominator) / (divisions * 4 * timing.Numerator) * 256);
            switch (inverseDuration)
            {
                case 512:
                    return typeof(RestWholeGlyph);
                case 256:
                    return typeof(RestWholeGlyph); 
                case 128:
                    return typeof(RestHalfGlyph);
                case 64:
                    return typeof(RestQuarterGlyph); 
                case 32:
                    return typeof(RestEighthGlyph); 
                case 16:
                    return typeof(Rest16thGlyph); 
                case 8:
                    return typeof(Rest32ndGlyph); 
                case 4:
                    return typeof(Rest64thGlyph); 
                case 2:
                    return typeof(Rest128thGlyph); 
                default:
                    return typeof(RestWholeGlyph);
                    //Log warning...
                    //throw new ArgumentException("Can't currently handle rests of length 256th. Or other bad shit's happened");
            }
        }

        private double GetRestYCoord(Type glyphType)
        {
            double returnYCoord = _restYCoords.StandardRestY;

            if (glyphType == typeof(RestWholeGlyph))
            {
                returnYCoord = _restYCoords.WholeRestY;
            }
            
            return returnYCoord;
        }

    }
}
