using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.Glyphs
{
    internal class MuseScoreCharacters : MusicFontCharacters
    {
        public MuseScoreCharacters()
        {
            GClef = new GClefGlyph("\xE19E");
            FClef = new FClefGlyph("\xE19C");
            CClef = new CClefGlyph("\xE19A");
            Sharp = new SharpGlyph("\xE10E");
            Flat = new FlatGlyph("\xE114");
            Natural = new NaturalGlyph("\xE113");
            DoubleSharp = new DoubleSharpGlyph("\xE112");
            DoubleFlat = new DoubleFlatGlyph("\xE11A");

            RestWhole = new RestWholeGlyph("\xE100");
            RestHalf = new RestHalfGlyph("\xE101");
            RestQuarter = new RestQuarterGlyph("\xE107");
            RestEighth = new RestEighthGlyph("\xE109");
            Rest16th = new Rest16thGlyph("\xE10A");
            Rest32nd = new Rest32ndGlyph("\xE10B");
            Rest64th = new Rest64thGlyph("\xE10C");
            Rest128th = new Rest128thGlyph("\xE10D");

            WhiteNoteHead = new WhiteNoteHeadGlyph("\xE12C");
            BlackNoteHead = new BlackNoteHeadGlyph("\xE12D");
            WholeNote = new WholeNoteGlyph("\xE12B");
            HalfNote = new HalfNoteGlyph("\xE12C");

            NoteFlagEighth = new NoteFlagEighthGlyph("1");
            NoteFlagSixteenth = new NoteFlagSixteenthGlyph("2");
            NoteFlag32nd = new NoteFlag32ndGlyph("3");
            NoteFlag64th = new NoteFlag64thGlyph("4");
            NoteFlag128th = new NoteFlag128thGlyph("5");
            NoteFlagEighthRev = new NoteFlagEighthRevGlyph("!");
            NoteFlagSixteenthRev = new NoteFlagSixteenthRevGlyph("@");
            NoteFlag32ndRev = new NoteFlag32ndRevGlyph("#");
            NoteFlag64thRev = new NoteFlag64thRevGlyph("\xE193");
            NoteFlag128thRev = new NoteFlag128thRevGlyph("%");
            Dot = new DotGlyph(".");
            CommonTime = new CommonTimeGlyph("c");
            CutTime = new CutTimeGlyph("C");
            RepeatForward = new RepeatForwardGlyph(@"\");
            RepeatBackward = new RepeatBackwardGlyph(@"l");

            base.InitialiseGlyphDictionary();
        }
    }
}
