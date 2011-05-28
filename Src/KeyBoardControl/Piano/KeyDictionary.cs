using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyBoardControlLibrary
{
    class KeyDictionary
    {
        private int m_numberOfOctaves = 7;
        public Dictionary<int, Key> Keys { get; set; }
        
        public KeyDictionary()
        {
            Keys = new Dictionary<int, Key>();
            BuildKeyDictionary();
        }

        private void BuildKeyDictionary()
        {
            for (int i = 1; i <= m_numberOfOctaves; i++) BuildOctave(i);
        }

        private void BuildOctave(int octaveNumber)
        {
            double OctaveOffset = (Key.WhiteKeyWidth / 2) + ((octaveNumber - 1) * 7 * Key.WhiteKeyWidth);

            AddKeyToDictionary((octaveNumber + 1) * 12 + 0, KeyTypes.White, OctaveOffset + Key.WhiteKeyWidth * 0.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 2, KeyTypes.White, OctaveOffset + Key.WhiteKeyWidth * 1.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 4, KeyTypes.White, OctaveOffset + Key.WhiteKeyWidth * 2.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 5, KeyTypes.White, OctaveOffset + Key.WhiteKeyWidth * 3.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 7, KeyTypes.White, OctaveOffset + Key.WhiteKeyWidth * 4.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 9, KeyTypes.White, OctaveOffset + Key.WhiteKeyWidth * 5.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 11, KeyTypes.White, OctaveOffset + Key.WhiteKeyWidth * 6.0);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 1, KeyTypes.Black, OctaveOffset + Key.WhiteKeyWidth * 0.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 3, KeyTypes.Black, OctaveOffset + Key.WhiteKeyWidth * 1.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 6, KeyTypes.Black, OctaveOffset + Key.WhiteKeyWidth * 3.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 8, KeyTypes.Black, OctaveOffset + Key.WhiteKeyWidth * 4.5);
            AddKeyToDictionary((octaveNumber + 1) * 12 + 10, KeyTypes.Black, OctaveOffset + Key.WhiteKeyWidth * 5.5);
        }

        private void AddKeyToDictionary(int keyId, KeyTypes keyType, double keyWidth)
        {
            Key key = new Key(keyType, keyId, keyWidth);
            Keys.Add(keyId, key);
        }
    }
}
