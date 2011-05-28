using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class NoteItem
    {
        //# PURPOSE: Class to represent a music note in a sequence
        //# REF:     http://tomscarff.110mb.com/midi_analyser/midi_note_numbers_for_octaves.htm

        private int m_intNoteId;   // Unique Id for this note in a given sequence
        private int m_intNoteNo;   // Note Number (currently, midi note number is used) 
        private int m_intDuration; // Duration in ticks of the note
        private int m_intVelocity; // Velocity of the note
        private int m_intPosition; // Tick position within the sequence
        
        //# PROPERTY: Note ID of this note
        public int NoteId
        {
            get
            {
                return m_intNoteId;
            }
            set
            {
                m_intNoteId = value;
            }
        }

        //# PROPERTY: Note Number
        public int NoteNo
        {
            get
            {
                return m_intNoteNo;
            }
            set
            {
                m_intNoteNo = value;
            }
        }

        //# PROPERTY: Note Position
        public int Position
        {
          get
          {
            return m_intPosition;
          }
          set
          {
            m_intPosition = value;
          }
        }

        //# PROPERTY: Velocity
        public int Velocity
        {
          get
          {
            return m_intVelocity;
          }
          set
          {
            m_intVelocity = value;
          }
        }

        //# PROPERTY: Duration
        public int Duration
        {
          get
          {
            return m_intDuration;
          }
          set
          {
            m_intDuration = value;
          }
        }

        //# PROPERTY: Max Note Number
        public int MaxNoteNo
        {
          get
          {
            return 127;
          }
        }

        //# PROPRETY: Min Note Number
        public int MinNoteNo
        {
          get
          {
            return 0;
          }
        }


        //# PROPERTY: MIDI Note Number of this note
        public int MidiNote
        {
          get
            {
              return m_intNoteNo;
            }
          set 
            {
              m_intNoteNo = value;
            }
        }

        //# METHOD: Convert Number to Name
        public string ConvNoToName(int intNoteNo)
        {
          return pConvNoToName(intNoteNo);
        }

        //# METHOD: Convert Name to Number
        public int ConvNameToNo(string strNoteName)
        {
          return pConvNameToNo(strNoteName);
        }


        //# METHOD: Converts number to name
        private string pConvNoToName(int intNoteNo)
        {

          // Init
          string strNoteName;
          int intOctave;
          int intOffset;

          // Get the octave
          intOctave = (intNoteNo / 12) - 1;
          
          // Get the offset
          intOffset = intNoteNo % 12;
          
          // Work out the name from the Offset
          switch (intOffset)
          {
            case 0:
              strNoteName = "C";
              break;
            case 1:
              strNoteName= "C#";
              break;
            case 2:
              strNoteName = "D";
              break;
            case 3:
              strNoteName = "D#";
              break;
            case 4:
              strNoteName = "E";
              break;
            case 5:
              strNoteName = "F";
              break;
            case 6:
              strNoteName = "F#";
              break;
            case 7:
              strNoteName ="G";
              break;
            case 8:
              strNoteName = "G#";
              break;
            case 9:
              strNoteName = "A";
              break;
            case 10:
              strNoteName = "A#";
              break;
            case 11:
              strNoteName = "B";
              break;
            default:
              throw new Exception("Invalid Offset: " + intOffset.ToString());
          }
    
          // Append the octave
          strNoteName += intOctave.ToString();
           
          // Return
          return strNoteName;

        }


        //# METHOD: Converts note name to number
        private int pConvNameToNo(string strNoteName)
        {

          // Init
          string strLetter;
          string strSecondChar;
          int intOctave;
          int intNoteNo;
          int intOffset;
          bool blnIsSharp=false;

          // Tidy up 
          strNoteName = strNoteName.Trim().ToUpper();

          // Split up the note name into two parts 
          // E.g. C4 into  C and 0
          strSecondChar = strNoteName.Substring(1, 1);

          // See if a sharp is specified or not (e.g. "C#0")
          if (strSecondChar == "#")
          {
            blnIsSharp = true;
            strLetter = strNoteName.Substring(0, 2);
          } else {
            strLetter = strNoteName.Substring(0, 1);
          }

          // Get the octave
          if (blnIsSharp)
          {
            intOctave = System.Convert.ToInt32(strNoteName.Substring(2));
          }
          else
          {
            intOctave = System.Convert.ToInt32(strNoteName.Substring(1));
          }

          // Get the number offset based on Note Letter
          switch (strLetter)
          {
            case "A":
              intOffset = 9;
              break;
            case "A#":
              intOffset = 10;
              break;
            case "B":
              intOffset = 11;
              break;
            case "C":
              intOffset = 0;
              break;
            case "C#":
              intOffset = 1;
              break;
            case "D":
              intOffset = 2;
              break;
            case "D#":
              intOffset = 3;
              break;
            case "E":
              intOffset = 4;
              break;
            case "F":
              intOffset = 5;
              break;
            case "F#":
              intOffset = 6;
              break;
            case "G":
              intOffset = 7;
              break;
            case "G#":
              intOffset = 8;
              break;
            default:
              throw new Exception("Invalid Note Letter: " + strLetter);
          }

     
          // Work out the Note No (Midi Number)
          // Midi Note 0 = C-1
          // Midi Note 127 = G9
          intNoteNo = ((intOctave+1) * 12) + intOffset;

          // Return
          return intNoteNo;

        } // End Proc
       

    }


}
