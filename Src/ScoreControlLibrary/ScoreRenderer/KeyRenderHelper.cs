using MusicXml;
using ScoreControlLibrary.Glyphs;
using System.Windows.Controls;
using System;

namespace ScoreControlLibrary.ScoreRenderer
{
    internal class KeyRenderHelper
    {
        private Key _currentKey;

        RenderHelper _renderHelper;
        public KeyRenderHelper(RenderHelper renderHelper)
        {
            _renderHelper = renderHelper;
            _currentKey = null;
        }

        public void SetKey(double noteTime, Key key, double lowEReference_Y)
        {
            if (key == null) return;

            _currentKey = key;

            if (key.Fifths < 0) AddKeyItem(noteTime, FlatGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 2, ScoreLayoutDetails.DefaultNoteHeight * 0);
            if (key.Fifths < -1) AddKeyItem(noteTime, FlatGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 3.5, ScoreLayoutDetails.DefaultNoteHeight * 1);
            if (key.Fifths < -2) AddKeyItem(noteTime, FlatGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 1.5, ScoreLayoutDetails.DefaultNoteHeight * 2);
            if (key.Fifths < -3) AddKeyItem(noteTime, FlatGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 3, ScoreLayoutDetails.DefaultNoteHeight * 3);
            if (key.Fifths < -4) AddKeyItem(noteTime, FlatGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 1, ScoreLayoutDetails.DefaultNoteHeight * 4);
            if (key.Fifths < -5) AddKeyItem(noteTime, FlatGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 2.5, ScoreLayoutDetails.DefaultNoteHeight * 5);
            if (key.Fifths < -6) AddKeyItem(noteTime, FlatGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 0.5, ScoreLayoutDetails.DefaultNoteHeight * 6);
            //support up to -11....

            if (key.Fifths > 0) AddKeyItem(noteTime, SharpGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 4, ScoreLayoutDetails.DefaultNoteHeight * 0);
            if (key.Fifths > 1) AddKeyItem(noteTime, SharpGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 2.5, ScoreLayoutDetails.DefaultNoteHeight * 1);
            if (key.Fifths > 2) AddKeyItem(noteTime, SharpGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 4.5, ScoreLayoutDetails.DefaultNoteHeight * 2);
            if (key.Fifths > 3) AddKeyItem(noteTime, SharpGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 3, ScoreLayoutDetails.DefaultNoteHeight * 3);
            if (key.Fifths > 4) AddKeyItem(noteTime, SharpGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 1.5, ScoreLayoutDetails.DefaultNoteHeight * 4);
            if (key.Fifths > 5) AddKeyItem(noteTime, SharpGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 3.5, ScoreLayoutDetails.DefaultNoteHeight * 5);
            if (key.Fifths > 6) AddKeyItem(noteTime, SharpGlyph(), lowEReference_Y - ScoreLayoutDetails.LineSpacing_Y * 2, ScoreLayoutDetails.DefaultNoteHeight * 6);
            //support up to 11....
        }

        private void AddKeyItem(double noteTime, TextBlock tb, double line_Y, double xOffset)
        {
            _renderHelper.AddItemToRender(noteTime, tb, line_Y - tb.BaselineOffset, xOffset, xOffset, RenderItemType.Key);
        }

        private TextBlock FlatGlyph()
        {
            return _renderHelper.Glyphs.GetGlyph(typeof(FlatGlyph), ScoreLayoutDetails.DefaultAlterationScaling);
        }
        private TextBlock DoubleFlatGlyph()
        {
            return _renderHelper.Glyphs.GetGlyph(typeof(DoubleFlatGlyph), ScoreLayoutDetails.DefaultAlterationScaling);
        }
        private TextBlock SharpGlyph()
        {
            return _renderHelper.Glyphs.GetGlyph(typeof(SharpGlyph), ScoreLayoutDetails.DefaultAlterationScaling);
        }
        private TextBlock DoubleSharpGlyph()
        {
            return _renderHelper.Glyphs.GetGlyph(typeof(DoubleSharpGlyph), ScoreLayoutDetails.DefaultAlterationScaling);
        }

        public NoteAlterationType GetAlteration(Note note)
        {
            if (note.Pitch == null) return NoteAlterationType.Natural;

            //Has note been adjusted in this measure?

            //Check if note is natural in key. If so, do nothing
            int naturalAlteration = KeyHelper.NaturalAlteration(note.Pitch.Step, _currentKey.Fifths);

            //Do nothing if natural
            if (note.Pitch.Alter - naturalAlteration == 0) return NoteAlterationType.Natural;

            if (note.Pitch.Alter == 0) return NoteAlterationType.Neutral;
            
            switch (note.Pitch.Alter - naturalAlteration)
            {
                case -2: return NoteAlterationType.DoubleFlat;
                case -1: return NoteAlterationType.Flat;
                case 0: return NoteAlterationType.Natural;
                case 1: return NoteAlterationType.Sharp;
                case 2: return NoteAlterationType.DoubleSharp;
            }

            return NoteAlterationType.Natural;
        }
    }
}
