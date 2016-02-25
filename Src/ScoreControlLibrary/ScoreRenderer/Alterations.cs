using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoreControlLibrary.ScoreRenderer
{
    internal enum NoteAlterationType { DoubleFlat, Flat, Natural, Sharp, DoubleSharp, Neutral }
    
    internal class Alterations
    {
        public NoteAlterationType A { get; set; }
        public NoteAlterationType B { get; set; }
        public NoteAlterationType C { get; set; }
        public NoteAlterationType D { get; set; }
        public NoteAlterationType E { get; set; }
        public NoteAlterationType F { get; set; }
        public NoteAlterationType G { get; set; }

        public Alterations()
        {
            A = NoteAlterationType.Natural;
            B = NoteAlterationType.Natural;
            C = NoteAlterationType.Natural;
            D = NoteAlterationType.Natural;
            E = NoteAlterationType.Natural;
            F = NoteAlterationType.Natural;
            G = NoteAlterationType.Natural;
        }
        
        public NoteAlterationType ForNote(char note)
        {
            switch (note)
            {
                case 'A':
                    return A;
                case 'B':
                    return B;
                case 'C':
                    return C;
                case 'D':
                    return D;
                case 'E':
                    return E;
                case 'F':
                    return F;
                case 'G':
                    return G;
            }
            return A;
        }

        public void SetAlteration(char note, NoteAlterationType newAlterationType)
        {
            switch (note)
            {
                case 'A':
                    A = newAlterationType;
                    return;
                case 'B':
                    B = newAlterationType;
                    return;
                case 'C':
                    C = newAlterationType;
                    return;
                case 'D':
                    D = newAlterationType;
                    return;
                case 'E':
                    E = newAlterationType;
                    return;
                case 'F':
                    F = newAlterationType;
                    return;
                case 'G':
                    G = newAlterationType;
                    return;
            }
        }

        public static Alterations CreateFromFifths(int fifths)
        {
            var alterations = new Alterations();

            if (fifths < 0) { alterations.B = NoteAlterationType.Flat; }
            if (fifths < -1) { alterations.E = NoteAlterationType.Flat; }
            if (fifths < -2) { alterations.A = NoteAlterationType.Flat; }
            if (fifths < -3) { alterations.D = NoteAlterationType.Flat; }
            if (fifths < -4) { alterations.G = NoteAlterationType.Flat; }
            if (fifths < -5) { alterations.C = NoteAlterationType.Flat; }
            if (fifths < -6) { alterations.F = NoteAlterationType.Flat; }
            
            if (fifths > 0) { alterations.F = NoteAlterationType.Sharp; }
            if (fifths > 1) { alterations.C = NoteAlterationType.Sharp; }
            if (fifths > 2) { alterations.G = NoteAlterationType.Sharp; }
            if (fifths > 3) { alterations.D = NoteAlterationType.Sharp; }
            if (fifths > 4) { alterations.A = NoteAlterationType.Sharp; }
            if (fifths > 5) { alterations.E = NoteAlterationType.Sharp; }
            if (fifths > 6) { alterations.B = NoteAlterationType.Sharp; }

            return alterations;
        }

        public static int AlterationToInt(NoteAlterationType type)
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

        public static NoteAlterationType IntToAlteration(int alteration)
        {
            switch (alteration)
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
