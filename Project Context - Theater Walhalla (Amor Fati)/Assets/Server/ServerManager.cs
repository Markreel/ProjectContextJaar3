using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



public class ServerManager : MonoBehaviour
{   
    public string[] values; // lengte moet aangepast worden aan aantal posts;
    public string[] labels; // lengte moet aangepast worden aan aantal posts;

    void Start()
    {
        values = new string[] {"","","",}; // lengte van 3
        labels = new string[] {"","","",}; // lengte van 3
    }

    
    /*  In deze Enumerator posten we data naar een site.
        LET OP, dit post maar een ding per aanroeping, voor het efficient posten van meerdere data zullen er meerdere namen en values manual toegevoegd worden.
        In dit voorbeeld gaan we 3 waardes Posten. */

    void Update()
    {
        // voorbeeld case. wanneer spatie wordt ingedrukt
        if (Input.GetButtonDown("Jump")) 
        {     
            /*  Dit zijn 3 voorbeeld waardes en bijbehorende naam/label.
                Eerst moeten de labels en bijbehorende waardes ingevult worden, daarna pas kan de Enumerator "postData" aangeroepen worden. */


            //StartCoroutine(postData()); //start post en gaat de array af

            StartCoroutine(getdata());

            /* voor een vervolg iteratie. Array wordt automatisch langer voor elke waarde die wordt toegevoegd */
        }
    }


    

    public IEnumerator postData() 
    {
        string response; //response van de server
        string site = "https://www.marcianosordam.com/projects/context3/postdata.php?buffer=0"; // site waarmee gecomuniceert wordt


        //voegt de waardes toe aan de totale post, loopt door de array door.
        string post = "&MediaRonde1=01100101010110";
        //for (int i = 0 ; i<values.Length; i++){ 
        //    post += "&" + labels[i] + "=" + values[i];     
        //}
        
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
                
                Debug.Log(response); //log response
            }
        }
    }


    /*  Deze Enumerator checkt de text op een website. Deze waarde wordt ge'parse'd naar een float.
        De waarde die in floaat wordt gestopt wordt gelijk gemaakt aan de reqiestedData.
        Volgende stap zou een reference zijn om deze functie van elk script te kunnen roepen. */

    float Target; //voorbeeld float;
    public IEnumerator getdata()
    {
        string requestedData;
        string uri = "https://www.marcianosordam.com/projects/context3/getdata.php"; //site waarmee gecomuniceert wordt

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
                requestedData = webRequest.downloadHandler.text;
                
                Debug.Log(requestedData);              
            }
        }
    }

}
