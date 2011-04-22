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

namespace KeyBoardControlLibrary
{
    /// <summary>
    /// Interaction logic for KeyBoardControl.xaml
    /// </summary>
    public partial class KeyBoardControl : UserControl
    {
        private int numberOfOctaves = 8;

        public KeyBoardControl(int numberOfOctaves)
        {
            this.numberOfOctaves = numberOfOctaves;
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
            AddKeys();
        }

        public void AddKeys()
        {

            for (int i = 1; i <= numberOfOctaves; i++) DrawOctave(i);
        }

        private void DrawOctave(int octaveNumber)
        {
            double OctaveOffset = (PianoKey.WhiteKeyWidth / 2) + ((octaveNumber - 1) * 7 * PianoKey.WhiteKeyWidth);
            var keys = new List<PianoKey> 
            { 
                new PianoKey(KeyTypes.White, (octaveNumber + 1) * 12 + 0, OctaveOffset + PianoKey.WhiteKeyWidth * 0.0),
                new PianoKey(KeyTypes.White, (octaveNumber + 1) * 12 + 2, OctaveOffset + PianoKey.WhiteKeyWidth * 1.0),
                new PianoKey(KeyTypes.White, (octaveNumber + 1) * 12 + 4, OctaveOffset + PianoKey.WhiteKeyWidth * 2.0),
                new PianoKey(KeyTypes.White, (octaveNumber + 1) * 12 + 5, OctaveOffset + PianoKey.WhiteKeyWidth * 3.0),
                new PianoKey(KeyTypes.White, (octaveNumber + 1) * 12 + 7, OctaveOffset + PianoKey.WhiteKeyWidth * 4.0),
                new PianoKey(KeyTypes.White, (octaveNumber + 1) * 12 + 9, OctaveOffset + PianoKey.WhiteKeyWidth * 5.0),
                new PianoKey(KeyTypes.White, (octaveNumber + 1) * 12 + 11, OctaveOffset + PianoKey.WhiteKeyWidth * 6.0),
                new PianoKey(KeyTypes.Black, (octaveNumber + 1) * 12 + 1, OctaveOffset + PianoKey.WhiteKeyWidth * 0.5),
                new PianoKey(KeyTypes.Black, (octaveNumber + 1) * 12 + 3, OctaveOffset + PianoKey.WhiteKeyWidth * 1.5),
                new PianoKey(KeyTypes.Black, (octaveNumber + 1) * 12 + 6, OctaveOffset + PianoKey.WhiteKeyWidth * 3.5),
                new PianoKey(KeyTypes.Black, (octaveNumber + 1) * 12 + 8, OctaveOffset + PianoKey.WhiteKeyWidth * 4.5),
                new PianoKey(KeyTypes.Black, (octaveNumber + 1) * 12 + 10, OctaveOffset + PianoKey.WhiteKeyWidth * 5.5)
            };

            foreach (var key in keys)
            {
                //MessageBox.Show(key.ToString());
                KeyBoardCanvas.Children.Add(key);
            }
        }
    }
}
