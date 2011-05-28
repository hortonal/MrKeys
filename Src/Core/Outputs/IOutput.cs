using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Devices;

namespace Common.Outputs
{
    public interface IOutput: IOutputDeviceStatusService
    {
        void Initialise();
        void Send(Common.Events.PianoKeyStrokeEventArgs args);
        void Close();
    }
}
