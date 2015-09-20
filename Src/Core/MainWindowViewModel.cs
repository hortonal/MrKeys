
using Common.Media;
using Common.Devices;
using Common.Services;

using KeyBoardControlLibrary;
using Microsoft.Practices.Unity;
using Common.IO;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MrKeys
{
    public class NotifyPropertyBasic: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    class MainWindowViewModel: NotifyPropertyBasic
    {
        private readonly IUnityContainer _container;
        private readonly IMediaService _mediaService;
        private readonly IDialogService _dialoagService;
        private readonly IOutput _output;
        private readonly IMidiInput _input;
        private readonly IVirtualKeyBoard _keyBoard;
        private ITestControlService _currentTestControl;
        private ObservableCollection<ITestControlService> availableTestControls;

        

        public MainWindowViewModel(IUnityContainer container,  IMediaService mediaService, 
            IDialogService dialogService, IOutput outputDevice, IMidiInput inputDevice,
            IVirtualKeyBoard keyBoard, ITestControlService currentTest, IInputEvents inputEvents)
        {
            _container =container;
            _dialoagService = dialogService;
            _mediaService = mediaService;
            _output = outputDevice;
            _input = inputDevice;
            _keyBoard = keyBoard;
            _currentTestControl = currentTest;

            availableTestControls = new ObservableCollection<ITestControlService>();
            availableTestControls.Add(_currentTestControl);
            availableTestControls.Add(_container.Resolve<NoteGuess.NoteGuessControl>());
            
            ResolveViews();
            
            //Hook up keyboard (mouse presses) to output
            //_keyBoard.KeyPressEvent += (o, e) => _output.Send(e);

            //inputEvents.InputMessageRaised += (o, e) => _keyBoard.HandleIncomingMessage(o, e);

            //_input.MessageReceived += (o, e) => _keyBoard.HandleIncomingMessage(o, e);

        }



        public KeyBoardControl KeyBoardControlView { get; set; }
        public MediaControl MediaControlView { get; set; }
        public DeviceStatusControl InputStatusControlView { get; set; }
        public DeviceStatusControl OutputStatusControlView { get; set; }
        
        //Need to get this hooked up for updates etc.
        public UserControl CurrentTestView 
        {
            get { return _currentTestControl.Control; }
            private set
            {               
                RaisePropertyChanged("CurrentTestView");
            }
        }



        private void ResolveViews()
        {
            KeyBoardControlView = _container.Resolve<KeyBoardControl>();
            MediaControlView = _container.Resolve<MediaControl>();

            //Resolve two instances of the same class
            InputStatusControlView = _container.Resolve<DeviceStatusControl>(new ParameterOverride("deviceStatusService", _input));
            OutputStatusControlView = _container.Resolve<DeviceStatusControl>(new ParameterOverride("deviceStatusService", _output));
        }

        //Some test stuff
        public ObservableCollection<ITestControlService> AvailableTestControls
        {
            get { return availableTestControls; }
            set {
                availableTestControls = value;
            }
        }
        
        public ITestControlService SelectedTest
        {
            get { return _currentTestControl; }
            set {
                _currentTestControl = value;
                CurrentTestView = _currentTestControl.Control;
            }
        }
        
    }
}
