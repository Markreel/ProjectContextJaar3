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
using System.Xml.XPath;
using UnityEditorInternal;

public class MusicMixer : MonoBehaviour
{
    // Mixing the song
    [SerializeField] RecordingsHandler recordingsHandler;
    [SerializeField] AudioClip backtrack;
    [SerializeField] AudioClip backtrackKaraoke;
    [SerializeField] AudioClip karaokeEinde;
    [SerializeField] float enhanceRecording;

    int microphoneFrequency;
    AudioClip recording;
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
        microphoneFrequency = recordingsHandler.microphoneFrequency;

        // Double check if all audio tracks are recorded
        AudioClip recording = recordingsHandler.recording;
        if (recording == null) return;

        AudioClip generatedSong = Combine(recording);
        customAudio.clip = generatedSong;
    }

    float ClampToValidRange(float value)
    {
        float min = -1.0f;
        float max = 1.0f;
        return (value < min) ? min : (value > max) ? max : value;
    }

    // Total song
    AudioClip Combine(AudioClip clip)
    {
        // Determine song length
        if (backtrack.channels != 1) Debug.Log("Make sure the audiofile is mono");
        int length = backtrack.samples * backtrack.channels;

        // Set the audio data for the backtrack in a buffer
        float[] floatBackTrack = new float[length];
        backtrack.GetData(floatBackTrack, 0);

        // Hardcode the offset based on audio data
        int sampleOffset = 393317;

        // Set the audio data for the recorded tracks in another buffer
        float[] floatRecordings = new float[length];

        // Get the data of the recording
        float[] bufferRecording = new float[clip.samples * clip.channels];
        clip.GetData(bufferRecording, beepOffset);
        bufferRecording.CopyTo(floatRecordings, sampleOffset);

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

    // Karaoke part (for listening back)
    AudioClip CombineKaraoke(AudioClip recording)
    {
        // Set vars
        microphoneFrequency = recordingsHandler.microphoneFrequency;
        if (backtrackKaraoke.channels != 1) Debug.Log("Make sure the audiofile is mono");

        // Set the audio data for the backtrack in a buffer
        int karaokeLength = backtrackKaraoke.samples * backtrackKaraoke.channels;
        float[] karaokeBuffer = new float[backtrackKaraoke.samples * backtrackKaraoke.channels];
        backtrackKaraoke.GetData(karaokeBuffer, 0);

        // Also set the audio data for the final lines for niceness reasons
        int lengthEnding = karaokeEinde.samples * karaokeEinde.channels;
        float[] endBuffer = new float[lengthEnding];
        karaokeEinde.GetData(endBuffer, 0);

        // Combine these two into instrumental/music track
        float[] instrumentalBuffer = new float[karaokeLength + lengthEnding];
        karaokeBuffer.CopyTo(instrumentalBuffer, 0);
        endBuffer.CopyTo(instrumentalBuffer, karaokeLength);

        // Set the audio data for the recorded tracks in another buffer
        float[] floatRecording = new float[recording.samples * recording.channels];
        recording.GetData(floatRecording, 0);

        // Combine the instrumental/music buffer and the recording
        float[] resultBuffer = new float[karaokeLength + lengthEnding];
        resultBuffer = instrumentalBuffer;
        for (int i = 0; i < floatRecording.Length; i++)
        {
            resultBuffer[i] = ClampToValidRange((instrumentalBuffer[i] + floatRecording[i] * enhanceRecording) / 2);
        }

        // Create audio clip
        AudioClip result = AudioClip.Create("Karaoke", resultBuffer.Length, 1, microphoneFrequency, false);
        result.SetData(resultBuffer, 0);
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
    public AudioClip generateKaraoke(AudioClip recording) => CombineKaraoke(recording);
    public void PlaySong() => StartCoroutine(PlayCustomSong());
    public void DownloadButton() => StartCoroutine(DownloadSong());

    #endregion

}
