using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sanford.Multimedia.Midi;
using System.Diagnostics;
using KeyBoardControlLibrary;
using Common.Events;
using Common.Media;
using Common.Devices;

namespace MrKeys
{
  /// <summary>
  /// Records input from Midi keyboard
  /// </summary>
  class RecordSession : IMediaService, IDeviceStatusService
  {

    // Module Level Vars
    private InputDevice m_InDevice;
    private SynchronizationContext m_context;
    private RecordingSession m_Session;
    
    public delegate void MessageReceivedHandler();
    public event PianoKeyStrokeEvent MessageReceived;

    #region ctor
    public RecordSession()
    {
        //TODO: This should be removed later...
        CanPlay = true;
    }
    #endregion

    #region Properties

    /// <summary>
    /// Sequencer property responsible for playing midi information
    /// </summary>
    public Sequencer Sequencer
    {
      get;
      set;
    }

    /// <summary>
    ///  Midi Clock
    /// </summary>
    public MidiInternalClock Clock
    {
      get;
      set;
    }

    /// <summary>
    /// Number of messages received in the current session
    /// </summary>
    public int NumMsgsReceived
    {
      get;
      set;
    }

    /// <summary>
    /// Session containing Received Midi Data
    /// </summary>
    public RecordingSession RecordingSession
    {
      get
      {
        return m_Session;
      }
    }

    /// <summary>
    /// True if a recording is currently in progress
    /// </summary>
    public bool IsRecording
    {
      get;
      set;
    }

      /// <summary>
      /// True if playback is paused
      /// </summary>
    public bool IsPaused { get; set; }
    #endregion // Properties


    #region Public

    /// <summary>
    /// Init method
    /// </summary>
    public void Init()
    {
      // Init
      InputDevice inDevice;
      m_context = SynchronizationContext.Current;
      IsRecording = false;
      this.Clock = new MidiInternalClock();
      m_Session = new RecordingSession(this.Clock);
      this.Sequencer = new Sequencer();
      

      // Set up the midi device
      if (InputDevice.DeviceCount == 0)
      {
        throw new Exception("No midi devices detected");
      }
      else
      {
        try
        {
          // Set up device and link to handler
          inDevice = new InputDevice(0);
          inDevice.ChannelMessageReceived += HandleChannelMessageReceived;
          m_InDevice = inDevice;
          CanPlay = true;
          CanRecord = true;
          IsInitialised = true;
        }
        catch (Exception ex)
        {
          Exceptions.ErrHandler(ex);
        }
      }
    }

    /// <summary>
    ///  Clears and resets the current session
    /// </summary>
    public void Clear()
    {
      // Remove the recording session
      m_Session.Clear();
      //#TODO: Un comment this? this.Sequencer.Sequence = null;
      this.NumMsgsReceived = 0;
    }


    /// <summary>
    /// Starts recording the Midi input
    /// </summary>
    public void StartRecording()
    {
      
    }


    /// <summary>
    /// Stops recording Midi input
    /// </summary>
    public void StopRecording()
    {


    }

    /// <summary>
    /// Playback the current sequence
    /// </summary>
    public void Playback()
    {

      // Build a track object from
      // the current session
      m_Session.Build();
      Track trk = m_Session.Result;

      // Create a new sequence
      // from the current track
      Sequence seq = new Sequence();
      seq.Add(trk);

      // Load the sequence into
      // the sequencer
      Sequencer.Sequence = seq;

      // Start playing
      Sequencer.Start();
    }
    #endregion 


    #region Private

    /// <summary>
     /// Handles the Midi message received
     /// Only called if in recording mode
     /// </summary>
     /// <param name="sender"></param>
     /// <param name="e"></param>
    private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
    {
      
      m_context.Post(delegate(object dummy)
      {
        // Create new channel message object and add to session
          ChannelMessage msg = e.Message;
        //todo: maybe reimplement this if broken new ChannelMessage(e.Message.Command,e.Message.MidiChannel,e.Message.Data1,e.Message.Data2);

        m_Session.Record(msg);

        // Update number of messages received and raise event
        this.NumMsgsReceived++;
        if (MessageReceived != null) MessageReceived(this, SanfordUtils.ConvertChannelMessageToKeyStrokeEventArgs(msg));
      
      }, null);

    } // End Proc

    #endregion

    #region MediaServiceImplementation
    public bool CanPlay { get; set; }

    public bool CanStop { get; set; }

    public bool CanPause { get; set; }

    public bool CanRecord { get; set; }

    public void Play()
    {
        
        // Build a track object from
        // the current session
        m_Session.Build();
        Track trk = m_Session.Result;

        // Create a new sequence
        // from the current track
        Sequence seq = new Sequence();
        seq.Add(trk);

        // Load the sequence into
        // the sequencer
        Sequencer.Sequence = seq;

        // Start playing
        Sequencer.Start();

        CanPlay = false;
        CanPause = true;
    }

    public void Stop()
    {
        if (IsRecording)
        {
            // Stop recording
            try
            {
                m_InDevice.StopRecording();
                this.Clock.Stop();
                IsRecording = false;
            }
            catch (Exception ex)
            {
                Exceptions.ErrHandler(ex);
            }
        }


        
        CanPlay = true;
        
    }

    public void Pause()
    {
        if (IsPaused)
        {
            Sequencer.Continue();
            IsPaused = false;
        }
        else
        {
            Sequencer.Stop();
            IsPaused = true;
        }
    }

    public void Record()
    {

        // Clear current sequence if any
        this.Clear();

        // Start recording
        try
        {
            m_InDevice.StartRecording();
            this.Clock.Start();
            IsRecording = true;
        }
        catch (Exception ex)
        {
            Exceptions.ErrHandler(ex);
        }

    }
    #endregion 

    #region DeviceStatusService implementation

    public bool IsInitialised { get; private set; }

    public string Name { get { return "I shouldn't be hardcoded!"; } }

    #endregion 
  } // End Class
} // End Namespace
