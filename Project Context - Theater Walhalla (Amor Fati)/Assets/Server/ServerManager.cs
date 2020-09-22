using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class ServerManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("pressed");
            StartCoroutine(GetRequest());
        }
    }

    IEnumerator GetRequest()
    {
        string uri = "https://www.slatecanvas.com/context3";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
