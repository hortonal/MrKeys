using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Infrastructure;
using System.Windows.Input;
using Common.Services;

namespace Common.Media
{
    public class MediaControlViewModel : ObservableObject
    {
        private IMediaService _mediaService;
        private IDialogService _dialogService;

        //these params are typically dependency injected
        public MediaControlViewModel(IMediaService mediaService, IDialogService diaglogService)
        {       
            _mediaService = mediaService;
            _dialogService = diaglogService;
        }


        #region Command Actions
        void PlayExecute()
        {
            try
            {
                _mediaService.Play();
            } catch (Exception ex)
            {
                _dialogService.ShowException("Couldn't play: " + ex.Message);
            }
        }

        void StopExecute()
        {
            try
            {
                _mediaService.Stop();
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
                _mediaService.Pause();
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
                _mediaService.Record();
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
            get { return new RelayCommand(PlayExecute, () => _mediaService.CanPlay); }
        }

        public ICommand StopCommand
        {
            get { return new RelayCommand(StopExecute, () => _mediaService.CanStop); }
        }

        public ICommand PauseCommand
        {
            get { return new RelayCommand(PauseExecute, () => _mediaService.CanPause); }
        }

        public ICommand RecordCommand
        {
            get { return new RelayCommand(RecordExecute, () => _mediaService.CanRecord); }
        }

        #endregion //Commands
    }
}
