using System;
using Sanford.Multimedia.Midi;
using Common;
using Common.Devices;
using Common.Events;
using Common.Infrastructure;
using Common.Logging;

namespace Common.IO
{
    public class MidiOutput : ObservableObject, IOutput
    {
        OutputDevice _outputDevice;
        private ILogger _logger;

        public MidiOutput(ILogger logger)
        {
            _logger = logger;
        }

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
            if (args == null) return;
            if (IsInitialised)
            {
                try
                {
                    _logger.Log(this, LogLevel.Info, "Sending message");
                    _outputDevice.Send(SanfordUtils.ConvertKeyStrokeEventArgsToChannelMessage(args));

                }
                catch (Exception ex)
                {
                    //Catch ex...
                }
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
