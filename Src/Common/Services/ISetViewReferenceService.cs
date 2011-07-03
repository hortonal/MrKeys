using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Common.Services
{
    interface ISetViewReferenceService
    {
        void SetViewCanvas(Panel parentControl);
    }
}
