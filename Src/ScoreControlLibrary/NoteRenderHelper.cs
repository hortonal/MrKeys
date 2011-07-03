using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

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

        public void AddNote(Note note, Timing timing, double devisions, double noteTime, double yCoord, double defaultNoteSize)
        {
            Type glyphType = typeof(BlackNoteHeadGlyph);
            double finalYCoord = yCoord;
            double defaultNoteWidth = 20.0;

            if (note.IsRest)
            {
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
                    case "eigth":
                        glyphType = typeof(RestEighthGlyph);
                        break;
                    case "quarter":
                        glyphType = typeof(RestQuarterGlyph);
                        break;
                    case "half":
                        glyphType = typeof(RestHalfGlyph);
                        defaultNoteWidth = defaultNoteWidth * 2;
                        break;
                    case "whole":
                        glyphType = typeof(RestWholeGlyph);
                        defaultNoteWidth = defaultNoteWidth * 4;
                        break;
                    case "":
                        glyphType = CalculateRestType(note.Duration, timing, devisions);
                        break;
                }
                finalYCoord = GetRestYCoord(glyphType);
            }
            else
            {
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

            if (note.Stem != "")
            {
                Line line = new Line();

                line.StrokeThickness = 1.2;
                line.Stroke = Brushes.Black;
                
                double stemHeight = defaultNoteSize * 3.0;
                switch(note.Stem)
                {
                    case "down":
                        line.Y1 = 0.1;
                        line.Y2 = stemHeight;
                        line.X1 = 0.1;
                        line.X2 = 0.1;
                        break;
                    case "up":
                        line.Y1 = - 0.1;
                        line.Y2 = - stemHeight;
                        line.X1 = defaultNoteSize - 0.1;
                        line.X2 = defaultNoteSize - 0.1;
                        break;
                }

                _renderHelper.AddItemToRender(noteTime, line, finalYCoord, 0, RenderItemType.NoteStem);
                
            }
            
            TextBlock tb = _renderHelper.Glyphs.GetGlyph(glyphType, defaultNoteSize * 3.3);

            _renderHelper.AddItemToRender(noteTime, tb, finalYCoord - tb.BaselineOffset, 20.0, RenderItemType.Note);
        }

        private Type CalculateRestType(int noteDuration, Timing timing, double devisions)
        {
            //Converts the rest duration into a fraction of a bar.  Times by 256 to handle switching and
            //handle the shortest possible rest
            switch ( Convert.ToInt16((noteDuration * timing.Denominator) / (devisions * 4 * timing.Numerator) * 256))
            {
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
                    throw new ArgumentException("Can't currently handle rests of length 256th. Or other bad shit's happened");
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
