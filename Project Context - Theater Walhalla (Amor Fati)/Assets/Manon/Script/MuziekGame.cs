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
    [SerializeField] GameObject karaokevideo;
    [SerializeField] GameObject postRecording;
    /*[SerializeField] GameObject track1Video;
    [SerializeField] GameObject track2Video;
    [SerializeField] GameObject track3Video; */

  //  [Header("Track elements")]
  /*  [SerializeField] GameObject track1_recording;
    [SerializeField] GameObject track2_recording;
    [SerializeField] GameObject track3_recording; */
 /*   [SerializeField] GameObject track1Post;
    [SerializeField] GameObject track2Post;
    [SerializeField] GameObject track3Post; */

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

                // Deactivate recording menu
                /* track1_recording.transform.localScale = new Vector3(1, 1, 1);
                 track1_recording.gameObject.SetActive(false);
                 track2_recording.gameObject.SetActive(false);
                 track3_recording.gameObject.SetActive(false);
                 track1Post.gameObject.SetActive(false);
                 track2Post.gameObject.SetActive(false);
                 track3Post.gameObject.SetActive(false);
                 track1Video.gameObject.SetActive(false);
                 track2Video.gameObject.SetActive(false);
                 track3Video.gameObject.SetActive(false); */

                // Deactivate elements
                startKaraokeButton.gameObject.SetActive(false);
                karaokevideo.SetActive(false);
                playButton.gameObject.SetActive(false);
                downloadButton.gameObject.SetActive(false);
                postRecording.SetActive(false);

                break;

            case (int)GameState.recording:
                // Transition
                StartCoroutine(TransitionToRecording());

                // Deactive elements
                startButton.gameObject.SetActive(false);
                voorbeeldButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                downloadButton.gameObject.SetActive(false);

                // Active elements
                karaokevideo.SetActive(true);
                // track1Video.gameObject.SetActive(true);


                // voorbeeldButton.gameObject.SetActive(false);
                // startButton.gameObject.SetActive(false);

                // Activate recording menu
                /*  track1_recording.gameObject.SetActive(true);
                  track2_recording.gameObject.SetActive(true);
                  track3_recording.gameObject.SetActive(true); */

                /*   track2Video.gameObject.SetActive(true);
                   track3Video.gameObject.SetActive(true); */
                break;

            case (int)GameState.downloading:

                // Deactivate elements
                startButton.gameObject.SetActive(false);
                voorbeeldButton.gameObject.SetActive(false);
                startKaraokeButton.gameObject.SetActive(false);
                karaokevideo.SetActive(false);

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
        currentState = (int)GameState.menu;
        UpdateUI(currentState);
    }

    
    //public void ProcessRecording(int trackNr)
    //{
    //    switch (trackNr)
    //    {
    //        case 0:
    //           // track1_recording.SetActive(false);
    //            startKaraokeButton.gameObject.SetActive(false);
    //            karaokevideo.gameObject.SetActive(false);
    //            //track1Video.gameObject.SetActive(false);
    //            postRecording.SetActive(true);
    //          //  track1Post.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
    //          //  track1Post.SetActive(true);
    //            break;
    //     /*   case 1:
    //            track2_recording.SetActive(false);
    //            track2Video.gameObject.SetActive(false);
    //            track2Post.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
    //            track2Post.SetActive(true);
    //            break;
    //        case 2:
    //            track3_recording.SetActive(false);
    //            track3Video.gameObject.SetActive(false);
    //            track3Post.GetComponentInChildren<Button>().GetComponent<Image>().sprite = play;
    //            track3Post.SetActive(true);
    //            break; */
    //    }
    //} 

    // This method is the opposite of ProcessRecording
    //public void RedoRecording(int trackNr)
    //{
    //    switch (trackNr)
    //    {
    //        case 0:
    //            //    track1_recording.SetActive(true);

    //            //track1Video.gameObject.SetActive(true);
    //            karaokevideo.gameObject.SetActive(true);
    //            startKaraokeButton.gameObject.SetActive(true);
    //            postRecording.SetActive(false);
    //           // track1Video.gameObject.SetActive(true);
    //           // track1Post.SetActive(false);
    //            break;
    //      /*  case 1:
    //            track2_recording.SetActive(true);
    //            track2Video.gameObject.SetActive(true);
    //            track2Post.SetActive(false);
    //            break;
    //        case 2:
    //            track3_recording.SetActive(true);
    //            track3Video.gameObject.SetActive(true);
    //            track3Post.SetActive(false);
    //            break; */
    //    }
    //} 

    #endregion
}
