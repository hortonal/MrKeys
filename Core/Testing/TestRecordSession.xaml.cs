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
using System.Threading;
using Sanford.Multimedia.Midi;
using KeyBoardControlLibrary;
using Common;
using PSAMWPFControlLibrary;

namespace MrKeys.Testing
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class MainTest : Window
    {   
        // Module vars
        private RecordSession m_Session;
        private bool m_Closing = false;
        private OutputDevice m_OutDevice;

        private SynchronizationContext m_context;

        public MainTest()
        {
            InitializeComponent();
            m_context = SynchronizationContext.Current;
            InitialiseRecordSession();
            InitialiseStaveView();
        }

        #region Init
        private void InitialiseStaveView()
        {
            StaveControl.LoadFromXmlFile(@"../../../ThirdParty/PSAMControlLibrary/example.xml");
        }

        private void InitialiseRecordSession()
        {
            try
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

                //Subscribe Keyboard to message recieved event
                m_Session.MessageReceived += Keyboard.HandleMessage;

                //Subscribe session to Keyboard pressed event
                Keyboard.KeyPressEvent += (o, ea) => m_OutDevice.Send(SanfordUtils.ConvertKeyStrokeEventArgsToChannelMessage(ea));

                InitialisedRadioButton.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialisation Failed.  Err Msg: " + ex.Message);
            }
        }
        #endregion Init

        #region Button Clicks
        private void InitialiseButton_Click(object sender, RoutedEventArgs e)
        {

            InitialiseRecordSession();
        }

        private void TestMidiEventsButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = "C:\\Windows\\Media\\Flourish.mid";
            Sequence seq = new Sequence(fileName);
            NoteGroup grp = new NoteGroup();
            Track trk;
            trk = seq[0]; // Get the first track
            grp.ImportTrack(trk);

        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            m_Session.Playback();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_Session.IsRecording)
            {
                m_Session.StopRecording();
            }
            {
                m_Session.Sequencer.Stop();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_Session.IsPaused)
            {
                m_Session.Sequencer.Continue();
                m_Session.IsPaused = false;
            }
            else
            {
                m_Session.Sequencer.Stop();
                m_Session.IsPaused = true;
            }
            
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            m_Session.Sequencer.Stop();
            m_Session.StartRecording();
        }
        #endregion Button Clicks



        private void UpdateMessageReceived(object sender, EventArgs e)
        {

            lblMsgsReceived.Content = m_Session.NumMsgsReceived;

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
            //Post on the main GUI thread...
            m_context.Post( 
                    _ => Keyboard.HandleMessage(sender, SanfordUtils.ConvertChannelMessageToKeyStrokeEventArgs(e.Message))
                    ,null);  
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

        private void LoadFile(string fileName)
        {
            Sequence seq = new Sequence(fileName);
            m_Session.Sequencer.Sequence = seq;
        }

    }
}
