using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RecordingsHandler : MonoBehaviour
{
    #region Private variables

    // Audio
    [Header("Karaoke tracks")]
    [SerializeField] public AudioClip[] recordedTracks = new AudioClip[3];
    [SerializeField] public AudioClip[] history = new AudioClip[3];
    AudioSource _audio;

    // Video
    [Header("Karaokebolletje-videos")]
    //[SerializeField] VideoPlayer track1Video;
    //[SerializeField] VideoPlayer track2Video;
   // [SerializeField] VideoPlayer track3Video;

    // Microphone
    bool microphonePresent;
    public int microphoneFrequency = 44100;
    string device;
    int duration = 5;

    // Game
    [Header("Game")]
    [HideInInspector] public int counter;
    [SerializeField] MuziekGame game;
    [SerializeField] Sprite play;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite stop;
    [SerializeField] Sprite microphone;
    [SerializeField] Button playback_track1;
    [SerializeField] Button playback_track2;
    [SerializeField] Button playback_track3;
    [SerializeField] GameObject track1_recording;
    [SerializeField] GameObject track2_recording;
    [SerializeField] GameObject track3_recording;

  //  [SerializeField] Button record_track1;
    //[SerializeField] Button record_track2;
    //[SerializeField] Button record_track3;

    #endregion

    #region Private methods

    private void Start()
    {
        // Initialize audio
        _audio = GetComponent<AudioSource>();
    }

    bool MicrophoneSetup()
    {
        // Find microphone devices
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            // Set device
            device = devices[0];

            // Get max frequency of device
            int minFreq;
            int maxFreq;
            Microphone.GetDeviceCaps(device, out minFreq, out maxFreq);
            if (maxFreq < microphoneFrequency) microphoneFrequency = maxFreq;
            return true;
        }

        else return false;
    }

    IEnumerator StartRecording(int trackNr)
    {
        // Check microphone
        microphonePresent = MicrophoneSetup();
        if (!microphonePresent) yield break;

        // Check if there was already a recording present
        history[trackNr] = recordedTracks[trackNr];

        // Start microphone if it isn't already recording
        if (!Microphone.IsRecording(device))
        {
            recordedTracks[trackNr] = Microphone.Start(device, false, duration, microphoneFrequency);

            // Update icon and play karaoke video
            if (trackNr == 0)
            {
                track1_recording.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
                track1_recording.GetComponentInChildren<VideoPlayer>().Play();
            }

            else if (trackNr == 1)
            {
                track2_recording.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
                track2_recording.GetComponentInChildren<VideoPlayer>().Play();
            }
                
                
            else if (trackNr == 2)
            {
                track3_recording.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
                track3_recording.GetComponentInChildren<VideoPlayer>().Play();
            }

            // Stop recording after set duration
            yield return new WaitForSeconds(duration);
            Microphone.End(device);

            // Change icon back
            track1_recording.GetComponentInChildren<Button>().GetComponent<Image>().sprite = microphone;
            track2_recording.GetComponentInChildren<Button>().GetComponent<Image>().sprite = microphone;
            track3_recording.GetComponentInChildren<Button>().GetComponent<Image>().sprite = microphone;

            // Stop video
            track1_recording.GetComponentInChildren<VideoPlayer>().Stop();
            track2_recording.GetComponentInChildren<VideoPlayer>().Stop();
            track3_recording.GetComponentInChildren<VideoPlayer>().Stop();

            // Process recording if this went well
            if (recordedTracks[trackNr] != null)
            {
                game.ProcessRecording(trackNr);
                if (history[trackNr] == null) counter++;
            }
        }

        yield break;
    }

    void ChangePlaybackIcon(int trackNr)
    {
        // all icons to playsetting
        playback_track1.GetComponent<Image>().sprite = play;
        playback_track2.GetComponent<Image>().sprite = play;
        playback_track3.GetComponent<Image>().sprite = play;

        if (_audio.isPlaying)
        {
            // except for the one that is playing
            if (trackNr == 0) playback_track1.GetComponent<Image>().sprite = stop;
            if (trackNr == 1) playback_track2.GetComponent<Image>().sprite = stop;
            if (trackNr == 2) playback_track3.GetComponent<Image>().sprite = stop;
        }
    }

    IEnumerator StartPlaying(int trackNr)
    {
        // current clip is already playing
        if (_audio.clip == recordedTracks[trackNr] && _audio.isPlaying)
        {
            _audio.Stop();
            ChangePlaybackIcon(trackNr);
            yield break;
        }

        // if different clip is playing, stop this clip before continuing
        else if (_audio.isPlaying)
        {
            _audio.Stop();
            ChangePlaybackIcon(trackNr);
        }

        // start playing current clip
        _audio.clip = recordedTracks[trackNr];
        _audio.Play();
        ChangePlaybackIcon(trackNr);

        while (_audio.isPlaying) yield return null;

        ChangePlaybackIcon(trackNr);
        yield break;
    }

    #endregion

    #region Public methods

    // Recording
    public void RecordTrack1() => StartCoroutine(StartRecording(0));
    public void RecordTrack2() => StartCoroutine(StartRecording(1));
    public void RecordTrack3() => StartCoroutine(StartRecording(2));

    // Playing
    public void PlayTrack1()
    {
        if (!Microphone.IsRecording(device)) StartCoroutine(StartPlaying(0));
    }
    public void PlayTrack2()
    {
        if (!Microphone.IsRecording(device)) StartCoroutine(StartPlaying(1));
    }
        
    public void PlayTrack3()
    {
        if (!Microphone.IsRecording(device)) StartCoroutine(StartPlaying(2));
    }
        
    // Re-recording
    public void RedoTrack1()
    {
        if (_audio.clip == recordedTracks[0]) _audio.Stop();
        if (!Microphone.IsRecording(device)) game.RedoRecording(0);
    }
       
    public void RedoTrack2()
    {
        if (_audio.clip == recordedTracks[1]) _audio.Stop();
        if (!Microphone.IsRecording(device)) game.RedoRecording(1);
    }
        
    public void RedoTrack3()
    {
        if (_audio.clip == recordedTracks[2]) _audio.Stop();
        if (!Microphone.IsRecording(device)) game.RedoRecording(2);
    }

    #endregion
}
