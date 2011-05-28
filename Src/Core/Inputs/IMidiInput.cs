using System;
using Common.Devices;
namespace Common.Inputs
{
    interface IMidiInput : IInputDeviceStatusService
    {
        void Initialise();
        event Common.Events.PianoKeyStrokeEvent MessageReceived;
        void StartRecording();
        void StopRecording();
        void Close();
    }
}
