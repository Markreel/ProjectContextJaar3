using System.CodeDom;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

public class MicrophoneControle : MonoBehaviour
{

    public static MicrophoneControle Instance;

    #region Private variables
    int microphoneFrequency = 44100;
    bool microphonePresent;
    string device;
    string savePath;
    AudioSource audio;
    AudioClip[] clips;
    #endregion

    #region Adjustable variables

    [SerializeField] int nameDuration = 2; // in seconds
    [SerializeField] int jobDuration = 2; // in seconds
    [SerializeField] string songName = "generated song";
    [SerializeField] string saveFolder = "\\Downloads";
    [SerializeField] AudioClip name;
    [SerializeField] AudioClip job;
    [SerializeField] AudioClip intro1;
    [SerializeField] AudioClip intro2;
    [SerializeField] AudioClip intro3;
    [SerializeField] AudioClip endSong;

    #endregion

    #region Private methods
    private void Awake()
    {
        Instance = this;
        savePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + saveFolder;
    }

    private IEnumerator Start()
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
        
        // Initialize audio
        audio = GetComponent<AudioSource>();
        yield break;

        // Ask for permission to use microphone
        // yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
    }

    IEnumerator RecordName()
    {
        // Return if no microphone is present
        if (!microphonePresent)
        {
            Debug.Log("No microphone detected!");
            yield break;
        }

        // Record the name clip
        Debug.Log("Recording name...");
        name = Microphone.Start(device, false, nameDuration, microphoneFrequency);
        yield return new WaitForSeconds(nameDuration);
        Microphone.End(device);
        Debug.Log("Recording done");
        yield break;
    }

    IEnumerator RecordJob()
    {
        // Return if no microphone is present
        if (!microphonePresent)
        {
            Debug.Log("No microphone detected!");
            yield break;
        }

        // Record the job clip
        Debug.Log("Recording job...");
        job = Microphone.Start(device, false, jobDuration, microphoneFrequency);
        yield return new WaitForSeconds(jobDuration);
        Microphone.End(device);
        Debug.Log("Recording done");
        yield break;
    }

    IEnumerator PlayCustomSong()
    {
        // Check all info is present
        if (name == null)
        {
            Debug.Log("Je moet nog een naam inspreken!");
            yield break;
        }
            
        if (job == null)
        {
            Debug.Log("Je moet nog een beroep inspreken!");
            yield break;
        }

        // Combine all song snippets
        Debug.Log("Generating song...");
        clips = new AudioClip[] {intro1, name, job,
            intro2, name, job,
            intro3, name, job, 
            endSong};

        AudioClip generatedSong = Combine(clips);

        // Debugging
        //for (int i = 0; i < clips.Length; i++)
        //{
        //    audio.clip = clips[i];
        //    audio.Play();
        //    Debug.Log("Playing clip " + i);
        //    yield return new WaitForSeconds(clips[i].length);
        //    Debug.Log("Turning to the next one");
        //}

        // Play song
        Debug.Log("Playing song...");
        audio.clip = generatedSong;
        audio.Play();
        yield break;
    }

    IEnumerator DownloadSong()
    {
        // Check if clip is present
        if (audio.clip == null)
        {
            Debug.Log("Je hebt nog geen nummer gemaakt!");
            yield return null;
        }

        // Download the clip
        try
        {
            Debug.Log("Downloading...");
            savePath = Path.Combine(savePath, songName + ".wav");
            SavWav.Save(savePath, audio.clip);
            yield break;
        }

        catch (Exception e)
        {
            Debug.Log("Error! " + e.Message);
            yield break;
        }
    }

    AudioClip Combine(AudioClip[] clips)
    {
        /* TODO: Clean up microphone recordings (popping sound at start) */

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
       // AudioClip result = AudioClip.Create("Personal Song", length / 2, 2, microphoneFrequency, false);
        result.SetData(data, 0);

        return result;
    }

    #endregion

    #region Public methods
    public void NameButton() => StartCoroutine(RecordName());
    public void BeroepButton() => StartCoroutine(RecordJob());
    public void PlayButton() => StartCoroutine(PlayCustomSong());
    public void DownloadButton() => StartCoroutine(DownloadSong());
    #endregion 
}
