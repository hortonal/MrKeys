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
using System.Windows.Shapes;
using Sanford.Multimedia.Midi;
using KeyBoardControlLibrary;

namespace MrKeys.Testing
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        // Module vars
        private RecordSession m_Session;
        private bool m_Closing = false;
        private OutputDevice m_OutDevice;


        public Window2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            // Init Proc

            m_Session = new RecordSession();
            m_Session.Init();
            m_Session.MessageReceived += UpdateMessageReceived;
            m_OutDevice = new OutputDevice(0);


            // Subscribe to events
            m_Session.Sequencer.Position = 0;
            m_Session.Sequencer.PlayingCompleted += new System.EventHandler(this.HandlePlayingCompleted);
            m_Session.Sequencer.ChannelMessagePlayed += new System.EventHandler<Sanford.Multimedia.Midi.ChannelMessageEventArgs>(this.HandleChannelMessagePlayed);
            m_Session.Sequencer.Stopped += new System.EventHandler<Sanford.Multimedia.Midi.StoppedEventArgs>(this.HandleStopped);
            m_Session.Sequencer.SysExMessagePlayed += new System.EventHandler<Sanford.Multimedia.Midi.SysExMessageEventArgs>(this.HandleSysExMessagePlayed);
            m_Session.Sequencer.Chased += new System.EventHandler<Sanford.Multimedia.Midi.ChasedEventArgs>(this.HandleChased);



            MessageBox.Show("Initialised");

        }

        private void UpdateMessageReceived(object sender, EventArgs e)
        {

            lblMsgsReceived.Content = m_Session.NumMsgsReceived;

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            m_Session.StartRecording();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            m_Session.StopRecording();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            m_Session.Playback();
        }


        #region Sequencer Handling Events

        //private void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //  toolStripProgressBar1.Value = e.ProgressPercentage;
        //}

        //private void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
        //{
        //  this.Cursor = Cursors.Arrow;
        //  startButton.Enabled = true;
        //  continueButton.Enabled = true;
        //  stopButton.Enabled = true;
        //  openToolStripMenuItem.Enabled = true;
        //  toolStripProgressBar1.Value = 0;

        //  if (e.Error == null)
        //  {
        //    positionHScrollBar.Value = 0;
        //    positionHScrollBar.Maximum = sequence1.GetLength();
        //  }
        //  else
        //  {
        //    MessageBox.Show(e.Error.Message);
        //  }
        //}

        private void HandleChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            if (m_Closing)
            {
                return;
            }

            m_OutDevice.Send(e.Message);
            //pianoControl1.Send(e.Message);
        }

        private void HandleChased(object sender, ChasedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
            {
                m_OutDevice.Send(message);
            }
        }

        private void HandleSysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
            //     outDevice.Send(e.Message); Sometimes causes an exception to be thrown because the output device is overloaded.
        }

        private void HandleStopped(object sender, StoppedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
            {
                m_OutDevice.Send(message);
                //pianoControl1.Send(message);
            }
        }

        private void HandlePlayingCompleted(object sender, EventArgs e)
        {
            //timer1.Stop();
        }


        #endregion

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            string fileName = "C:\\Windows\\Media\\Flourish.mid";
            Sequence seq = new Sequence(fileName);
            NoteGroup grp = new NoteGroup();
            Track trk;
            trk = seq[0]; // Get the first track
            grp.ImportTrack(trk);

        }

        private void ShowKeyboard_Click(object sender, RoutedEventArgs e)
        {
            var viewbox = new Viewbox();
            var keyBoard = new KeyBoardControl(8);
            viewbox.Child = keyBoard;
            viewbox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            ParentWindow.Children.Add(viewbox);
        }

    }
}
