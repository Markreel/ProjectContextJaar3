using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Linq;

public class MusicMixer : MonoBehaviour
{
    // Mixing the song
    [SerializeField] RecordingsHandler recordingsHandler;
    [SerializeField] AudioClip backtrack;
    [SerializeField] float enhanceRecording;

    int microphoneFrequency;
    AudioClip[] recordings;
    AudioSource customAudio;
    int beepOffset = 3800;

    // Downloading the song
    string defaultPath;
    [SerializeField] string songName = "Bullshit-Baan";
    [SerializeField] string saveFolder = "\\Downloads";

    // UI
    [SerializeField] Sprite play;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite stop;
    [SerializeField] Button playButton;

    private void Start()
    {
        customAudio = GetComponent<AudioSource>();
        defaultPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + saveFolder;
    }

    IEnumerator PlayCustomSong()
    {
        if (!customAudio.isPlaying)
        {
            // Play song
            GenerateSong();
            customAudio.Play();
            while (customAudio.isPlaying)
            {
                playButton.GetComponent<Image>().sprite = stop;
                yield return null;
            }
            playButton.GetComponent<Image>().sprite = play;
            yield break;
        }

        if (customAudio.isPlaying)
        {
            customAudio.Stop();
            playButton.GetComponent<Image>().sprite = play;
        }
    }

    void GenerateSong()
    {
        // Double check if all audio tracks are recorded
        microphoneFrequency = recordingsHandler.microphoneFrequency;
        recordings = recordingsHandler.recordedTracks;
        foreach (AudioClip r in recordings) if (r == null) break;

        // Combine the song snippets
        AudioClip generatedSong = Combine(recordings);
        customAudio.clip = generatedSong;
    }

    float ClampToValidRange(float value)
    {
        float min = -1.0f;
        float max = 1.0f;
        return (value < min) ? min : (value > max) ? max : value;
    }

    AudioClip Combine(AudioClip[] clips)
    {
        // Determine song length
        if (backtrack.channels != 1) Debug.Log("Make sure the audiofile is mono");
        int length = backtrack.samples * backtrack.channels;

        // Set the audio data for the backtrack in a buffer
        float[] floatBackTrack = new float[length];
        backtrack.GetData(floatBackTrack, 0);

        // Hardcode the offsets based on audio data
        int sampleOffset1 = 393317;
        int sampleOffset2 = 608232;
        int sampleOffset3 = 819670;

        // Set the audio data for the recorded tracks in another buffer
        float[] floatRecordings = new float[length];

        // Get the data of track 1
        float[] bufferT1 = new float[clips[0].samples * clips[0].channels];
        clips[0].GetData(bufferT1, beepOffset);
        bufferT1.CopyTo(floatRecordings, sampleOffset1);
        // Get the data of track 2
        float[] bufferT2 = new float[clips[1].samples * clips[1].channels];
        clips[1].GetData(bufferT2, beepOffset);
        bufferT2.CopyTo(floatRecordings, sampleOffset2);
        // Get the data of track 3
        float[] bufferT3 = new float[clips[2].samples * clips[2].channels];
        clips[2].GetData(bufferT3, beepOffset);
        bufferT3.CopyTo(floatRecordings, sampleOffset3);

        // Combine the two tracks
        float[] mixedFloatArray = new float[length];
        for (int i = 0; i < length; i++)
        {
            mixedFloatArray[i] = ClampToValidRange((floatBackTrack[i] + floatRecordings[i] * enhanceRecording) / 2);
        }

        // Create an audioclip
        AudioClip result = AudioClip.Create("Personal song", length, 1, microphoneFrequency, false);
        result.SetData(mixedFloatArray, 0);
        return result;
    }

    IEnumerator DownloadSong()
    {
        // Check if generated clip is present
        if (customAudio.clip == null) GenerateSong();

        // Try downloading the clip
        try
        {
            StartCoroutine(ShowSaveDialog());
        }
        
        catch(System.Exception e)
        {
            yield break;
        }
    }

    IEnumerator ShowSaveDialog()
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, defaultPath, songName + DateTime.Now.ToString("-yyyy-dd-M--HH-mm-ss"), "Selecteer map");
        if (FileBrowser.Success)
        {
            SavWav.Save(FileBrowser.Result.First(), customAudio.clip);
        }
        yield break;
    }

    #region Public methods

    public void PlaySong() => StartCoroutine(PlayCustomSong());
    public void DownloadButton() => StartCoroutine(DownloadSong());

    #endregion

}
