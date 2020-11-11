using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;


public class DataCollectionManager : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] float timer;
    [SerializeField] int totalShots;
    [SerializeField] int hitShots;
    [SerializeField] int breakables, targetshit, fatihit;
    [SerializeField] float mouseHoldTime;
    [SerializeField] float frameRate, maxFrameRate, minFrameRate;
    [SerializeField] float averageFPSCalculation, averageFPS;
    string deviceDataText;

    void Start()
    {
        deviceData();
        minFrameRate = Mathf.Infinity;
    }
    void FixedUpdate()
    {
        timer += 1 * Time.fixedDeltaTime;
        CheckFrameRate();
    }

    public void deviceData()
    {
        deviceDataText += " deviceModel: " + SystemInfo.deviceModel;
        deviceDataText += " graphicsDeviceName: " + SystemInfo.graphicsDeviceName;
        deviceDataText += " processor: " + SystemInfo.processorType;
        deviceDataText += " threads: " + SystemInfo.processorCount;
        
        //deviceDataText =deviceDataText.Replace(" ", "_");
        //deviceDataText = UrlEncode(deviceDataText, Encoding);
        deviceDataText = WebUtility.UrlEncode(deviceDataText);
        Debug.Log(deviceDataText);
    }

    void CheckFrameRate()
    {
        frameRate = 1.0f / Time.fixedDeltaTime;
        if (frameRate < minFrameRate)
        {
            minFrameRate = frameRate;
        }
        else if (frameRate > maxFrameRate
       )
        {
            maxFrameRate = frameRate;
        }
        averageFPSCalculation += ((Time.deltaTime / Time.timeScale) - averageFPSCalculation) * 0.03f;
        averageFPS = 1f / averageFPSCalculation;
    }

    public void ScoreUp(int x)
    {
        score += x;
    }

    public void TotalShots()
    {
        totalShots++;
    }

    public void HitShots()
    {
        hitShots++;
    }
    public void BreakablesHit()
    {
        breakables++;
    }
    public void TargetsHit()
    {
        targetshit++;
    }
    public void FatiHit()
    {
        fatihit++;
    }
    public void StartMouseTimer()
    {
        mouseHoldTime += 1 * Time.deltaTime;
    }

    public void PostData()
    {
        StartCoroutine(postData());
    }
    public IEnumerator postData()
    {
        string response; //response van de server
        string site = "https://marcianosordam.com/projects/context3/Shooter/postdata.php?"; // site waarmee gecomuniceert wordt

        string post = "score=" + score;
        post += "&timer=" + Mathf.Round(timer);
        post += "&totalShots=" + totalShots;
        post += "&hitShots=" + hitShots;
        post += "&breakables=" + breakables;
        post += "&targetshit=" + targetshit;
        post += "&fatihit=" + fatihit;
        post += "&mouseHoldTime=" + Mathf.Round(mouseHoldTime);
        post += "&maxFrameRate=" + Mathf.Round(maxFrameRate);
        post += "&minFrameRate=" + Mathf.Round(minFrameRate);
        post += "&averageFPS=" + Mathf.Round(averageFPS);
        post += "&deviceData=" + deviceDataText;

        //maakt de volledige link aan met post data
        string uri = site + post;
        Debug.Log(uri);

        //post data naar server
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Wacht tot er een response is 
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error); //error message
            }
            else
            {
                response = webRequest.downloadHandler.text;

                Debug.Log(response); //log response
            }
        }
    }
}
