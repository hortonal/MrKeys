using System;
using Common.MediaControls;
using Microsoft.Practices.Unity;
using System.Windows.Controls;
using System.Linq;
using Common.Devices;
using System.Reflection;

namespace Common.Midi
{
    /// <summary>
    /// Interaction logic for MidiStatusControl.xaml
    /// </summary>
    public partial class DeviceStatusControl : UserControl
    {

        public DeviceStatusControl(IUnityContainer container, IDeviceStatusService deviceStatusService): this()
        {
            //To resolve this view model, the necessary services must have already been registered in the container
            
            this.DataContext = container.Resolve<MidiStatusControlViewModel>(new ParameterOverride("deviceStatusService", deviceStatusService));            
        }

        public DeviceStatusControl()
        { 
            InitializeComponent();
        }
    }
}
