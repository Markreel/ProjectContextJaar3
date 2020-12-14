using ShooterGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MuziekGame : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Button voorbeeldButton;
    [SerializeField] Button startButton;
    [SerializeField] Button downloadButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button playButton;
    [SerializeField] Button startKaraokeButton;
    [SerializeField] Sprite play;
    [SerializeField] GameObject postRecording;
    [SerializeField] GameObject karaokeTekst;

    [Header("General")]
    [SerializeField] RecordingsHandler recordingsHandler;
    int currentState;

    #region Private variables
    enum GameState { menu, recording, downloading };

    #endregion

    #region Private methods

    private void Start()
    {
        currentState = (int)GameState.menu;
        UpdateUI(currentState);
    }

    void UpdateUI(int state)
    {
        switch (state)
        {
            case (int)GameState.menu:
                // Active elements
                voorbeeldButton.gameObject.SetActive(true);
                startButton.transform.localScale = new Vector3(1, 1, 1);
                startButton.gameObject.SetActive(true);
                tutorialButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);

                // Deactivate elements
                startKaraokeButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                downloadButton.gameObject.SetActive(false);
                postRecording.SetActive(false);
                karaokeTekst.SetActive(false);
                recordingsHandler.metronoomAnimator.SetBool("IsPlaying", false);

                // Turn off any playing audio
                if (recordingsHandler._audio.isPlaying) recordingsHandler._audio.Stop();
                if (recordingsHandler.source.isPlaying) recordingsHandler.source.Stop();

                break;

            case (int)GameState.recording:
                // Transition
                StartCoroutine(TransitionToRecording());

                // Deactive elements
                startButton.gameObject.SetActive(false);
                voorbeeldButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                downloadButton.gameObject.SetActive(false);
                postRecording.SetActive(false);

                // Active elements
                karaokeTekst.SetActive(true);
                break;

            case (int)GameState.downloading:

                // Deactivate elements
                startButton.gameObject.SetActive(false);
                voorbeeldButton.gameObject.SetActive(false);
                startKaraokeButton.gameObject.SetActive(false);
                karaokeTekst.SetActive(false);
                recordingsHandler.metronoomAnimator.SetBool("IsPlaying", false);

                // Activate elements
                postRecording.SetActive(true);
                playButton.gameObject.SetActive(true);
                downloadButton.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator TransitionToRecording()
    {
        // Make start button disappear
        LeanTween.scale(startButton.gameObject, new Vector3(0, 0, 0), 0.2f).setEaseInOutExpo();
        startKaraokeButton.gameObject.SetActive(true);
        startKaraokeButton.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(startKaraokeButton.gameObject, new Vector3(1, 1, 1), 0.2f).setEaseInExpo();
        yield break;
    }

    private void Update()
    {
        // Quit application
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    #endregion

    #region Public methods
    public void RecordingState()
    {
        currentState = (int)GameState.recording;
        UpdateUI(currentState);
    }

    public void DownloadState()
    {
        currentState = (int)GameState.downloading;
        UpdateUI(currentState);
    }

    public void ReturnToMenu()
    {
        if (currentState != (int)GameState.menu)
        {
            // Stop the karaoke
            recordingsHandler.StopAllCoroutines();
            currentState = (int)GameState.menu;
            UpdateUI(currentState);
        }
       
        else if (currentState == (int)GameState.menu)
        {
            Application.Quit();
        }
    }

    #endregion
}
