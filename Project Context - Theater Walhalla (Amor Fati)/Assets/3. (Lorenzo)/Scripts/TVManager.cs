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

    }


    public void TurnOn()
    {
        videoPlayers[currentTV].Play();
        currentTV++;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
