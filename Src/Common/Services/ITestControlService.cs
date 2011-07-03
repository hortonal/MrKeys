using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Common.Services
{
    public interface ITestControlService
    {
        UserControl Control {get;}
        void StartTest();
        void MarkTest();
    }
}
