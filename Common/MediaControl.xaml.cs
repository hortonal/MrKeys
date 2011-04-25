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

namespace Common
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

            uiCtx = SynchronizationContext.Current;

            var marginThickness = new Thickness(4.0);

            MediaControlContainer.Children.Add(new PlayButton() { Width = 30, Height = 30, Margin = marginThickness });
            MediaControlContainer.Children.Add(new StopButton() { Width = 30, Height = 30, Margin = marginThickness });
            MediaControlContainer.Children.Add(new RecordButton() { Width = 30, Height = 30, Margin = marginThickness });
            MediaControlContainer.Children.Add(new PauseButton() { Width = 30, Height = 30, Margin = marginThickness });
        }
    }
}
