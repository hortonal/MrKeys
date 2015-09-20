using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Infrastructure;
using Common.IO;
using Common.Devices;
using Sanford.Multimedia.Midi;
using Common.Events;
using System.Threading;

namespace Common.IO
{
    public class MidiInput : ObservableObject, IMidiInput, IDisposable
    {
        SynchronizationContext _uiCtx;
        InputDevice _inputDevice;
        //Avoid having to handle non-subsribed events
        public event PianoKeyStrokeEvent MessageReceived = (o, e) => { };
        private IInputEvents _inputEvents;

        public MidiInput() { }
        
        public void Close()
        {
            if(IsInitialised) _inputDevice.Close();
        }

        public void StartRecording()
        {
            _inputDevice.StartRecording();
        }

        public void StopRecording()
        {
            _inputDevice.StopRecording();
        }

        #region Init
        public void Initialise()
        {
            _uiCtx = SynchronizationContext.Current;
            
            // Set up the midi device
            if (InputDevice.DeviceCount == 0)
            {
                //throw new Exception("No midi devices detected");
            }
            else
            {
                try
                {
                    // Set up device and link to handler
                    _inputDevice = new InputDevice(0);
                    _inputDevice.ChannelMessageReceived += HandleInputMessageReceived;
                    IsInitialised = true;
                    //Remove me when you have the recorder setup
                    this.StartRecording();
                }
                catch (Exception ex)
                {
                    Exceptions.ErrHandler(ex);
                }
            }
        }
        #endregion Init

        private void HandleInputMessageReceived(object sender, ChannelMessageEventArgs args)
        {
            _uiCtx.Post(o =>
            {
                // Create new channel message object and add to session
                ChannelMessage msg = args.Message;

                if (MessageReceived != null) MessageReceived(this, SanfordUtils.ConvertChannelMessageToKeyStrokeEventArgs(msg));

            }, null);
        }


        #region DeviceStatusService implementation
        private bool _isInitialised;
        private string _deviceName;

        public bool IsInitialised
        {
            get
            {
                return _isInitialised;
            }
            private set
            {
                _isInitialised = value;
                RaisePropertyChanged("IsInitialised");
            }
        }

        public string Name
        {
            get
            {
                return "(make me dynamic pls)";
            }
            set
            {
                _deviceName = value;
                RaisePropertyChanged("Name");
            }
        }

        public DeviceType DeviceType
        {
            get { return DeviceType.Input; }
        }
        #endregion

        public void Dispose()
        {
            _inputDevice.Close();
            _inputDevice.Dispose();
        }
    }
}
