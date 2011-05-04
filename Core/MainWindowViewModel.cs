using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Infrastructure;
using Common.Media;
using Common.Services;

namespace MrKeys
{
    class MainWindowViewModel: ObservableObject
    {
        IMediaService _mediaService;
        IDialogService _dialoagService;

        public MainWindowViewModel(IMediaService mediaService, IDialogService dialogService)
        {
            _dialoagService = dialogService;
            _mediaService = mediaService;
        }

        private string _textBoxTest = "My Name is Dave";
        public string TextBoxTest { get { return _textBoxTest; } 
            set 
            { 
                _textBoxTest = value; 
            } }

    }
}
