using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Infrastructure;
using System.Windows.Input;
using Common.Services;

namespace Common.Media
{
    public class MediaControlViewModel 
    {
        private IMediaServiceHost _mediaServiceHost;
        private IDialogService _dialogService;

        //these params are typically dependency injected
        public MediaControlViewModel(IMediaServiceHost mediaServiceHost, IDialogService diaglogService)
        {       
            _mediaServiceHost = mediaServiceHost;
            _dialogService = diaglogService;
        }

        #region Command Actions
        void PlayExecute()
        {
            try
            {
                _mediaServiceHost.MediaService.Play();
            } catch (Exception ex)
            {
                _dialogService.ShowException("Couldn't play: " + ex.Message);
            }
        }

        void StopExecute()
        {
            try
            {
                _mediaServiceHost.MediaService.Stop();
            }
            catch (Exception ex)
            {
                _dialogService.ShowException("Couldn't stop: " + ex.Message);
            }
        }

        void PauseExecute()
        {
            try
            {
                _mediaServiceHost.MediaService.Pause();
            }
            catch (Exception ex)
            {
                _dialogService.ShowException("Couldn't pause: " + ex.Message);
            }
        }

        void RecordExecute()
        {
            try
            {
                _mediaServiceHost.MediaService.Record();
            }
            catch (Exception ex)
            {
                _dialogService.ShowException("Couldn't record: " + ex.Message);
            }
        }
        #endregion

        #region Commands

        public ICommand PlayCommand
        {
            get { return new RelayCommand(PlayExecute, () => _mediaServiceHost.MediaService.CanPlay); }
        }

        public ICommand StopCommand
        {
            get { return new RelayCommand(StopExecute, () => _mediaServiceHost.MediaService.CanStop); }
        }

        public ICommand PauseCommand
        {
            get { return new RelayCommand(PauseExecute, () => _mediaServiceHost.MediaService.CanPause); }
        }

        public ICommand RecordCommand
        {
            get { return new RelayCommand(RecordExecute, () => _mediaServiceHost.MediaService.CanRecord); }
        }

        #endregion //Commands
    }
}
