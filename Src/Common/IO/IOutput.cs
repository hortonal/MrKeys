using Common.Devices;

namespace Common.IO
{
    public interface IOutput: IOutputDeviceStatusService
    {
        void Initialise();
        void Send(object sender, Events.PianoKeyStrokeEventArgs args);
        void Close();
    }
}
