using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Infrastructure;
using System.ComponentModel;

namespace Common.Devices
{
    public interface IDeviceStatusService : INotifyPropertyChanged
    {
        bool IsInitialised { get; }
        string Name { get; }
        DeviceType DeviceType { get; }
    }
}
