using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Media;
using Common.Services;
using System.Windows;

namespace Common
{
    public class MediaService : IMediaService
    {

        public MediaService()
        {
            CanPause = true;
            CanPlay = true;
            CanStop = true;
            CanRecord = true;
        }

        public void Play()
        {
            MessageBox.Show("I'm playing");
        }

        public void Stop()
        {
            MessageBox.Show("I'm stopped");
        }

        public void Pause()
        {
            MessageBox.Show("I'm paused");
        }

        public void Record()
        {
            MessageBox.Show("I'm recording");
        }

        public bool CanPlay { get; set; }

        public bool CanStop { get; set; }

        public bool CanPause { get; set; }

        public bool CanRecord { get; set; }
    }
}
