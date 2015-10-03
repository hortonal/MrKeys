using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.Glyphs
{
    internal interface IGlyph
    {
        string GlyphCode { get; }
    }

    internal class Glyph : IGlyph
    {
        public string GlyphCode {get; private set;}
        public Glyph(string glyphCode)
        {
            GlyphCode = glyphCode;
        }
    }

    //Defines the full list of glyphs to be used. Will probably grow with time
    internal class GClefGlyph : Glyph { public GClefGlyph(string glyphCode) : base(glyphCode) { } }
    internal class FClefGlyph : Glyph { public FClefGlyph(string glyphCode) : base(glyphCode) { } }
    internal class CClefGlyph : Glyph { public CClefGlyph(string glyphCode) : base(glyphCode) { } }
    internal class SharpGlyph : Glyph { public SharpGlyph(string glyphCode) : base(glyphCode) { } }
    internal class FlatGlyph : Glyph { public FlatGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NaturalGlyph : Glyph { public NaturalGlyph(string glyphCode) : base(glyphCode) { } }
    internal class DoubleSharpGlyph : Glyph { public DoubleSharpGlyph(string glyphCode) : base(glyphCode) { } }
    internal class DoubleFlatGlyph : Glyph { public DoubleFlatGlyph(string glyphCode) : base(glyphCode) { } }
    internal class WholeNoteGlyph : Glyph { public WholeNoteGlyph(string glyphCode) : base(glyphCode) { } }
    internal class HalfNoteGlyph : Glyph { public HalfNoteGlyph(string glyphCode) : base(glyphCode) { } }
    internal class RestWholeGlyph : Glyph { public RestWholeGlyph(string glyphCode) : base(glyphCode) { } }
    internal class RestHalfGlyph : Glyph { public RestHalfGlyph(string glyphCode) : base(glyphCode) { } }
    internal class RestQuarterGlyph : Glyph { public RestQuarterGlyph(string glyphCode) : base(glyphCode) { } }
    internal class RestEighthGlyph : Glyph { public RestEighthGlyph(string glyphCode) : base(glyphCode) { } }
    internal class Rest16thGlyph : Glyph { public Rest16thGlyph(string glyphCode) : base(glyphCode) { } }
    internal class Rest32ndGlyph : Glyph { public Rest32ndGlyph(string glyphCode) : base(glyphCode) { } }
    internal class Rest64thGlyph : Glyph { public Rest64thGlyph(string glyphCode) : base(glyphCode) { } }
    internal class Rest128thGlyph : Glyph { public Rest128thGlyph(string glyphCode) : base(glyphCode) { } }
    internal class WhiteNoteHeadGlyph : Glyph { public WhiteNoteHeadGlyph(string glyphCode) : base(glyphCode) { } }
    internal class BlackNoteHeadGlyph : Glyph { public BlackNoteHeadGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlagEighthGlyph : Glyph { public NoteFlagEighthGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlagSixteenthGlyph : Glyph { public NoteFlagSixteenthGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlag32ndGlyph : Glyph { public NoteFlag32ndGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlag64thGlyph : Glyph { public NoteFlag64thGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlag128thGlyph : Glyph { public NoteFlag128thGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlagEighthRevGlyph : Glyph { public NoteFlagEighthRevGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlagSixteenthRevGlyph : Glyph { public NoteFlagSixteenthRevGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlag32ndRevGlyph : Glyph { public NoteFlag32ndRevGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlag64thRevGlyph : Glyph { public NoteFlag64thRevGlyph(string glyphCode) : base(glyphCode) { } }
    internal class NoteFlag128thRevGlyph : Glyph { public NoteFlag128thRevGlyph(string glyphCode) : base(glyphCode) { } }
    internal class DotGlyph : Glyph { public DotGlyph(string glyphCode) : base(glyphCode) { } }
    internal class CommonTimeGlyph : Glyph { public CommonTimeGlyph(string glyphCode) : base(glyphCode) { } }
    internal class CutTimeGlyph : Glyph { public CutTimeGlyph(string glyphCode) : base(glyphCode) { } }
    internal class RepeatForwardGlyph : Glyph { public RepeatForwardGlyph(string glyphCode) : base(glyphCode) { } }
    internal class RepeatBackwardGlyph : Glyph { public RepeatBackwardGlyph(string glyphCode) : base(glyphCode) { } }      
}
