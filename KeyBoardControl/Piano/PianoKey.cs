using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace KeyBoardControlLibrary
{
    public class PianoKey : Shape
    {
        
        #region ctor and member variable declarations

        public static int WhiteKeyWidth = 10;
        public static int KeyOffset = WhiteKeyWidth / 2;
        public static int BlackKeyWidth = 5;
        public static int WhiteKeyHeight = 30;
        public static int BlackKeyHeight = WhiteKeyHeight * 2 / 3;

        public KeyTypes KeyType { get; set; }
        public int KeyMidiId { get; private set; }
        private double centreXPosition;


        public PianoKey(KeyTypes keyType, int keyMidiId, double centrePosition)
        {
            KeyType = keyType;
            KeyMidiId = keyMidiId;
            centreXPosition = centrePosition;

            Stroke = Brushes.Black;
            SetDefaultKeyColour();
            
            
            MouseEnter += (o, e) =>
                {
                    Fill = Brushes.GreenYellow;
                };
            MouseLeave += (o, e) =>
                {
                    SetDefaultKeyColour();
                };
            MouseUp += (o, e) => MessageBox.Show("This key had Midi id: " + KeyMidiId);
            
        }
        #endregion

        #region implemented abstract methods from base class
        protected override Geometry DefiningGeometry
        {
            get
            {
                Rect rect;
                if (KeyType == KeyTypes.Black)
                {
                    rect = new Rect(centreXPosition - BlackKeyWidth / 2.0, 0, BlackKeyWidth, BlackKeyHeight);
                }
                else 
                {
                    rect = new Rect(centreXPosition - WhiteKeyWidth / 2.0, 0, WhiteKeyWidth, WhiteKeyHeight);
                }
                return new RectangleGeometry(rect);                
            }
        }
        #endregion

        #region Events
        
        #endregion Events

        private void SetDefaultKeyColour()
        {
            Fill = (KeyType == KeyTypes.White) ? Brushes.Ivory : Brushes.Black;
        }


        public override string ToString()
        {
            return string.Format("Fill: {0}, Stroke: {1}, ActualHeight{2}, ActualWidth{3}", Fill, Stroke, ActualHeight, ActualWidth);
        }
    }

    public enum KeyTypes { White, Black };
}