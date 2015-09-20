using System;
using Common.Events;

namespace Common.IO
{
    public interface IInputEvents
    {
        event PianoKeyStrokeEvent MessageReceived;
        void HandleInputEvent(object o, PianoKeyStrokeEventArgs e);
    }

    public class InputEvents : IInputEvents
    {
        public event PianoKeyStrokeEvent MessageReceived;

        /// <summary>
        /// Simply reraises the events it's subscribed to
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void HandleInputEvent(object o, PianoKeyStrokeEventArgs e)
        { 
            if(MessageReceived != null) MessageReceived(o, e);
        }
    }
}
