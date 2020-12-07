using ShooterGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuziekGame : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Button voorbeeldButton;
    [SerializeField] Button startButton;
    [SerializeField] Sprite play;

    [Header("Track elements")]
    [SerializeField] GameObject track1_recording;
    [SerializeField] GameObject track2_recording;
    [SerializeField] GameObject track3_recording;
    [SerializeField] GameObject track1Post;
    [SerializeField] GameObject track2Post;
    [SerializeField] GameObject track3Post;
    [SerializeField] GameObject finishGame;

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
                // Activate start menu
                voorbeeldButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(true);

                // Deactivate recording menu
                track1_recording.gameObject.SetActive(false);
                track2_recording.gameObject.SetActive(false);
                track3_recording.gameObject.SetActive(false);
                track1Post.gameObject.SetActive(false);
                track2Post.gameObject.SetActive(false);
                track3Post.gameObject.SetActive(false);

                // Deactivate final menu
                finishGame.gameObject.SetActive(false);

                break;

            case (int)GameState.recording:
                // Deactivate startmenu
                voorbeeldButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);

                // Activate recording menu
                track1_recording.gameObject.SetActive(true);
                track2_recording.gameObject.SetActive(true);
                track3_recording.gameObject.SetActive(true);
                break;

            case (int)GameState.downloading:
                finishGame.gameObject.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        // Check if download button should be visible
        if (currentState != (int)GameState.downloading && recordingsHandler.counter == 3)
        {
            currentState = (int)GameState.downloading;
            UpdateUI(currentState);
        }

        // Quit application
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    #endregion

    #region Public methods
    public void StartGame()
    {
        currentState = (int)GameState.recording;
        UpdateUI(currentState);
    }

    public void ReturnToMenu()
    {
        currentState = (int)GameState.menu;
        UpdateUI(currentState);
    }

    public void ProcessRecording(int trackNr)
    {
        switch (trackNr)
        {
            case 0:
                track1_recording.SetActive(false);
                track1Post.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
                track1Post.SetActive(true);
                break;
            case 1:
                track2_recording.SetActive(false);
                track2Post.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
                track2Post.SetActive(true);
                break;
            case 2:
                track3_recording.SetActive(false);
                track3Post.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
                track3Post.SetActive(true);
                break;
        }
    }

    // This method is the opposite of ProcessRecording
    public void RedoRecording(int trackNr)
    {
        switch (trackNr)
        {
            case 0:
                track1_recording.SetActive(true);
                track1Post.SetActive(false);
                break;
            case 1:
                track2_recording.SetActive(true);
                track2Post.SetActive(false);
                break;
            case 2:
                track3_recording.SetActive(true);
                track3Post.SetActive(false);
                break;
        }
    }

    #endregion
}
