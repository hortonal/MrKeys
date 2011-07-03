using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.Glyphs
{
    internal class MusicFontCharacters
    {
        private Dictionary<Type, IGlyph> glyphs;

        //Clefs
        public GClefGlyph GClef { get; protected set; }
        public FClefGlyph FClef { get; protected set; }
        public CClefGlyph CClef { get; protected set; }

        //Accidentals
        public SharpGlyph Sharp { get; protected set; }
        public FlatGlyph Flat { get; protected set; }
        public NaturalGlyph Natural { get; protected set; }
        public DoubleSharpGlyph DoubleSharp { get; protected set; }
        public DoubleFlatGlyph DoubleFlat { get; protected set; }
        public WholeNoteGlyph WholeNote { get; protected set; }
        public HalfNoteGlyph HalfNote { get; protected set; }

        //Rests
        public RestWholeGlyph RestWhole { get; protected set; }
        public RestHalfGlyph RestHalf { get; protected set; }
        public RestQuarterGlyph RestQuarter { get; protected set; }
        public RestEighthGlyph RestEighth { get; protected set; }
        public Rest16thGlyph Rest16th { get; protected set; }
        public Rest32ndGlyph Rest32nd { get; protected set; }
        public Rest64thGlyph Rest64th { get; protected set; }
        public Rest128thGlyph Rest128th { get; protected set; }

        //Notes/Other
        public WhiteNoteHeadGlyph WhiteNoteHead { get; protected set; }
        public BlackNoteHeadGlyph BlackNoteHead { get; protected set; }
        public NoteFlagEighthGlyph NoteFlagEighth { get; protected set; }
        public NoteFlagSixteenthGlyph NoteFlagSixteenth { get; protected set; }
        public NoteFlag32ndGlyph NoteFlag32nd { get; protected set; }
        public NoteFlag64thGlyph NoteFlag64th { get; protected set; }
        public NoteFlag128thGlyph NoteFlag128th { get; protected set; }
        public NoteFlagEighthRevGlyph NoteFlagEighthRev { get; protected set; }
        public NoteFlagSixteenthRevGlyph NoteFlagSixteenthRev { get; protected set; }
        public NoteFlag32ndRevGlyph NoteFlag32ndRev { get; protected set; }
        public NoteFlag64thRevGlyph NoteFlag64thRev { get; protected set; }
        public NoteFlag128thRevGlyph NoteFlag128thRev { get; protected set; }
        public DotGlyph Dot { get; protected set; }
        public CommonTimeGlyph CommonTime { get; protected set; }
        public CutTimeGlyph CutTime { get; protected set; }
        public RepeatForwardGlyph RepeatForward { get; protected set; }
        public RepeatBackwardGlyph RepeatBackward { get; protected set; }

        public MusicFontCharacters()
        {
            glyphs = new Dictionary<Type, IGlyph>();
        }

        //The inheriting class should set all the glyph mappings and then build the dictionary
        public void InitialiseGlyphDictionary()
        {
            glyphs.Add(typeof(GClefGlyph), GClef);
            glyphs.Add(typeof(FClefGlyph), FClef);
            glyphs.Add(typeof(CClefGlyph), CClef);
            glyphs.Add(typeof(SharpGlyph), Sharp);
            glyphs.Add(typeof(FlatGlyph), Flat);
            glyphs.Add(typeof(NaturalGlyph), Natural);
            glyphs.Add(typeof(DoubleSharpGlyph), DoubleSharp);
            glyphs.Add(typeof(DoubleFlatGlyph), DoubleFlat);
            glyphs.Add(typeof(WholeNoteGlyph), WholeNote);
            glyphs.Add(typeof(HalfNoteGlyph), HalfNote);
            glyphs.Add(typeof(RestWholeGlyph), RestWhole);
            glyphs.Add(typeof(RestHalfGlyph), RestHalf);
            glyphs.Add(typeof(RestQuarterGlyph), RestQuarter);
            glyphs.Add(typeof(RestEighthGlyph), RestEighth);
            glyphs.Add(typeof(Rest16thGlyph), Rest16th);
            glyphs.Add(typeof(Rest32ndGlyph), Rest32nd);
            glyphs.Add(typeof(Rest64thGlyph), Rest64th);
            glyphs.Add(typeof(Rest128thGlyph), Rest128th);
            glyphs.Add(typeof(WhiteNoteHeadGlyph), WhiteNoteHead);
            glyphs.Add(typeof(BlackNoteHeadGlyph), BlackNoteHead);
            glyphs.Add(typeof(NoteFlagEighthGlyph), NoteFlagEighth);
            glyphs.Add(typeof(NoteFlagSixteenthGlyph), NoteFlagSixteenth);
            glyphs.Add(typeof(NoteFlag32ndGlyph), NoteFlag32nd);
            glyphs.Add(typeof(NoteFlag64thGlyph), NoteFlag64th);
            glyphs.Add(typeof(NoteFlag128thGlyph), NoteFlag128th);
            glyphs.Add(typeof(NoteFlagEighthRevGlyph), NoteFlagEighthRev);
            glyphs.Add(typeof(NoteFlagSixteenthRevGlyph), NoteFlagSixteenthRev);
            glyphs.Add(typeof(NoteFlag32ndRevGlyph), NoteFlag32ndRev);
            glyphs.Add(typeof(NoteFlag64thRevGlyph), NoteFlag64thRev);
            glyphs.Add(typeof(NoteFlag128thRevGlyph), NoteFlag128thRev);
            glyphs.Add(typeof(DotGlyph), Dot);
            glyphs.Add(typeof(CommonTimeGlyph), CommonTime);
            glyphs.Add(typeof(CutTimeGlyph), CutTime);
            glyphs.Add(typeof(RepeatForwardGlyph), RepeatForward);
            glyphs.Add(typeof(RepeatBackwardGlyph), RepeatBackward);
        }

        public IGlyph GetGlyphInstance(Type glyphType)
        {
            if (glyphs.ContainsKey(glyphType))
                return glyphs[glyphType];
            throw new ArgumentException("Glyph Type Dictionary doesn't contain an instance for: " + glyphType);
        }
    }
}
