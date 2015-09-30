using Common.Devices;
using System;

namespace Common.IO
{
    public interface IOutput: IOutputDeviceStatusService, IDisposable
    {
        void Initialise();
        void Send(object sender, Events.PianoKeyStrokeEventArgs args);
        void Close();
    }
}
