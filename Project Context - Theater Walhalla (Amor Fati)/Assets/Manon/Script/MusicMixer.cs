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
    AudioClip[] recordings;
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
        // Double check if all audio tracks are recorded
        microphoneFrequency = recordingsHandler.microphoneFrequency;
        AudioClip recording = recordingsHandler.recording;
        if (recording == null) return;
     /*   recordings = new AudioClip[] { recording }; // TODO : not in array */
         /*recordings = recordingsHandler.recordedTracks; */
      /*   foreach (AudioClip r in recordings) if (r == null) break; */

        // Combine the song snippets
        // AudioClip generatedSong = CombineTotaal(recordings);

        AudioClip generatedSong = Combine(recording);
        customAudio.clip = generatedSong;
    }

    float ClampToValidRange(float value)
    {
        float min = -1.0f;
        float max = 1.0f;
        return (value < min) ? min : (value > max) ? max : value;
    }

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

    AudioClip CombineTotaal(AudioClip[] clips) // Function for multiple recordings (might be obsolete)
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

        

        // Put the karaokepart and ending together
      //  float[] resultBuffer = new float[length + lengthEnding];
     //   mixedBuffer.CopyTo(resultBuffer, 0);
      //  floatEnding.CopyTo(resultBuffer, length);

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
