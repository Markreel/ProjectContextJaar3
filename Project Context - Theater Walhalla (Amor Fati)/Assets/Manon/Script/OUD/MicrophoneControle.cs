using System.CodeDom;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using UnityEngine.Video;

public class MicrophoneControle : MonoBehaviour
{

    public static MicrophoneControle Instance;

    #region Private variables
   
    // UI
    //UnityEngine.UI.Button generateButton;
    //UnityEngine.UI.Button downloadButton;
  //  public UnityEngine.UI.Slider slider;

    // Karaoke track
    public AudioSource bullshitbanen;
    private bool silentStart;
    private float updateStep = 0.1f;
    private int sampleDataLength = 1024;
    private float currentUpdateTime = 0f;
    private float clipLoudness;
    private float[] clipSampleData;
    string savePath;
    AudioSource _audio;
    AudioClip[] clips;
    AudioClip name_1;
    AudioClip job_1;
    AudioClip name_2;
    AudioClip job_2;
    AudioClip name_3;
    AudioClip job_3;

    // Recordings
    int microphoneFrequency = 44100;
    bool microphonePresent;
    string device;
    private bool currentlyRecording;
    private AudioClip[] recordings;
    private int clipCounter;
    private int duration = 2; // in seconds
    #endregion

    #region Adjustable variables
    [SerializeField] GameObject microphoneObject;
    [SerializeField] GameObject exportScherm;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject downloadButton;
    [SerializeField] UnityEngine.UI.Slider slider;
    [SerializeField] UnityEngine.UI.Slider sliderPlaySong;
    [SerializeField] VideoPlayer video;
    [SerializeField] VideoClip _videoClip;
    [SerializeField] ProgressBar progressBar;
   // [SerializeField] string backgroundVideo = "RutteZegtGwnJeBekHouden.mp4";
   // [SerializeField] string downloadVideoName = "Video";
    [SerializeField] string songName = "Bullshit-Baan";
    [SerializeField] string saveFolder = "\\Downloads";
    [SerializeField] AudioClip kenje1;
    [SerializeField] AudioClip kenje2;
    [SerializeField] AudioClip kenje3;
    [SerializeField] AudioClip dieis1;
    [SerializeField] AudioClip dieis2;
    [SerializeField] AudioClip dieis3;
    [SerializeField] AudioClip endSong;

    #endregion

    #region Private methods

    private void Awake()
    {
        // Init objects
        Instance = this;
        bullshitbanen = GameObject.Find("FullSong").GetComponent<AudioSource>();
        savePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + saveFolder;
        downloadButton.gameObject.SetActive(false);
        microphoneObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        microphoneObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);

