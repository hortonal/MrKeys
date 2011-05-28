using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Media
{
    public interface IMediaService
    {
        bool CanPlay { get; set; }
        bool CanStop { get; set; }
        bool CanPause { get; set; }
        bool CanRecord { get; set; }

        void Play();
        void Stop();
        void Pause();
        void Record();
    }
}
