using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Media
{
    public interface IMediaServiceHost
    {
        IMediaService MediaService { get; set; }
    }
}