        // Set variables
        clipSampleData = new float[sampleDataLength];
        recordings = new AudioClip[8];
        currentlyRecording = false;
        silentStart = true;
      //  string videoPath = Application.streamingAssetsPath + "/" + backgroundVideo;
      //  video.url = videoPath;
    }

    private IEnumerator Start()
    {
        // Find microphone devices
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            // Set device
            microphoneObject.GetComponent<Renderer>().material.color = Color.green;
            microphonePresent = true;
            device = devices[0];
            Debug.Log("Current device:" + device);

            // Get max frequency of device
            int minFreq;
            int maxFreq;
            Microphone.GetDeviceCaps(device, out minFreq, out maxFreq);
            if (maxFreq < microphoneFrequency) microphoneFrequency = maxFreq;
        }
        else
        {
            microphoneObject.GetComponent<Renderer>().material.color = Color.red;
            yield break;
        }

        // Initialize audio
        _audio = GetComponent<AudioSource>();
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

        // Show feedback
        microphoneObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);

        // Record the job clip
        Debug.Log("Recording clip " + counter);
        recordings[counter] = Microphone.Start(device, false, duration, microphoneFrequency);
        yield return new WaitForSeconds(duration);
        Microphone.End(device);
        yield break;
    }

    IEnumerator StartGame()
    {
        // Reset slider value
        slider.value = 0;
        microphoneObject.SetActive(true);
        downloadButton.SetActive(false);

        // Play audio
        clipCounter = 0;
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
            else if (clipLoudness > 0.01f && silentStart && bullshitbanen.time > 2)
            {
                Debug.Log("Flip starts his chanson");
                silentStart = false;
            }

            // If Flip is singing, stop recording
            else if (clipLoudness > 0.01f && currentlyRecording)
            {
                // Show feedback
                microphoneObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                currentlyRecording = false;
            }

            // Advance slider
            slider.value = bullshitbanen.time / bullshitbanen.clip.length;
            yield return null;
        }

        // Set the variables of the microphone control 
        name_1 = recordings[0];
        job_1 = recordings[1];
        name_2 = recordings[2];
        job_2 = recordings[3];
        name_3 = recordings[4];
        job_3 = recordings[5];

        // Update UI
        downloadButton.gameObject.SetActive(true);
        microphoneObject.SetActive(false);

        yield break;
    }

    IEnumerator PlayCustomSong()
    {
        // Play song
        Debug.Log("Playing song...");
        if (_audio.clip == null) GenerateSong();
        _audio.Play();

        // Update slider
        while (_audio.isPlaying)
        {
            sliderPlaySong.value = _audio.time / _audio.clip.length;
            yield return null;
        }
        yield break;
    }

    void GenerateSong()
    {
        // Check if audio clips have been recorded and assigned to correctly
        if (name_1 == null || job_1 == null || name_2 == null || job_2 == null || name_3 == null || job_3 == null)
        {
            Debug.Log("Name and job variables were not well assigned. Could not generate song");
            return;
        }

        // Combine all song snippets
        Debug.Log("Generating song...");
        clips = new AudioClip[] {kenje1, name_1, dieis1, job_1,
            kenje2, name_2, dieis2, job_2,
            kenje3, name_3, dieis3, job_3,
            endSong};
        AudioClip generatedSong = Combine(clips);
        _audio.clip = generatedSong;
    }

    //IEnumerator WaitForRequest(WWW www)
    //{
    //    yield return www;
    //    if (www.error == null) Debug.Log("WWW ok!: " + www.text);
    //    else Debug.Log("WWW Error: " + www.error);
    //}

    IEnumerator DownloadSong()
    {
        // Check if clip is present
        if (_audio.clip == null) GenerateSong();

        /* 
        // Set generated song to videoclip
        video.audioOutputMode = VideoAudioOutputMode.AudioSource;
        video.SetTargetAudioSource(0, _audio);

        // Prepare video download
        WWW www = new WWW(video.url);
        StartCoroutine(WaitForRequest(www)); */

        // Download the clip
        try
        {
            Debug.Log("Downloading...");
          /*  // Video
            string downloadPath = Path.Combine(savePath, downloadVideoName + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".mp4");
            File.WriteAllBytes(downloadPath, www.bytes); */

            // Audio
            string saveAt = Path.Combine(savePath, songName + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".wav");
            SavWav.Save(saveAt, _audio.clip);
            yield break;
        }

        catch (System.Exception e)
        {
            Debug.Log("Error! " + e.Message);
            yield break;
        }
    }

    AudioClip Combine(AudioClip[] clips)
    {
        // Check if there are clips
        if (clips == null || clips.Length == 0) return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null) continue;
            if (clips[i].channels != 1) Debug.Log("Please make sure all clips have same nr of channels to avoid weird pitch changes");
            length += clips[i].samples * clips[i].channels;
        }

        // Set data
        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null) continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        // Combine clips
        if (length == 0) return null;
        AudioClip result = AudioClip.Create("Personal Song", length, 1, microphoneFrequency, false);
        /* For stereo instead of mono, use:
         * AudioClip result = AudioClip.Create("Personal Song", length / 2, 2, microphoneFrequency, false); */
        result.SetData(data, 0);

        return result;
    }

    void ReturnToGame()
    {
        if (_audio.isPlaying) _audio.Stop();
        exportScherm.SetActive(false);
        startButton.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    #endregion

    #region Public methods
    public void PlayButton() => StartCoroutine(PlayCustomSong());
    public void DownloadButton() => StartCoroutine(DownloadSong());
    public void StartKaraoke() => StartCoroutine(StartGame());

    public void ReturnButton() => ReturnToGame();
    #endregion


}
