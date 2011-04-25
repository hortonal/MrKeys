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
        public PlayButton PlayButton { get; set; }
        public StopButton StopButton { get; set; }
        public RecordButton RecordButton { get; set; }
        public PauseButton PauseButton { get; set; }
        
        private SynchronizationContext uiCtx;

        public MediaControl()
        {
            InitializeComponent();
            
            var marginThickness = new Thickness(4.0);
            PlayButton = new PlayButton() { Width = 30, Height = 30, Margin = marginThickness };
            StopButton = new StopButton() { Width = 30, Height = 30, Margin = marginThickness };
            PauseButton = new PauseButton() { Width = 30, Height = 30, Margin = marginThickness };
            RecordButton = new RecordButton() { Width = 30, Height = 30, Margin = marginThickness };
            
            uiCtx = SynchronizationContext.Current;

            MediaControlContainer.Children.Add(PlayButton);
            MediaControlContainer.Children.Add(StopButton);
            MediaControlContainer.Children.Add(RecordButton);
            MediaControlContainer.Children.Add(PauseButton);
        }
    }
}
