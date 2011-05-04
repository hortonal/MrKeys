using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Devices
{
    public interface IDeviceStatusService
    {
        bool IsInitialised { get; }
        string Name { get; }
    }
}
