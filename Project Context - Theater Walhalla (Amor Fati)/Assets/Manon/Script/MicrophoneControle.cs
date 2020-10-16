using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MicrophoneControle : MonoBehaviour
{

    public static MicrophoneControle Instance;

    #region Private variables
    int microphoneFrequency = 44100;
    bool microphonePresent;
    string device;
    AudioSource audio;
    AudioClip[] clips;
    #endregion

    #region Adjustable variables

    [SerializeField] int nameDuration = 2; // in seconds
    [SerializeField] int jobDuration = 2; // in seconds
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
        else yield return null;
        
        // Initialize audio
        audio = GetComponent<AudioSource>();
        yield return null;

        // Ask for permission to use microphone
        // yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
    }

    // Record name
    IEnumerator RecordName()
    {
        // Return if no microphone is present
        if (!microphonePresent)
        {
            Debug.Log("No microphone detected!");
            yield return null;
        }

        // Record the name clip
        Debug.Log("Recording name...");
        name = Microphone.Start(device, false, nameDuration, microphoneFrequency);
        yield return new WaitForSeconds(nameDuration);
        Microphone.End(device);
        Debug.Log("Recording done");
        yield break;
    }

    // Record job
    IEnumerator RecordJob()
    {
        // Return if no microphone is present
        if (!microphonePresent)
        {
            Debug.Log("No microphone detected!");
            yield return null;
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

        // Play song
        audio.clip = generatedSong;
        audio.Play();

        // Play song
        //foreach (AudioClip snippet in clips)
        //{
        //    Debug.Log("Current snippet:" + snippet);
        //    audio.clip = snippet;
        //    audio.Play();
        //    yield return new WaitForSeconds(audio.clip.length);
        //}

    }

    AudioClip Combine(AudioClip[] clips)
    {
        // Check if there are clips
        if (clips == null || clips.Length == 0) return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null) continue;
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
        AudioClip result = AudioClip.Create("Personal Song", length / 2, 2, microphoneFrequency, false);
        result.SetData(data, 0);

        return result;
    }

    //IEnumerator RecordTwoClips()
    //{
    //    // Return if no microphone is present
    //    if (!microphonePresent)
    //    {
    //        Debug.Log("No microphone detected!");
    //        yield return null;
    //    }

    //    // Record two separate audioclips
    //    // The first one should be the name
    //    // The second one should be the job
    //    for (int i = 0; i < 2; i++)
    //    {
    //        clips[i] = Microphone.Start(device, false, recordingDuration, microphoneFrequency);
    //        Debug.Log("Recording clip " + i);
    //        yield return new WaitForSeconds(recordingDuration);
    //        Debug.Log("This clip is done!");
    //        Debug.Log("Starting new record");
    //    }

    //    Microphone.End(device);
    //    Debug.Log("Done recording all clips!");

    //    // Identify clips
    //    name = clips[0];
    //    job = clips[1];

    //    yield break;
    //}

    //IEnumerator PlayAudio()
    //{
    //    // Play all recorded pieces
    //    for (int i = 0; i < clips.Length; i++)
    //    {
    //        audio.clip = clips[i];
    //        audio.Play();
    //        Debug.Log("Playing clip " + i);
    //        yield return new WaitForSeconds(recordingDuration);
    //        Debug.Log("Turning to the next one");
    //    }

    //    // TODO: Play back on correct place in song
    //    MixSongs();

    //    yield return new WaitForSeconds(recordingDuration);
    //    Debug.Log("Played back all clips!");

    //    yield break;
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    // If the microphone is clicked, either start or stop recording
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            GameObject clickedItem = hit.collider.gameObject;
    //            if (clickedItem.transform.parent != null && (clickedItem.transform.parent).GetComponent<MicrophoneItem>())
    //            {
    //               // StartCoroutine(RecordTwoClips());
    //            }
    //        }
    //    }

    //    // Option to listen to the song
    // //   if (Input.GetKeyDown(KeyCode.P)) StartCoroutine(PlayAudio());
        
    //    // Option to redo recording

    //    // Option to save recording
    //}

    #endregion

    #region Public methods
    public void NameButton() => StartCoroutine(RecordName());
    public void BeroepButton() => StartCoroutine(RecordJob());
    public void PlayButton() => StartCoroutine(PlayCustomSong());
    #endregion 
}
