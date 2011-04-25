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

namespace MrKeys.Testing
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {   
        // Module vars
        private bool m_InputInitialised = false;
        private RecordSession m_Session;
        private bool m_Closing = false;
        private OutputDevice m_OutDevice;
        private string KeyboardViewName = "KeyBoardViewBox";
        private KeyBoardControl m_keyBoardControl;
        private SynchronizationContext m_context;

        public Window2()
        {
            InitializeComponent();
            m_context = SynchronizationContext.Current;

            AddCustomControls();
        }

        private void AddCustomControls()
        {
            Common.MediaControl c = new Common.MediaControl();
            MediaControls.Children.Add(c);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
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

                m_InputInitialised = true;
                MessageBox.Show("Initialised");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialisation Failed.  Err Msg: " + ex.Message);
            }
        }

        private void UpdateMessageReceived(object sender, EventArgs e)
        {

            lblMsgsReceived.Content = m_Session.NumMsgsReceived;

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            m_Session.Sequencer.Stop();
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
            //Post on the main GUI thread...
            m_context.Post( 
                    _ => m_keyBoardControl.HandleMessage(sender, SanfordUtils.BuildKeyStrokeEventArgs(e.Message))
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
            if (m_keyBoardControl == null) AddKeyBoard();
            //if (!ControlHelpers.IsControlLoaded<Viewbox>(ParentWindow, KeyboardViewName))
            //{
            //    AddKeyBoard();
            //}
        }

        private void HideKeyboard_Click(object sender, RoutedEventArgs e)
        {
            Viewbox vb =  ControlHelpers.GetControl<Viewbox>(ParentWindow, KeyboardViewName);
            if (vb != null) ParentWindow.Children.Remove(vb);
            m_keyBoardControl = null;
        }

        private void AddKeyBoard()
        {
            //Add the control in a viewbox to support scaling
            var viewbox = new Viewbox();
            m_keyBoardControl = new KeyBoardControl(8);
            viewbox.Child = m_keyBoardControl;
            viewbox.Name = KeyboardViewName;
            viewbox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            ParentWindow.Children.Add(viewbox);
            //If the input is initialised, hook up the keyboard to the midi events
            if (m_InputInitialised)
            {
                m_Session.MessageReceived += m_keyBoardControl.HandleMessage;
            }
        }

        private void StopPlayBack_Click(object sender, RoutedEventArgs e)
        {
            m_Session.Sequencer.Stop();

        }

        private void LoadFile(string fileName)
        {
            Sequence seq = new Sequence(fileName);
            m_Session.Sequencer.Sequence = seq;
        }
    }
}
