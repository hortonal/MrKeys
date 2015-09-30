using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using MusicXml;
using Common.Music;

namespace ScoreControlLibrary
{
    internal class StaffCleff
    {
        public char Sign { get; set; }
        public int Line { get; set; }
        public int OctaveChange { get; set; }
    }

    internal class Timing
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
    }

    internal class RestYCoords
    {
        public double StandardRestY { get; private set; }
        public double WholeRestY { get; private set; }

        public RestYCoords(double lowestLine_Y, double lineSpacing)
        {
            StandardRestY = lowestLine_Y - lineSpacing * 2.0;
            WholeRestY = lowestLine_Y - lineSpacing * 3.0;
        }
    }

    /// <summary>
    /// Simple class to keep track of basic layout and details of staff
    /// </summary>
    internal class Staff
    {
        public StaffCleff StaffClef { get; set; }
        public Timing Timing { get; set; }
        private RestYCoords RestYCoords { get; set; }
        public double LineSpacing { get; private set; }
        public double LowestLine_Y { get; private set; }
        public double HighestLine_Y { get { return LowestLine_Y - (ScoreLayoutDetails.NumberOfLines - 1) * LineSpacing; } }

        private RenderHelper _renderHelper;
        private NoteRenderHelper _noteRenderHelper;
        private double _defaultNoteSeparation;

        public Staff(RenderHelper renderHelper, double lineSpacing, double lowestLine_Y, double defaultNoteSeparation)
        {
            _renderHelper = renderHelper;
            LineSpacing = lineSpacing;
            LowestLine_Y = lowestLine_Y;
            RestYCoords = new RestYCoords(LowestLine_Y, LineSpacing);

            _noteRenderHelper = new NoteRenderHelper(_renderHelper, RestYCoords);

            _defaultNoteSeparation = defaultNoteSeparation;

            //Set some values that will never occur in practice, so when we first check to see if the attributes
            //in the xml are different to our staff, we'll always update the first time around
            StaffClef = new StaffCleff
            {
                Sign = 'x',
                Line = -1,
                OctaveChange = -1,
            };
            Timing = new Timing
            {
                Numerator = -1,
                Denominator = -1
            };
        }

        private enum ClefTypes { StandardTrebble, StandardBass, Other }

        private ClefTypes ClefType
        {
            get
            {
                if (StaffClef.Sign == 'G' && StaffClef.Line == 2) return ClefTypes.StandardTrebble;
                if (StaffClef.Sign == 'F' && StaffClef.Line == 4) return ClefTypes.StandardBass;
                return ClefTypes.Other;
            }
        }


        #region Staff Lines
        public void DrawStaffLines(double startX, double endX)
        {
            foreach (double y in StaffLine_YCoords())
            {
                Line line = new Line();
                line.X1 = 0;
                line.X2 = endX - startX;
                line.StrokeThickness = 1;
                line.Stroke = Brushes.Black;
                _renderHelper.RenderItemXY(line, startX, y);
            }
        }

        private IEnumerable<double> StaffLine_YCoords()
        {
            var retList = new List<double>();

            retList.Add(LowestLine_Y);                      //e.g. Staff 1: E
            retList.Add(LowestLine_Y - LineSpacing);        //e.g. Staff 1: G
            retList.Add(LowestLine_Y - LineSpacing * 2.0);  //e.g. Staff 1: B
            retList.Add(LowestLine_Y - LineSpacing * 3.0);  //e.g. Staff 1: D
            retList.Add(LowestLine_Y - LineSpacing * 4.0);  //e.g. Staff 1: F

            return retList;
        }
        #endregion 

        #region Note Addition Logic
        public void AddNote(Note note, double devisions, double noteTime)
        {
            double yCoord = CalculateYForNote(note);
            _noteRenderHelper.AddNote(note, Timing, devisions, noteTime, yCoord, ScoreLayoutDetails.DefaultNoteHeight, _defaultNoteSeparation);

            AddLedgerLines(note, devisions, noteTime, yCoord);
        }

        private void AddLedgerLines(Note note, double devisions, double noteTime, double yCoord)
        {
            //Options
            //= 1 line. Draw it
            //=1.5 lines. Draw 1 line
            if (HighestLine_Y - yCoord >= LineSpacing)
            {
                //Get remainder
                var numberToAdd = (int) Math.Round( (HighestLine_Y - yCoord) / LineSpacing);
                for (int i = 1; i <= numberToAdd; i++){
                    _noteRenderHelper.AddLedgerLine(noteTime, yCoord + LineSpacing * (i - 1));
                }
            }
            
            if (yCoord - LowestLine_Y >= LineSpacing)
            {
                var numberToAdd = (int)Math.Round( (yCoord - LowestLine_Y) / LineSpacing);
                for (int i = 1; i <= numberToAdd; i++)
                {
                    _noteRenderHelper.AddLedgerLine(noteTime, yCoord + LineSpacing * (i - 1));
                }
            }

            return;
        }
        
            
        private double CalculateYForNote(Note note)
        {
            if (note.IsRest) return CalculateYForRest(note);

            double middleC_Y;

            switch (ClefType)
            {
                case ClefTypes.StandardTrebble :  
                    middleC_Y =  LowestLine_Y + LineSpacing;
                    break;
                case ClefTypes.StandardBass :  
                    middleC_Y =  HighestLine_Y - LineSpacing;
                    break;
                default: 
                    middleC_Y =  LowestLine_Y + LineSpacing;
                    break;
            }
            
            int octaveOffsetFromMiddleC = note.Pitch.Octave - 4; //Octave 4 starts at middle C
            return middleC_Y - (octaveOffsetFromMiddleC * 7.0 + XmlMusicHelper.GetLineOffsetFromC(note)) * ScoreLayoutDetails.OffsetPerNote_Y;
        }

        private double CalculateYForRest(Note note)
        {
            return LowestLine_Y + LineSpacing * 2.0;
        }


        #endregion


        #region Clef Logic
        public void AddCleffChange(double noteTime)
        {
            double yPosition;
            double clefSize = LineSpacing * 4;
            double clefWidth = LineSpacing * 3.2;
            Type glyphType;

            switch (ClefType)
            {
                case ClefTypes.StandardBass:
                    yPosition = HighestLine_Y + LineSpacing;
                    glyphType = typeof(FClefGlyph);                    
                    break;
                default:
                    yPosition = LowestLine_Y - LineSpacing;
                    glyphType = typeof(GClefGlyph);                    
                    break;
            }
            
            TextBlock cleffVisual = _renderHelper.Glyphs.GetGlyph(glyphType, clefSize);

            _renderHelper.AddItemToRender(noteTime, cleffVisual, yPosition - cleffVisual.BaselineOffset, clefWidth, RenderItemType.Clef);

        }

        public bool ClefEquivalentToCurrent(Clef testClef)
        {
            return (testClef.Sign == StaffClef.Sign && testClef.Line == StaffClef.Line);
        }
        #endregion

        #region Timing Signatures Logic
        /// <summary>
        /// Adds a new timing to be rendered.  The caller should have already set the timing via the
        /// Timing property of this class
        /// </summary>
        /// <param name="noteTime"></param>
        public void AddTimingChange(double noteTime)
        {
            double fontSize = LineSpacing * 3.7; 

            TextBlock numeratorGlyph = _renderHelper.Glyphs.GetGlyph(Timing.Numerator, fontSize);
            TextBlock denominatorGlyph = _renderHelper.Glyphs.GetGlyph(Timing.Denominator, fontSize);
            
            _renderHelper.AddItemToRender(noteTime, numeratorGlyph,
                LowestLine_Y - numeratorGlyph.BaselineOffset - LineSpacing * 2.0, 
                fontSize, RenderItemType.TimeSignature);
            
            _renderHelper.AddItemToRender(noteTime, denominatorGlyph,
                LowestLine_Y - denominatorGlyph.BaselineOffset, fontSize, 
                RenderItemType.TimeSignature);
        }
        #endregion
    }
}
