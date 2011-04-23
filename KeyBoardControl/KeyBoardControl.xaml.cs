using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common.Events;

namespace KeyBoardControlLibrary
{
    /// <summary>
    /// Interaction logic for KeyBoardControl.xaml
    /// </summary>
    public partial class KeyBoardControl : UserControl
    { 
        private int m_numberOfOctaves = 8;
        private Dictionary<int, PianoKey> m_KeyDicionary;

        public KeyBoardControl(int numberOfOctaves)
        {
            this.m_numberOfOctaves = numberOfOctaves;
            Initialise();
        }
        
        public KeyBoardControl()
        {
            Initialise();
        }

        private void Initialise()
        {
            InitializeComponent();
            Visibility = System.Windows.Visibility.Visible;

            m_KeyDicionary = new Dictionary<int,PianoKey>();
            AddKeys();
        }

        public void AddKeys()
        {

            for (int i = 1; i <= m_numberOfOctaves; i++) BuildOctave(i);


            foreach (var key in m_KeyDicionary.Values)
            {
                //MessageBox.Show(key.ToString());
                KeyBoardCanvas.Children.Add(key);
            }

        }

        private void BuildOctave(int octaveNumber)
        {
            double OctaveOffset = (PianoKey.WhiteKeyWidth / 2) + ((octaveNumber - 1) * 7 * PianoKey.WhiteKeyWidth);

            AddKeyToDictionary((octaveNumber + 1) * 12 + 0, KeyTypes.White, OctaveOffset + PianoKey.WhiteKeyWidth * 0.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 2, KeyTypes.White, OctaveOffset + PianoKey.WhiteKeyWidth * 1.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 4, KeyTypes.White, OctaveOffset + PianoKey.WhiteKeyWidth * 2.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 5, KeyTypes.White, OctaveOffset + PianoKey.WhiteKeyWidth * 3.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 7, KeyTypes.White, OctaveOffset + PianoKey.WhiteKeyWidth * 4.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 9, KeyTypes.White, OctaveOffset + PianoKey.WhiteKeyWidth * 5.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 11, KeyTypes.White, OctaveOffset + PianoKey.WhiteKeyWidth * 6.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 1, KeyTypes.Black, OctaveOffset + PianoKey.WhiteKeyWidth * 0.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 3, KeyTypes.Black, OctaveOffset + PianoKey.WhiteKeyWidth * 1.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 6, KeyTypes.Black, OctaveOffset + PianoKey.WhiteKeyWidth * 3.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 8, KeyTypes.Black, OctaveOffset + PianoKey.WhiteKeyWidth * 4.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 10, KeyTypes.Black, OctaveOffset + PianoKey.WhiteKeyWidth * 5.5);
        }

        private void AddKeyToDictionary(int keyId, KeyTypes keyType, double keyWidth)
        {
            m_KeyDicionary.Add(keyId, new PianoKey(keyType, keyId, keyWidth));
        }

        #region Keyboard event handling
        public delegate void PianoKeyStrokeEvent(object sender,PianoKeyStrokeEventArgs e);

        public void HandleMessage(object sender,PianoKeyStrokeEventArgs e)
        {
            if (e == null) return;
            if (m_KeyDicionary.ContainsKey(e.midiKeyId))
            {
                if (e.keyStrokeType == KeyStrokeType.KeyPress)
                {
                    m_KeyDicionary[e.midiKeyId].SetKeyPressedColour(e.KeyVelocity);
                }
                if (e.keyStrokeType == KeyStrokeType.KeyRelease) m_KeyDicionary[e.midiKeyId].SetDefaultKeyColour();
            }

        }
        #endregion
    }
}
