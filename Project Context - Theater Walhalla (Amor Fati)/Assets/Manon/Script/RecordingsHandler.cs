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

    // Audio
    [Header("Muziek")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip aftellen;
    [SerializeField] AudioClip eindekaraoke;
    [SerializeField] AudioClip karaokeTrack;
    [SerializeField] float volumeDuringRecording;
    public AudioSource _audio;
    public AudioClip recording;
    bool karaokePlaying;

    // Video
    [Header("Karaokebolletje-videos")]
    [SerializeField] VideoPlayer karaokevideo;

    // Microphone
    bool microphonePresent;
    public int microphoneFrequency = 44100;
    string device;
    int duration = 15;

    #endregion

    #region Private methods

    private void Start()
    {
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

    IEnumerator StartRecording()
    {
        // Check microphone
        microphonePresent = MicrophoneSetup();
        if (!microphonePresent) yield break;

        // Start the karaoke game
        if (!karaokePlaying)
        {
            karaokePlaying = true;

            // Update icon
            startKaraokeButton.GetComponent<Image>().sprite = stop;
            
            // Start aftellen
            source.clip = aftellen;
            source.Play();
            yield return new WaitWhile(() => (source.clip == aftellen && source.isPlaying));

            // Start karaoketrack and recording
            source.clip = karaokeTrack;
            source.volume = volumeDuringRecording;
          //  karaokevideo.Play();
            source.Play();
            recording = Microphone.Start(device, false, duration, microphoneFrequency);

            // Stop recording after set duration
             yield return new WaitWhile(() => (source.clip == karaokeTrack && source.isPlaying));
           // karaokevideo.Stop();

            // Play end of song for continuity
            source.clip = eindekaraoke;
            source.volume = 1f;
            source.Play();

            // Stop the recording
            Microphone.End(device);

            yield return new WaitWhile(() => (source.clip == eindekaraoke && source.isPlaying));

            // Update icon
            startKaraokeButton.GetComponent<Image>().sprite = microphone;

            // Reset boolean
            karaokePlaying = false;

            // Go to download screen
            game.DownloadState();
        }

        // End the karaoke game
        if (karaokePlaying)
        {
            // Stop karaoke
            if (source.isPlaying) { source.Stop(); source.clip = null; }
            karaokePlaying = false;
            karaokevideo.Stop();
            karaokevideo.frame = 0;

            startKaraokeButton.GetComponent<Image>().sprite = microphone;

            // Stop coroutine
            StopAllCoroutines();
        }

        yield break;
    }

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

        yield break;
    } 

    #endregion

    #region Public methods

    // Recording
    public void RecordTrack() => StartCoroutine(StartRecording());

    // Playing
    public void PlayTrack() 
    {
        if (!Microphone.IsRecording(device)) StartCoroutine(StartPlaying());
    }
        
    // Re-recording
    public void RedoTrack()
    {
        if (_audio.isPlaying) _audio.Stop();
        if (!karaokePlaying) game.RecordingState();
    }

    #endregion
}
