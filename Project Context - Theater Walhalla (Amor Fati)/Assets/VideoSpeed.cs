using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VideoSpeed : MonoBehaviour
{
    [SerializeField] VideoPlayer videoplayer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            videoplayer.playbackSpeed += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            videoplayer.playbackSpeed -= 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            videoplayer.playbackSpeed = 1;
        }
    }
}
