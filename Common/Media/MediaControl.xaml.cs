using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common.MediaControls;
using System.Threading;
using Microsoft.Practices.Unity;

namespace Common.Media
{
    /// <summary>
    /// Interaction logic for MediaControl.xaml
    /// </summary>
    public partial class MediaControl : UserControl
    {
        private SynchronizationContext uiCtx;

        public MediaControl()
        {
            InitializeComponent();

            var container = IOC.Container;
            this.DataContext = container.Resolve<MediaControlViewModel>();

            uiCtx = SynchronizationContext.Current;
        }
    }
}
