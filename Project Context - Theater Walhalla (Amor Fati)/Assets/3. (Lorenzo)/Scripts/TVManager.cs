using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVManager : MonoBehaviour
{
    public List<VideoPlayer> videoPlayers;
    private int currentTV; 



    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInChildren(true, videoPlayers);

        //The first item in the list is the static video player, we do not want to turn the static on but the next videoplayer instead.
        currentTV = 1;


    }


    public void TurnOn()
    {
        videoPlayers[currentTV].Play();
        //We need to add two because the Static video also counts as a video player.
        //This results in the hierachy being static - video - static - video.
        //Fix this later.
        currentTV += 2;
        Debug.Log(currentTV);

        //if (videoPlayers[currentTV].CompareTag("Video") == true)
        //{
        //    videoPlayers[currentTV].Play();
        //    currentTV ++;
        //}




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
