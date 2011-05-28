using System.Windows;
using Common;
using Common.Devices;
using Common.Inputs;
using Common.Media;
using Common.Outputs;
using Common.Services;
using KeyBoardControlLibrary;
using Microsoft.Practices.Unity;

namespace MrKeys
{
    public partial class App : Application
    {

        IUnityContainer _container;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            //Not sure why this is needed... The MS practices example does it this way...  
            base.OnStartup(e);
                
            _container = new UnityContainer();
            
            InitialiseServices();

            _container.RegisterType<MediaControlViewModel>();

            var window = _container.Resolve<MainWindow>();
            window.DataContext = _container.Resolve<MainWindowViewModel>();

            window.Show();
        }

        private void InitialiseServices()
        {

            //Build a virtual keyboard to handle events and such..
            _container.RegisterType<IVirtualKeyBoard, VirtualKeyBoard>(new ContainerControlledLifetimeManager());

            MidiInput midiInput = _container.Resolve<MidiInput>();
            MidiOutput midiOutput = _container.Resolve<MidiOutput>();

            //Try and initialise the midi input
            midiInput.Initialise();
            midiOutput.Initialise();

            _container.RegisterInstance<IMidiInput>(midiInput);
            _container.RegisterInstance<IOutput>(midiOutput);

            //Register some basic services to be used by child views later...
            RecordSession recordSession = _container.Resolve<RecordSession>();

            _container.RegisterType<IDialogService, ModalDialogService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<MediaControlViewModel, MediaControlViewModel>();

            _container.RegisterInstance<IMediaService>(recordSession);
            _container.RegisterInstance<IInputDeviceStatusService>(midiInput);
            _container.RegisterInstance<IOutputDeviceStatusService>(midiOutput);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            //Have to close down the IO devices otherwise we leave threads open...
            _container.Resolve<IMidiInput>().Close();
            _container.Resolve<IOutput>().Close();
        }
    }
}
