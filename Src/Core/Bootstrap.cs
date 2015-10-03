using System.Windows;
using Common.Devices;
using Common.Media;
using Common.IO;
using Common.Services;
using KeyBoardControlLibrary;
using Microsoft.Practices.Unity;
using System;
using ScoreControlLibrary;
using SongPlayer;

namespace MrKeys
{
    public partial class App : Application
    {
        private void Bootstrap()
        {
            InputEvents inputEvents = _container.Resolve<InputEvents>();

            var midiInput = _container.Resolve<MidiInput>();
            var midiOutput = _container.Resolve<MidiOutput>();
            //var mediaServiceHost = _container.Resolve<MediaServiceHost>();

            //Try and initialise the midi input/output
            try { midiInput.Initialise(); }
            catch (Exception ex) { MessageBox.Show("Failed to initialise Input: " + ex.Message); }

            try { midiOutput.Initialise(); }
            catch (Exception ex) { MessageBox.Show("Failed to initialise Output: " + ex.Message); }

            //Wire midi inputs through central the input events, could do this with OIC but the
            //midi input shouldn't need to know about this centralised routing
            midiInput.MessageReceived += inputEvents.HandleInputEvent;
            inputEvents.MessageReceived += (o, e) => midiOutput.Send(o, e);

            _container.RegisterType<IVirtualKeyBoard, VirtualKeyBoard>(new ContainerControlledLifetimeManager());

            _container.RegisterInstance<IInputEvents>(inputEvents);
            _container.RegisterInstance<IMidiInput>(midiInput);
            _container.RegisterInstance<IOutput>(midiOutput);

            //Register some basic services to be used by child views later...
            RecordSession recordSession = _container.Resolve<RecordSession>();

            _container.RegisterType<IDialogService, ModalDialogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<MediaControlViewModel, MediaControlViewModel>();
            _container.RegisterType<IMediaServiceHost, MediaServiceHost>(new ContainerControlledLifetimeManager());
            _container.RegisterInstance<IInputDeviceStatusService>(midiInput);
            _container.RegisterInstance<IOutputDeviceStatusService>(midiOutput);
            _container.RegisterType<ITestControlService, BasicTestControl>(new ContainerControlledLifetimeManager());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            //_container.Resolve<InputEvents>().Dispose();
            _container.Resolve<IMidiInput>().MessageReceived -= _container.Resolve<InputEvents>().HandleInputEvent;
            _container.Resolve<InputEvents>().MessageReceived -= _container.Resolve<IOutput>().Send;


            //Have to close down the IO devices otherwise we leave threads open...
            try { _container.Resolve<IMidiInput>().Dispose(); }
            catch { }

            try { _container.Resolve<IVirtualKeyBoard>().Dispose(); }
            catch { }

            try { _container.Resolve<IOutput>().Dispose(); }
            catch { }

            try { _container.Resolve<IOutput>().Dispose(); }
            catch { }
        }
    }
}
