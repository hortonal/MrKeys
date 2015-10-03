using System;
using Common.Events;
using System.Windows.Controls;
using Common.IO;
using System.Windows;

namespace KeyBoardControlLibrary
{
    public interface IVirtualKeyBoard: IMidiInput, IDisposable
    {
        void HandleIncomingMessage(object sender, Common.Events.PianoKeyStrokeEventArgs e);
        event PianoKeyStrokeEvent KeyPressEvent;
        void DrawKeys(Panel parentControl); //feels like this shouldn't be on the interface...
        KeyRange GetkeyRange();
    }
}
