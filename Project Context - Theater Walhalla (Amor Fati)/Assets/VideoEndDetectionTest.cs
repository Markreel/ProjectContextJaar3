using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoEndDetectionTest : MonoBehaviour
{
    public VideoPlayer vp;
    [SerializeField] GameObject endScreen;

    private void Update()
    {
        if (!vp.isPlaying && vp.time > vp.length/10)
        {
            Debug.Log("I AM DONE");
            endScreen.SetActive(true);
        }
    }
}
