using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class OnlineVideoManager : MonoBehaviour
{
    public static OnlineVideoManager Instance;

    [SerializeField] private GameObject connectionErrorPannel;
    [SerializeField] private GameObject videoErrorPannel;

    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] TextMeshProUGUI[] errorCodes; //all errorcodes text elements

    private bool isChecking;
    private bool isPaused;

    private void Awake()
    {
        Instance = Instance ?? (this);
        if (Instance != this) { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

        videoPlayer.loopPointReached += EndOfVideoReached; 
    }

    private void EndOfVideoReached(VideoPlayer _vp)
    {
        EpisodeManager.Instance.NextEpisode();
    }

    private void Update()
    {
        //if (!videoPlayer.isPlaying && videoPlayer.time > videoPlayer.length / 10 && isChecking)
        //{
        //    Debug.Log("I AM DONE");
        //    //follow up action
        //    isChecking = false;

        //    EpisodeManager.Instance.NextEpisode();
        //}
    }

    public void StopAllActions()
    {
        videoPlayer.Stop();
        StopAllCoroutines();
    }

    public bool PauseVideo()
    {
        isPaused = isPaused ? false : true;

        if (isPaused) { videoPlayer.Pause(); }
        if (!isPaused) { videoPlayer.Play(); }

        return isPaused;
    }

    public void LoadVideo(string _url, int _index)
    {
        Debug.Log($"Playing {_url}");
        videoPlayer.url = _url;

        foreach (TextMeshProUGUI errorText in errorCodes) //pre sets error texts
        {
            errorText.text = "ERV_" + _index;
        }

        CheckConnection();
    }

    public void CheckConnection()
    {
        StartCoroutine(StartPing("8.8.8.8"));
        connectionErrorPannel.SetActive(false);
    }

    IEnumerator StartPing(string ip)
    {
        float timer = 0;
        Ping pingAdress = new Ping(ip);
        while (pingAdress.isDone == false && timer<5)
        {
            //max 5 seconden
            yield return new WaitForSeconds(0.1f);
            timer += 10 * Time.deltaTime;
            Debug.Log(timer);
        }
        if (pingAdress.isDone == true)
        {
            StartCoroutine(FollowUp());
        } 
        else
        {
            //log screen, check connection.
            Debug.Log("no connection");
            connectionErrorPannel.SetActive(true);
        }

    }

    IEnumerator FollowUp()
    {
        // stuff when the Ping has finshed....
        Debug.Log("Ik Doe het");
        //speelt video
        videoPlayer.Play();
        isChecking = true;

        yield return new WaitForSeconds(3);
        if(videoPlayer.isPlaying)
        {
            Debug.Log("video is playing");
        }
        else
        {
            Debug.Log("still loading");
            yield return new WaitForSeconds(5);
            if (videoPlayer.isPlaying)
            {
                Debug.Log("video is playing");
            }
            else
            {
                videoErrorPannel.SetActive(true);
            }
        }
    }
}
