using System;
using Common.MediaControls;
using Microsoft.Practices.Unity;
using System.Windows.Controls;

namespace Common.Media
{
    /// <summary>
    /// Interaction logic for MediaControl.xaml
    /// </summary>
    public partial class MediaControl : UserControl
    {

        public MediaControl()
        {
            InitializeComponent();

            //To resolve this view model, the necessary services must have already been registered in the container
            var container = IOC.Container;
            this.DataContext = container.Resolve<MediaControlViewModel>();
        }
    }
}
