using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class OnlineVideoManager : MonoBehaviour
{

    [SerializeField] GameObject connectionErrorPannel;
    [SerializeField] GameObject videoErrorPannel;

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoEndDetectionTest videoEndDetection;

    [SerializeField] int scene = 0;

    [SerializeField] string siteLocation = "https://amorfatigame.nl/VideoLocatieSjqJ76fQkw6vCxSY/";

    [SerializeField] string filetype = ".mp4";

    [SerializeField] string[] videoNames;

    [SerializeField] TextMeshProUGUI[] errorCodes; //all errorcodes text elements


    private void Start()
    {
        NextVideo();
    }

    public void NextVideo()
    {
        //check if scene should go to next scene or to game
        if(true)
        {
            LoadVideo(scene);
            scene++;
        }
        else
        {
            //go to game
        }

    }

    public void LoadVideo(int i)
    {
        Debug.Log($"Playing {videoNames[i]}");
        videoPlayer.url = siteLocation + videoNames[i] + filetype;


        foreach (TextMeshProUGUI errorText in errorCodes) //pre sets error texts
        {
            errorText.text = "ERV_" + videoNames[i];
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
        videoEndDetection.resetDone();

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
