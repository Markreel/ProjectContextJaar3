using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class ArchiveOutput : MonoBehaviour
{
    [SerializeField] GameObject wordblock;
    [SerializeField] Transform[] target;
    string requestedData;
    int index = 0;
    public string textToObject;

    void Start()
    {
        //StartCoroutine(OutputWord());
        StartCoroutine(getdata());
    }

     public IEnumerator getdata()
    {
        string uri = "https://marcianosordam.com/projects/context3/words.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Wacht tot er een response is 
            yield return webRequest.SendWebRequest(); 

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                requestedData = webRequest.downloadHandler.text;
                
                Debug.Log(requestedData);              
            }
        }

        if (requestedData == ""){
            Debug.Log("data is empty"); //if empty
        } else {
         ArchiveData _archiveData;
        _archiveData = JsonUtility.FromJson<ArchiveData>(requestedData);
        //Debug.Log(_archiveData.word_1);
        foreach (var item in _archiveData.GetFilteredStrings(_archiveData.words_from_archive))
        {

            Debug.Log(item);
            textToObject = item;


            Instantiate(wordblock,target[index]);
            index++;
            if (index>target.Length-1)
            {
                index = 0;
            }
            //Debug.Log("spawn");

            yield return new WaitForSeconds(2);
        }

        //string tempWord = JsonUtility.FromJson<ArchiveData>();
        }



    }
    
}
[System.Serializable] public class ArchiveData
{
    public string words_from_archive;

    public List<string> GetFilteredStrings(string _input)
    {
        List<string> filteredWords = new List<string>();
        string _filteredWord = "";
        char _previousChar = ' ';

        foreach (char _char in _input)
        {
            if (_previousChar == ',' && _char == ' ') { continue; }


            if (_char == ',')
            {
                filteredWords.Add(_filteredWord);
                _filteredWord = "";
            }else {
                _filteredWord += _char;
            }

            _previousChar = _char;
        }
        return filteredWords;
    }
    
}
