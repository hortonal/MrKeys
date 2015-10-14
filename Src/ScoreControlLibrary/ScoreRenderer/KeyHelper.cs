using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.ScoreRenderer
{
    internal enum NoteAlterationType { DoubleFlat, Flat, Natural, Sharp, DoubleSharp, Neutral }

    internal class KeyHelper
    {

        public static int NaturalAlteration(char note, int numberFiths)
        {
            return AlterationToInt(NaturalNoteType(note, numberFiths));
        }
        
        private static int AlterationToInt(NoteAlterationType type)
        {
            switch (type)
            {
                case NoteAlterationType.DoubleFlat: return -2;
                case NoteAlterationType.Flat: return -1;
                case NoteAlterationType.Natural: return 0;
                case NoteAlterationType.Sharp: return 1;
                case NoteAlterationType.DoubleSharp: return 2;
            }
            return 0;
        }
        
        private static NoteAlterationType NaturalNoteType(char note, int numberFiths)
        {
            
            switch (note)
            {
                case 'A':
                    if (numberFiths <= -3) return NoteAlterationType.Flat;
                    if (numberFiths >= 5) return NoteAlterationType.Sharp;
                    return NoteAlterationType.Natural;
                case 'B':
                    if (numberFiths <= -1) return NoteAlterationType.Flat;
                    if (numberFiths >= 7) return NoteAlterationType.Sharp;
                    return NoteAlterationType.Natural;
                case 'C':
                    if (numberFiths <= -6) return NoteAlterationType.Flat;
                    if (numberFiths >= 2) return NoteAlterationType.Sharp;
                    return NoteAlterationType.Natural;
                case 'D':
                    if (numberFiths <= -4) return NoteAlterationType.Flat;
                    if (numberFiths >= 4) return NoteAlterationType.Sharp;
                    return NoteAlterationType.Natural;
                case 'E':
                    if (numberFiths <= -2) return NoteAlterationType.Flat;
                    if (numberFiths >= 6) return NoteAlterationType.Sharp;
                    return NoteAlterationType.Natural;
                case 'F':
                    if (numberFiths <= -7) return NoteAlterationType.Flat;
                    if (numberFiths >= 1) return NoteAlterationType.Sharp;
                    return NoteAlterationType.Natural;
                case 'G':
                    if (numberFiths <= -5) return NoteAlterationType.Flat;
                    if (numberFiths >= 3) return NoteAlterationType.Sharp;
                    return NoteAlterationType.Natural;
            }
            return NoteAlterationType.Natural;
        }
    }

    
    
}
