using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Infrastructure;
using Common.Devices;
using System.ComponentModel;

namespace Common.Midi
{
    /// <summary>
    /// This is simple readonly service to relay status information from an instance of IDeviceStatusService
    /// </summary>
    class MidiStatusControlViewModel : ObservableObject, IDeviceStatusService, IDisposable
    {
        IDeviceStatusService _deviceStatusService;
        
        public MidiStatusControlViewModel(IDeviceStatusService deviceStatusService)
        {
            _deviceStatusService = deviceStatusService;
            //Forward property changed events from IDeviceStatusService.
            _deviceStatusService.PropertyChanged += ReRaisePropertyChanged;
        }

        public bool IsInitialised 
        {
            get
            {
                return _deviceStatusService.IsInitialised;
            }
        }

        public string Name
        {
            get
            {
                return _deviceStatusService.Name;
            }
        }

        private void ReRaisePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            RaisePropertyChanged(args.PropertyName);
        }

        public void Dispose()
        {
            _deviceStatusService.PropertyChanged -= ReRaisePropertyChanged;
        }
    }
}
