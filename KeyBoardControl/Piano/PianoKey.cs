using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using Common.Events;

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
        private bool IsKeyPressed = false;

        public event PianoKeyStrokeEvent KeyPressEvent;

        public PianoKey(KeyTypes keyType, int keyMidiId, double centrePosition)
        {
            KeyType = keyType;
            KeyMidiId = keyMidiId;
            centreXPosition = centrePosition;
            
            Stroke = Brushes.Black;
            SetDefaultKeyColour();
            
            MouseEnter += (o, e) =>
                {
                    SetKeyPressedColour();
                };
            MouseLeave += (o, e) =>
                {
                    SetDefaultKeyColour();
                };

            
            MouseDown += HandleKeyDown; 
            MouseUp += HandleKeyUp;
            //When mouse leaves key region, throw a key upevent just incase.
            MouseLeave += HandleKeyUp;
        }
        #endregion

        private void HandleKeyDown(object sender, EventArgs e)
        {
            IsKeyPressed = true;
            //Raise PianoKeyStrokeEvent describing the user input event.
            if (KeyPressEvent != null)
            {
                KeyPressEvent(this, new PianoKeyStrokeEventArgs
                {
                    keyStrokeType = KeyStrokeType.KeyPress,
                    KeyVelocity = 100,
                    midiKeyId = KeyMidiId,
                });
            }
        }

        private void HandleKeyUp(object sender, EventArgs e)
        {
            IsKeyPressed = false;
            //Raise PianoKeyStrokeEvent describing the user input event.
            if (KeyPressEvent != null)
            {
                KeyPressEvent(this, new PianoKeyStrokeEventArgs
                {
                    keyStrokeType = KeyStrokeType.KeyPress,
                    KeyVelocity = 0,
                    midiKeyId = KeyMidiId,
                });
            }
        }

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


        #region Key Color manipulation methods
        public void SetDefaultKeyColour()
        {
            Fill = (KeyType == KeyTypes.White) ? Brushes.Ivory : Brushes.Black;
        }

        public void SetKeyPressedColour()
        {
            Fill = Brushes.GreenYellow;
        }

        public void SetKeyPressedColour(int velocity)
        {
            Color startColor = Colors.GreenYellow;
            Color endColor = Colors.Red;
            endColor.B = Convert.ToByte(200 * (1 - velocity / 127.0));
            endColor.G = Convert.ToByte(200 * (1 - velocity / 127.0));

            var b = new LinearGradientBrush(startColor, endColor, 90);
            
            Fill = b;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("Fill: {0}, Stroke: {1}, ActualHeight{2}, ActualWidth{3}", Fill, Stroke, ActualHeight, ActualWidth);
        }
    }

    public enum KeyTypes { White, Black };
    
}