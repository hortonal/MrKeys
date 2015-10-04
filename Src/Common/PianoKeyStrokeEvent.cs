using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    namespace Events
    {
        public delegate void PianoKeyStrokeEvent (object sender, PianoKeyStrokeEventArgs e);

        public class PianoKeyStrokeEventArgs : EventArgs
        {
            public int midiKeyId { get; set; }
            public KeyStrokeType KeyStrokeType { get; set; }
            public int KeyVelocity { get; set; }

            public PianoKeyStrokeEventArgs()
            {
            }

            public PianoKeyStrokeEventArgs(int midiKeyId, KeyStrokeType keyStrokeType, int keyVelocity)
            {
                this.midiKeyId = midiKeyId;
                this.KeyStrokeType = keyStrokeType;
                this.KeyVelocity = keyVelocity;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append("KeyStrokeType: ").Append(KeyStrokeType).Append(",key id:").Append(midiKeyId);
                return sb.ToString();
            }
        }

        public enum KeyStrokeType { KeyPress, KeyRelease };
    }
}
