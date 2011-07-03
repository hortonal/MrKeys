using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;
using ScoreControlLibrary.Glyphs;

namespace ScoreControlLibrary
{
    internal class MusicGlyphs
    {
        private Dictionary<Type, FontFamily> _glyphTypeFontFamilies;
        private Dictionary<FontFamily, MusicFontCharacters> _FontFamiliesCharacters;

        private FontFamily _defaultFontFamily;

        public MusicGlyphs()
        {
            
            var poliFontFamily = new FontFamily(new Uri("pack://application:,,,/ScoreControlLibrary;component/"), "./Resources/#Polihymnia");
            var museFontFamily = new FontFamily(new Uri("pack://application:,,,/ScoreControlLibrary;component/"), "./Resources/#MScore 20");

            _defaultFontFamily = museFontFamily;

            _glyphTypeFontFamilies = new Dictionary<Type, FontFamily>();
            _FontFamiliesCharacters = new Dictionary<FontFamily, MusicFontCharacters>();

            _FontFamiliesCharacters.Add(museFontFamily, new MuseScoreCharacters());

            //If you want certain glyphs to come from other font families - add
            //them to this dictionary:
            //_glyphTypeFontFamilies.Add(typeof(CClefGlyph), museFontFamily);
        }

        public TextBlock GetGlyph(int number, double fontSize)
        {
            TextBlock tb = new TextBlock();
            tb.FontFamily = GetFontFamily(typeof(int));
            tb.Text = number.ToString();
            tb.FontSize = fontSize;
            tb.BaselineOffset = fontSize * GetBaseLineOffsetFactor(typeof(int));
            return tb;
        }

        public TextBlock GetGlyph(Type glyphType, double fontSize)
        {
            IGlyph glyph = GetGlyphInstance(glyphType);
            TextBlock tb = new TextBlock();
            tb.FontFamily = GetFontFamily(glyphType);
            tb.Text = glyph.GlyphCode;
            tb.FontSize = fontSize;
            tb.BaselineOffset = fontSize * GetBaseLineOffsetFactor(glyphType);
            return tb;
        }

        private FontFamily GetFontFamily(Type glyphType)
        {
            if (_glyphTypeFontFamilies.ContainsKey(glyphType))
                return _glyphTypeFontFamilies[glyphType];
            return _defaultFontFamily;
        }
        
        private IGlyph GetGlyphInstance(Type glyphType)
        {
            FontFamily fontFamily = GetFontFamily(glyphType);
            MusicFontCharacters fontCharacters;
            
            if (_FontFamiliesCharacters.ContainsKey(fontFamily))
                fontCharacters = _FontFamiliesCharacters[fontFamily];
            else throw new ArgumentException("No font character set for font family");

            return fontCharacters.GetGlyphInstance(glyphType);
        }

        /// <summary>
        /// Gets Basline of font used for glyphType
        /// </summary>
        /// <param name="fontSize"></param>
        /// <param name="glyphType"></param>
        /// <returns></returns>
        private double GetBaseLineOffsetFactor(Type glyphType)
        {
            return GetFontFamily(glyphType).Baseline;
        }
    }
}
