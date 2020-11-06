using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    #region Private variables
    // Slider
    private Slider slider;
    private float targetProgress = 0;
    private float speed;
    [SerializeField] float FillSpeed = 0.5f;

    // Karaoke track
    private AudioSource bullshitbanen;
    private AudioClip fullSong;
    private bool silentStart; 
    private float lengthFullSong;
    private float updateStep = 0.1f;
    private int sampleDataLength = 1024;
    private float currentUpdateTime = 0f;
    private float clipLoudness;
    private float[] clipSampleData;

    // Recordings
    int microphoneFrequency = 44100;
    bool microphonePresent;
    string device;
    private bool currentlyRecording;
    private AudioClip[] recordings;
    private int clipCounter;
    private int duration = 2;

    // UI
    Button nameButton;
    Button jobButton;
    Button generateButton;
    Button downloadButton;

    // References to other scripts
    [SerializeField] MicrophoneControle microphoneControl;
    #endregion

    #region Private methods

    // TO DO:
    // - Sort of presentable UI
    // - Deze code en die van MicrophoneControl cleanen
    // - Make sure it records 6 clips (at the right time; recordings-size should then also be 6) (gaat mis aan begin)
    // - Fix progressbar (stops too early or speed too slow)
    // - Progressbar (user feedback when to record eg)
    // - Make sure recording has approx. same volume as song

    private void Awake()
    {
        // Init objects
        slider = gameObject.GetComponent<Slider>();
        bullshitbanen = GameObject.Find("FullSong").GetComponent<AudioSource>();
        fullSong = bullshitbanen.clip;
        nameButton = GameObject.Find("NaamButton").GetComponent<Button>();
        jobButton = GameObject.Find("BeroepButton").GetComponent<Button>();
        generateButton = GameObject.Find("RemixButton").GetComponent<Button>();
        downloadButton = GameObject.Find("DownloadButton").GetComponent<Button>();
        nameButton.gameObject.SetActive(false);
        jobButton.gameObject.SetActive(false);
        generateButton.gameObject.SetActive(false);
        downloadButton.gameObject.SetActive(false);

        // Set variables
        lengthFullSong = fullSong.length;
        speed = 1 / lengthFullSong;
        clipSampleData = new float[sampleDataLength];
        recordings = new AudioClip[8];
        currentlyRecording = false;
        silentStart = true;
    }

    IEnumerator Start()
    {
        // Find microphone devices
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            // Set device
            microphonePresent = true;
            device = devices[0];
            Debug.Log("Current device:" + device);

            // Get max frequency of device
            int minFreq;
            int maxFreq;
            Microphone.GetDeviceCaps(device, out minFreq, out maxFreq);
            if (maxFreq < microphoneFrequency) microphoneFrequency = maxFreq;
        }
        else yield break;

        yield break;
    }

    IEnumerator RecordClip(int counter)
    {
        // Return if no microphone is present
        if (!microphonePresent)
        {
            Debug.Log("No microphone detected!");
            yield break;
        }

        // Record the job clip
        Debug.Log("Recording clip " + counter);
        recordings[counter] = Microphone.Start(device, false, duration, microphoneFrequency);
        yield return new WaitForSeconds(duration);
        Microphone.End(device);
        Debug.Log("Recording of clip " + counter + " is done");
        yield break;
    }

    IEnumerator StartGame()
    {
        // Play audio
        clipCounter = 0;
        IncrementProgress(1);
        bullshitbanen.Play();

        // Start karaoke session
        while (bullshitbanen.isPlaying)
        {
            // Check volume of clip (every 100ms)
            currentUpdateTime += Time.deltaTime;
            if (currentUpdateTime >= updateStep)
            {
                currentUpdateTime = 0f;
                bullshitbanen.clip.GetData(clipSampleData, bullshitbanen.timeSamples);
                clipLoudness = 0f;
                foreach (var sample in clipSampleData) clipLoudness += Mathf.Abs(sample);
                clipLoudness /= sampleDataLength;
                yield return null;
            }

            // In the silent moments in the audio, turn on microphone to record clips
            if (clipLoudness < 0.01f && !silentStart)
            {
                if (!currentlyRecording)
                {
                    currentlyRecording = true;
                    StartCoroutine(RecordClip(clipCounter));
                    clipCounter++;
                }
            }

            // Toggle the start boolean if Flip starts singing for the first time
            else if (clipLoudness > 0.01f && silentStart)
            {
                Debug.Log("Flip starts his chanson");
                silentStart = false;
            }

            // If Flip is singing, stop recording
            else if (clipLoudness > 0.01f && currentlyRecording) 
            {
                Debug.Log("Recording is off!");
                currentlyRecording = false;
            }

            // Advance slider
            if (slider.value < targetProgress)
            {
                slider.value += speed * Time.deltaTime;
                yield return null;
            }
        }

        // Set the variables of the microphone control 
        microphoneControl.name_1 = recordings[0];
        microphoneControl.job_1 = recordings[1];
        microphoneControl.name_2 = recordings[2];
        microphoneControl.job_2 = recordings[3];
        microphoneControl.name_3 = recordings[4];
        microphoneControl.job_3 = recordings[5];

        // Show buttons
        generateButton.gameObject.SetActive(true);
        downloadButton.gameObject.SetActive(true);

        yield break;
    }

    #endregion

    #region Public methods
    // Add progress to the bar
    public void IncrementProgress(float updateProgress) => targetProgress = slider.value + updateProgress;
    public void StartKaraoke() => StartCoroutine(StartGame());
    #endregion
}
