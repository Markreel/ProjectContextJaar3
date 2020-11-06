using System.CodeDom;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
//using UnityEditorInternal;

public class MicrophoneControle : MonoBehaviour
{

    public static MicrophoneControle Instance;

    #region Private variables
    int microphoneFrequency = 44100;
    bool microphonePresent;
    string device;
    string savePath;
    AudioSource _audio;
    AudioClip[] clips;
    AudioClip _name;
    AudioClip job;
    UnityEngine.UI.Button nameButton;
    UnityEngine.UI.Button jobButton;
    UnityEngine.UI.Button karaokeButton;
    UnityEngine.UI.Button switchButton;
    UnityEngine.UI.Button generateButton;
    UnityEngine.UI.Button downloadButton;
    bool karaokeMode;
    bool buttonMode;
    #endregion

    #region Public variables
    [HideInInspector] public AudioClip name_1;
    [HideInInspector] public AudioClip job_1;
    [HideInInspector] public AudioClip name_2;
    [HideInInspector] public AudioClip job_2;
    [HideInInspector] public AudioClip name_3;
    [HideInInspector] public AudioClip job_3;
    #endregion

    #region Adjustable variables
    [SerializeField] int nameDuration = 2; // in seconds
    [SerializeField] int jobDuration = 2; // in seconds
    [SerializeField] string songName = "generated song";
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
        Instance = this;
        savePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + saveFolder;
        nameButton = GameObject.Find("NaamButton").GetComponent<UnityEngine.UI.Button>();
        jobButton = GameObject.Find("BeroepButton").GetComponent<UnityEngine.UI.Button>();
        karaokeButton = GameObject.Find("StartButton").GetComponent<UnityEngine.UI.Button>();
        switchButton = GameObject.Find("SwitchMode").GetComponent<UnityEngine.UI.Button>();
        generateButton = GameObject.Find("RemixButton").GetComponent<UnityEngine.UI.Button>();
        downloadButton = GameObject.Find("DownloadButton").GetComponent<UnityEngine.UI.Button>();
        karaokeMode = true;
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

        // Ask for permission to use microphone
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

        // Initialize audio
        _audio = GetComponent<AudioSource>();
        yield break;
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
        _name = Microphone.Start(device, false, nameDuration, microphoneFrequency);
        yield return new WaitForSeconds(nameDuration);
        Microphone.End(device);
        Debug.Log("Recording done");

        // if job is also recorded, you can download
        if (job != null)
        {
            generateButton.gameObject.SetActive(true);
            downloadButton.gameObject.SetActive(true);
        }

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

        // if name is also recorded, you can download
        if (_name != null)
        {
            generateButton.gameObject.SetActive(true);
            downloadButton.gameObject.SetActive(true);
        }

        yield break;
    }

    bool DoPresenceCheck()
    {
        bool success = true;

        // If both name and name_1 is empty, then something went wrong with the recording
        if (_name == null && name_1 == null)
        {
            Debug.Log("Je moet nog een naam inspreken!");
            success = false;
        }

        else if (job == null && job_1 == null)
        {
            Debug.Log("Je moet nog een beroep inspreken!");
            success = false;
        }

        // If name_1 is null but name is not, then the button-method was used
        // For now, you only record your name and job once
        // So set the other variables to the one you recorded
        else if (name != null && name_1 == null)
        {
            Debug.Log("Name and job variables are assigned to name_1 to name_3 and job_1 to job_3");
            name_1 = _name;
            name_2 = _name;
            name_3 = _name;
            job_1 = job;
            job_2 = job;
            job_3 = job;
        }

        // If name_1 till name_3 and job_1 till job_3 is null, then karaoke-recording has gone wrong
        else if (name_1 == null || job_1 == null || name_2 == null || job_2 == null || name_3 == null || job_3 == null)
        {
            Debug.Log("Name and job variables were not well assigned. Try again");
            success = false;
        }

        return success;
    }

    // This functions works with the separate buttons for name and job
    // Where name and job (for now) are only recorded once)
    IEnumerator PlayCustomSong()
    {
        // Check all info is present
        bool succesfullRecording = DoPresenceCheck();
        if (!succesfullRecording) yield break;

        //if (name == null && name_1 == null)
        //{
        //    Debug.Log("Je moet nog een naam inspreken!");
        //    yield break;
        //}

        //if (job == null && job_1 == null)
        //{
        //    Debug.Log("Je moet nog een beroep inspreken!");
        //    yield break;
        //}

        // Indicator that the buttonmethod was used
        // If you used the button-method, then for now you only record your name and job once
        // So set the other variables to the ones you recorded
        //if (name_1 == null)
        //{
        //    name_1 = name;
        //    name_2 = name;
        //    name_3 = name;
        //    job_1 = job;
        //    job_2 = job;
        //    job_3 = job;
        //}

        // Combine all song snippets
        Debug.Log("Generating song...");
        clips = new AudioClip[] {kenje1, name_1, dieis1, job_1,
            kenje2, name_2, dieis2, job_2,
            kenje3, name_3, dieis3, job_3, 
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
        _audio.clip = generatedSong;
        _audio.Play();
        yield break;
    }

    IEnumerator DownloadSong()
    {
        // Check if clip is present
        if (_audio.clip == null)
        {
            Debug.Log("Je hebt nog geen nummer gemaakt!");
            yield return null;
        }

        // Download the clip
        try
        {
            Debug.Log("Downloading...");
            savePath = Path.Combine(savePath, songName + ".wav");
            SavWav.Save(savePath, _audio.clip);
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
       // AudioClip result = AudioClip.Create("Personal Song", length / 2, 2, microphoneFrequency, false); // for stereo instead of mono
        result.SetData(data, 0);

        return result;
    }

    void SwitchMode()
    {
        // Switch from karaokemode to buttonmode
        if (karaokeMode)
        {
            nameButton.gameObject.SetActive(true);
            jobButton.gameObject.SetActive(true);
            karaokeButton.gameObject.SetActive(false);
            karaokeMode = false;
            buttonMode = true;
            switchButton.GetComponentInChildren<Text>().text = "Switch to button mode";
        }

        // Switch from buttonmode to karaokemode
        else if (buttonMode)
        {
            nameButton.gameObject.SetActive(false);
            jobButton.gameObject.SetActive(false);
            karaokeButton.gameObject.SetActive(true);
            buttonMode = false;
            karaokeMode = true;
            switchButton.GetComponentInChildren<Text>().text = "Switch to karaoke mode";
        }  
    }

    #endregion

    #region Public methods
    public void NameButton() => StartCoroutine(RecordName());
    public void BeroepButton() => StartCoroutine(RecordJob());
    public void PlayButton() => StartCoroutine(PlayCustomSong());
    public void DownloadButton() => StartCoroutine(DownloadSong());
    public void ModeButton() => SwitchMode();
    #endregion 
}
