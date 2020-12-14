using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] StartKaraokeButton startKaraokeButton;
    [SerializeField] Button afluisterButton;

    // Audio
    [Header("Muziek")]
    [SerializeField] public AudioSource source;
    [SerializeField] AudioClip aftellen;
    [SerializeField] AudioClip eindekaraoke;
    [SerializeField] AudioClip karaokeTrack;
    [SerializeField] float volumeDuringRecording;
    public AudioSource _audio;
    public AudioClip recording;
    bool karaokePlaying;

    // Metronoom
    [SerializeField] public Animator metronoomAnimator;

    // Karaoketekst
    [Header("Karaoke")]
    [SerializeField] Image line1;
    [SerializeField] Image line2;
    [SerializeField] Image line3;

    [SerializeField] Sprite _all;
    [SerializeField] Sprite _ken;
    [SerializeField] Sprite _je;
    [SerializeField] Sprite _naam;
    [SerializeField] Sprite _die;
    [SerializeField] Sprite _is;
    [SerializeField] Sprite _beroep;

    // Microphone
    [Header("Microfoon")]
    bool microphonePresent;
    public int microphoneFrequency = 44100;
    string device;
    int duration = 15;

    // For the demo with Flip -- to be removed later
    [Header("Flip Noorman - predikant")]
    [SerializeField] Image flip1;
    [SerializeField] Image flip2;
    [SerializeField] Image flip3;
    [SerializeField] Image predikant1;
    [SerializeField] Image predikant2;
    [SerializeField] Image predikant3;
    [SerializeField] public Sprite flip_idle;
    [SerializeField] public Sprite flip_active;
    [SerializeField] public Sprite predikant_idle;
    [SerializeField] public Sprite predikant_active;

    #endregion

    #region Private methods

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        karaokePlaying = false;
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
            startKaraokeButton.idle = startKaraokeButton.stop_idle;
            startKaraokeButton.active = startKaraokeButton.stop_active;
            startKaraokeButton.image.sprite = startKaraokeButton.idle;
            startKaraokeButton.GetComponent<Image>().sprite = stop;

            // Start aftellen
            source.clip = aftellen;
            source.Play();
            metronoomAnimator.SetBool("IsPlaying", true);
            yield return new WaitWhile(() => (source.clip == aftellen && source.isPlaying));

            // Start karaoketrack and recording
            source.clip = karaokeTrack;
            source.volume = volumeDuringRecording;
            source.Play();

            // Start text
            StartCoroutine(ShowLyrics());
            recording = Microphone.Start(device, false, duration, microphoneFrequency);

            // Stop recording after set duration
            yield return new WaitWhile(() => (source.clip == karaokeTrack && source.isPlaying));

            // Reset last line of karaoke
            line3.sprite = _all;
            predikant3.sprite = predikant_idle;

            // Play end of song for continuity
            source.clip = eindekaraoke;
            source.volume = 1f;
            source.Play();

            // Stop the recording
            Microphone.End(device);

            yield return new WaitWhile(() => (source.clip == eindekaraoke && source.isPlaying));

            // Update icon
            startKaraokeButton.idle = startKaraokeButton.start_idle;
            startKaraokeButton.active = startKaraokeButton.start_active;
            startKaraokeButton.image.sprite = startKaraokeButton.idle;
            startKaraokeButton.GetComponent<Image>().sprite = microphone;

            // Reset boolean
            metronoomAnimator.SetBool("IsPlaying", false);
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
            line1.sprite = _all;
            line2.sprite = _all;
            line3.sprite = _all;
            metronoomAnimator.SetBool("IsPlaying", false);

            // Update icon
            startKaraokeButton.idle = startKaraokeButton.start_idle;
            startKaraokeButton.active = startKaraokeButton.start_active;
            startKaraokeButton.image.sprite = startKaraokeButton.idle;
            startKaraokeButton.GetComponent<Image>().sprite = microphone;

            // Stop coroutine
            StopAllCoroutines();
        }

        yield break;
    }

    // Show correct lyrics at correct time
    IEnumerator ShowLyrics()
    {
        // Hardcoded timesamples
        int _ken1 = 16283;
        int _je1 = 29595;
        int _naam1 = 106651;
        int _die1 = 123035;
        int _is1 = 135067;
        int _beroep1 = 213915;

        int _ken2 = 225691;
        int _je2 = 241051;
        int _naam2 = 320411;
        int _die2 = 337563;
        int _is2 = 346267;
        int _beroep2 = 425371;

        int _ken3 = 439195;
        int _je3 = 452763;
        int _naam3 = 532123;
        int _die3 = 547995;
        int _is3 = 557979;
        int _beroep3 = 640052;

        // Reset the lyrics
        line1.sprite = _all;
        line2.sprite = _all;
        line3.sprite = _all;

        // Show correct image
        // Flip/Predikant should be commented out en _naam & _beroep gebruiken op de lijn ipv _all
        while (source.isPlaying)
        {
            // Line 1
            if (source.timeSamples < _ken1) line1.sprite = _ken;
            else if (source.timeSamples < _je1) line1.sprite = _je;
            else if (source.timeSamples < _naam1)
            {
              //  line1.sprite = _naam;
                line1.sprite = _all;
                flip1.sprite = flip_active;
            }


            else if (source.timeSamples < _die1)
            {
                flip1.sprite = flip_idle;
                line1.sprite = _die;
            }
                
            else if (source.timeSamples < _is1) line1.sprite = _is;
            else if (source.timeSamples < _beroep1)
            {
                // line1.sprite = _beroep;
                line1.sprite = _all;
                predikant1.sprite = predikant_active;
            }

            // Line 2
            else if (source.timeSamples < _ken2)
            {
                line1.sprite = _all;
                line2.sprite = _ken;
                predikant1.sprite = predikant_idle;
            }
               
            else if (source.timeSamples < _je2) line2.sprite = _je;
            else if (source.timeSamples < _naam2)
            {
                //   line2.sprite = _naam;
                line2.sprite = _all;
                flip2.sprite = flip_active;
            }
                
            else if (source.timeSamples < _die2)
            {
                flip2.sprite = flip_idle;
                line2.sprite = _die;
            }
               
            else if (source.timeSamples < _is2) line2.sprite = _is;
            else if (source.timeSamples < _beroep2)
            {
                // line2.sprite = _beroep;
                line2.sprite = _all;
                predikant2.sprite = predikant_active;
            }
               

            // Line 3
            else if (source.timeSamples < _ken3)
            {
                predikant2.sprite = predikant_idle;
                line2.sprite = _all;
                line3.sprite = _ken;
            }
            else if (source.timeSamples < _je3) line3.sprite = _je;
            else if (source.timeSamples < _naam3)
            {
                //  line3.sprite = _naam;
                line3.sprite = _all;
                flip3.sprite = flip_active;
            }
               
            else if (source.timeSamples < _die3)
            {
                flip3.sprite = flip_idle;
                line3.sprite = _die;
            }
                
            else if (source.timeSamples < _is3) line3.sprite = _is;
            else if (source.timeSamples < _beroep3)
            {
                // line3.sprite = _beroep;
                line3.sprite = _all;
               
                predikant3.sprite = predikant_active;
            }
                
            yield return null;
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

            yield return new WaitWhile(() => (_audio.clip == karaokeClip && _audio.isPlaying));
            afluisterButton.GetComponent<Image>().sprite = play;
        }

        yield break;
    }

    #endregion

    #region Public methods

    // Check microphone
    public bool MicrophoneSetup()
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
