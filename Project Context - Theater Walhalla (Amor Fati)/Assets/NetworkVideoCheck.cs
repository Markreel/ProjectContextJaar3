using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class NetworkVideoCheck : MonoBehaviour
{

    [SerializeField] GameObject errorPannel;
    [SerializeField] GameObject videoErrorPannel;

    [SerializeField] VideoPlayer videoScreen;

    private void Start()
    {
        CheckConnection();    
    }

    public void CheckConnection()
    {
        StartCoroutine(StartPing("8.8.8.8"));
        errorPannel.SetActive(false);
    }

    public void DownloadGame()
    {
        Application.OpenURL("https://Marcianosordam.com");
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
            errorPannel.SetActive(true);
        }

    }


    IEnumerator FollowUp()
    {
        // stuff when the Ping p has finshed....
        Debug.Log("Ik Doe het");
        //speelt video
        videoScreen.Play();
        yield return new WaitForSeconds(3);
        if(videoScreen.isPlaying)
        {
            Debug.Log("video is playing");
        }
        else
        {
            Debug.Log("still loading");
            yield return new WaitForSeconds(5);
            if (videoScreen.isPlaying)
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
