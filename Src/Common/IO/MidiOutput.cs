using System;
using Sanford.Multimedia.Midi;
using Common;
using Common.Devices;
using Common.Events;
using Common.Infrastructure;

namespace Common.IO
{
    public class MidiOutput : ObservableObject, IOutput, IDisposable
    {
        OutputDevice _outputDevice;

        public void Close()
        {
            if(IsInitialised) _outputDevice.Close();
        }

        public void Initialise()
        {
            try
            {
                _outputDevice = new OutputDevice(0);
                IsInitialised = true;
            }
            catch (Exception)
            {
                //Exceptions.ErrHandler("Failed to initialise output device: ", e);
            }
        }

        public void Send(object sender, PianoKeyStrokeEventArgs args)
        {
            if(IsInitialised)
            {
                _outputDevice.Send(SanfordUtils.ConvertKeyStrokeEventArgsToChannelMessage(args));
            }
        }

        #region IOutput implementation
        private string _deviceName;

        public bool IsInitialised
        {
            get;
            private set;
        }

        public string Name
        {
            get { return "Output (make dyn pls)"; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged("Name");
            }
        }

        public DeviceType DeviceType
        {
            get { return DeviceType.Output; }
        }
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            Close();
        }
        #endregion
    }
}
