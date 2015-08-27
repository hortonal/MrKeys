using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Infrastructure;
using Common.Media;
using Common.Devices;
using Common.Services;
using Common.Outputs;
using KeyBoardControlLibrary;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Common.Inputs;
using System.Windows.Controls;

namespace MrKeys
{
    class MainWindowViewModel: ObservableObject
    {
        private readonly IUnityContainer _container;
        private readonly IMediaService _mediaService;
        private readonly IDialogService _dialoagService;
        private readonly IOutput _output;
        private readonly IMidiInput _input;
        private readonly IVirtualKeyBoard _keyBoard;
        private UserControl _currentTest;
        
        public MainWindowViewModel(IUnityContainer container,  IMediaService mediaService, 
            IDialogService dialogService, IOutput outputDevice, IMidiInput inputDevice,
            IVirtualKeyBoard keyBoard
            , ITestControlService currentTest)
        {
            _container =container;
            _dialoagService = dialogService;
            _mediaService = mediaService;
            _output = outputDevice;
            _input = inputDevice;
            _keyBoard = keyBoard;
            _currentTest = currentTest.Control;

            ResolveViews();

            //Hook up keyboard (mouse presses) to output
            _keyBoard.KeyPressEvent += (o, e) => _output.Send(e); 
        }

        public KeyBoardControl KeyBoardControlView { get; set; }
        public MediaControl MediaControlView { get; set; }
        public DeviceStatusControl InputStatusControlView { get; set; }
        public DeviceStatusControl OutputStatusControlView { get; set; }
        
        public UserControl CurrentTestView 
        {
            get { return _currentTest; }
            set
            {
                _currentTest = value;
                RaisePropertyChanged("CurrentTestView");
            }
        }
        
        private void ResolveViews()
        {
            KeyBoardControlView = _container.Resolve<KeyBoardControl>();
            MediaControlView = _container.Resolve<MediaControl>();

            InputStatusControlView = _container.Resolve<DeviceStatusControl>(new ParameterOverride("deviceStatusService", _input));
            OutputStatusControlView = _container.Resolve<DeviceStatusControl>(new ParameterOverride("deviceStatusService", _output));
        }
        
        public ICommand ChangeTest
        {
            get { return new RelayCommand(ChangeCurrentTest, () => true); }
        }

        private void ChangeCurrentTest()
        {

        }
    }
}
