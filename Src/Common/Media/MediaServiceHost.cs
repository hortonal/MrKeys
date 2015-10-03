using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Media
{
    /// <summary>
    /// An object to host references to media services
    /// Allow various controls to take ownership of the buttons...
    /// </summary>
    public class MediaServiceHost : IMediaServiceHost
    {
        public IMediaService MediaService { get; set; }
    }
}
