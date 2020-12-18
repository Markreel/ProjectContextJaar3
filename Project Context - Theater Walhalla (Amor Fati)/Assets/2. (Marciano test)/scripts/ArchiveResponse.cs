using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using TMPro;

public class ArchiveResponse : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textinput;
    [SerializeField] TextMeshProUGUI responseText;
    // Start is called before the first frame update
    void Start()
    {
        textinput = GameObject.Find("InputText").GetComponent<TextMeshProUGUI>();
        responseText = GameObject.Find("ResponseText").GetComponent<TextMeshProUGUI>();
    }

    public void SendData(){
        Debug.Log("button clicked");
        StartCoroutine(postData());
    }
    public IEnumerator postData() 
    {
        string response; //response van de server
        string site = "https://www.marcianosordam.com/projects/context3/blacklistcheck.php?word="; // site waarmee gecomuniceert wordt


        //voegt de waardes toe aan de totale post, loopt door de array door.
        string post = textinput.text;
        
        //maakt de volledige link aan met post data
        string uri= site + post;
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
                
                responseText.text = response;
            }
        }
    }

}
