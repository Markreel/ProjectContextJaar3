using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoEndDetectionTest : MonoBehaviour
{
    public VideoPlayer vp;
    [SerializeField] OnlineVideoManager videoManager;
    [SerializeField] bool done;

    private void Update()
    {
        if (!vp.isPlaying && vp.time > vp.length/10 && !done)
        {
            Debug.Log("I AM DONE");
            //follow up action
            done = true;

            EpisodeManager.Instance.NextEpisode();
        }
    }

    public void resetDone()
    {
        done = false;
    }
}
