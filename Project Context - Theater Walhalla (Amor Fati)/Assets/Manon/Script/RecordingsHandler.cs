using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RecordingsHandler : MonoBehaviour
{
    #region Private variables

    // Audio
    [Header("Muziek")]
   /* [SerializeField] public AudioClip[] recordedTracks = new AudioClip[3];
    [SerializeField] public AudioClip[] history = new AudioClip[3]; */
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip aftellen;
    [SerializeField] AudioClip eindekaraoke;
    [SerializeField] AudioClip karaokeTrack;
    [SerializeField] float volumeDuringRecording;
    AudioSource _audio;
    public AudioClip recording;
    bool karaokePlaying;

    // Video
    [Header("Karaokebolletje-videos")]
    [SerializeField] VideoPlayer karaokevideo;
  /*  [SerializeField] VideoPlayer track1Video;
    [SerializeField] VideoPlayer track2Video;
    [SerializeField] VideoPlayer track3Video; */

    // Microphone
    bool microphonePresent;
    public int microphoneFrequency = 44100;
    string device;
    int duration = 15;

    // Game
    [Header("Game")]
    [HideInInspector] public int counter;
    [SerializeField] MuziekGame game;
    [SerializeField] MusicMixer mixer;
    [SerializeField] Sprite play;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite stop;
    [SerializeField] Sprite microphone;
    [SerializeField] Button startKaraokeButton;
    [SerializeField] Button afluisterButton;


   //  [SerializeField] Button playback_track1;
   /* [SerializeField] Button playback_track2;
    [SerializeField] Button playback_track3;
    [SerializeField] GameObject track1_recording;
    [SerializeField] GameObject track2_recording;
    [SerializeField] GameObject track3_recording; */
    //Vector3 recordTrackStartPos;
    //Vector3 recordTrackEndPos;

    #endregion

    #region Private methods

    private void Start()
    {
        // Initialize audio
        _audio = GetComponent<AudioSource>();
        karaokePlaying = false;
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

    //IEnumerator AnimateRecordButton(float duration)
    //{
    //    float tick = 0f;
    //    bool normalSize = startKaraokeButton.transform.localScale == new Vector3(1, 1, 1);

    //    // Move and downscale the button
    //    while (tick < 1f)
    //    {
    //        tick += Time.deltaTime / (duration > 0 ? duration : 1);
    //        LeanTween.scale(startKaraokeButton.gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.75f);
    //        startKaraokeButton.transform.position = Vector3.Lerp(recordTrackStartPos, recordTrackEndPos, tick);
    //        yield return null;
    //    }
    //    yield break;
    //}

    IEnumerator StartRecording()
    {
        // Check microphone
        microphonePresent = MicrophoneSetup();
        if (!microphonePresent) yield break;

        // Check if there was already a recording present
       /* history[trackNr] = recordedTracks[trackNr]; */

        // Start game it if it isn't currently playing
     //   if (!Microphone.IsRecording(device))
        if (!karaokePlaying)
        {
            karaokePlaying = true;

            // Animation on recording
            //  LeanTween.scale(startKaraokeButton.gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.75f);
            // StartCoroutine(AnimateRecordButton(1f));

            // Update icon
            startKaraokeButton.GetComponent<Image>().sprite = stop;
            startKaraokeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop karaoke";
            //startKaraokeButton.GetComponentInChildren<TextMeshPro>().text = "Stop karaoke";

            
            // Start aftellen
            source.clip = aftellen;
            source.Play();
            yield return new WaitWhile(() => (source.clip == aftellen && source.isPlaying));

            // Start karaoketrack and recording
            source.clip = karaokeTrack;
            source.volume = volumeDuringRecording;
            karaokevideo.Play();
            source.Play();
           /* track1Video.Play(); */
            recording = Microphone.Start(device, false, duration, microphoneFrequency);

            // Stop recording after set duration
             yield return new WaitWhile(() => (source.clip == karaokeTrack && source.isPlaying));
            // yield return new WaitForSeconds(duration);
            karaokevideo.Stop();

            // Play end of song for continuity
            source.clip = eindekaraoke;
            source.volume = 1f;
            source.Play();

            // Stop the recording
            Microphone.End(device);

            yield return new WaitWhile(() => (source.clip == eindekaraoke && source.isPlaying));

            // Update icon
            startKaraokeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start karaoke";
            startKaraokeButton.GetComponent<Image>().sprite = microphone;

            // Reset boolean
            karaokePlaying = false;

            // Go to download screen
            game.DownloadState();
            
          /*  track1Video.Stop(); */

            /*  recordedTracks[trackNr] = Microphone.Start(device, false, duration, microphoneFrequency);

              // Update icon and play karaoke video
              if (trackNr == 0)
              {
                  track1_recording.GetComponent<Image>().sprite = play;
                  track1Video.Play();
              }

              else if (trackNr == 1)
              {
                  track2_recording.GetComponent<Image>().sprite = play;
                  track2Video.Play();
              }


              else if (trackNr == 2)
              {
                  track3_recording.GetComponent<Image>().sprite = play;
                  track3Video.Play();
              } */

            /*   // Stop recording after set duration
               yield return new WaitForSeconds(duration);
               Microphone.End(device); */

            // Change icon back
        /*    track1_recording.GetComponent<Image>().sprite = microphone; */
          /*  track2_recording.GetComponent<Image>().sprite = microphone;
            track3_recording.GetComponent<Image>().sprite = microphone; */

            // Stop video
        /*    track1Video.Stop(); */
        /*    track2Video.Stop();
            track3Video.Stop(); */

            // Process recording if this went well
         /*   if (recordedTracks[trackNr] != null)
            {
                game.ProcessRecording(trackNr);
                if (history[trackNr] == null) counter++;
            } */
        }

        if (karaokePlaying)
        {
            // Stop karaoke
            if (source.isPlaying) { source.Stop(); source.clip = null; }
            karaokePlaying = false;
            karaokevideo.Stop();
            karaokevideo.frame = 0;

            startKaraokeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start karaoke";
            startKaraokeButton.GetComponent<Image>().sprite = microphone;

            // Stop coroutine
            StopAllCoroutines();
        }

     /*   if (counter == 3) game.DownloadState(); */

        yield break;
    }

 /*   void ChangePlaybackIcon(int trackNr)
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
    } */

    IEnumerator StartPlaying()
    {
        if (_audio.isPlaying)
        {
            _audio.Stop();
            afluisterButton.GetComponent<Image>().sprite = play;
        }

        else
        {
            AudioClip karaokeClip = mixer.generateKaraoke(recording);
            _audio.clip = karaokeClip;
            _audio.Play();
            afluisterButton.GetComponent<Image>().sprite = stop;
        }


        // TODO: If it is already playing

        //if (_audio.isPlaying)
        //{
        //    _audio.Stop();
        //    playback_track1.GetComponent<Image>().sprite = play;
        //}

        //else
        //{
        //    _audio.Play();
        //    playback_track1.GetComponent<Image>().sprite = stop;
        //}

        yield break;
        // ALLES OPNIEUW LINKEN


       /* // current clip is already playing
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
        yield break; */
    } 

    #endregion

    #region Public methods

    // Recording
    public void RecordTrack() => StartCoroutine(StartRecording());
  /*  public void RecordTrack2() => StartCoroutine(StartRecording(1));
    public void RecordTrack3() => StartCoroutine(StartRecording(2)); */

    // Playing
    public void PlayTrack()
    {
        if (!Microphone.IsRecording(device)) StartCoroutine(StartPlaying());
    }
  /*  public void PlayTrack2()
    {
        if (!Microphone.IsRecording(device)) StartCoroutine(StartPlaying(1));
    }
        
    public void PlayTrack3()
    {
        if (!Microphone.IsRecording(device)) StartCoroutine(StartPlaying(2));
    } */
        
    // Re-recording
    public void RedoTrack()
    {
       /* if (_audio.clip == recordedTracks[0]) _audio.Stop(); */
        if (_audio.isPlaying) _audio.Stop();
        //   if (!Microphone.IsRecording(device)) game.RedoRecording(0);
        if (!karaokePlaying) game.RecordingState();
    }
       
  /*  public void RedoTrack2()
    {
        if (_audio.clip == recordedTracks[1]) _audio.Stop();
        if (!Microphone.IsRecording(device)) game.RedoRecording(1);
    }
        
    public void RedoTrack3()
    {
        if (_audio.clip == recordedTracks[2]) _audio.Stop();
        if (!Microphone.IsRecording(device)) game.RedoRecording(2);
    } */

    #endregion
}
