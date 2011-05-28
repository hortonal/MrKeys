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

        public MediaControl(MediaControlViewModel viewModel): this()
        {
            this.DataContext = viewModel;
        }

        public MediaControl()
        {
            InitializeComponent();
        }
    }
}
