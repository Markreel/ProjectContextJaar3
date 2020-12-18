using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossfightMusicManager : MonoBehaviour
{
    [SerializeField] AudioSource start, fight, end, finish;
    bool started, ended;
    bool startTrack, hasEnded;
    [SerializeField] private float maxVolume = 0.3f;

    public void StartMusic()
    {
        Debug.Log("StartTrack");
        start.Play();
        startTrack = true;
    }

    void Update()
    {
        //sync fight player with end player
        end.timeSamples = fight.timeSamples;
        if (startTrack)
        {
            if (!started && !start.isPlaying)
            {
                fight.Play();
                end.Play();
                started = true;
            }
            else if (ended && !hasEnded)
            {
                fight.Pause();
                end.Pause();

                finish.Play();
                hasEnded = true;
            }

        }

    }

    public void StopAllMusic()
    {
        StopAllCoroutines();
        start.Stop();
        fight.Stop();
        end.Stop();
        finish.Stop();
    }

    public void StopMusic()
    {
        started = false;
        ended = true;
        Debug.Log("Stop Music");
    }

    public void SecondStage()
    {
        StartCoroutine(LerpEndVolume());
        Debug.Log("Second Stage");

    }

    IEnumerator LerpEndVolume()
    {
        float speed = 1;

        while (end.volume < 1)
        {
            end.volume += speed * Time.deltaTime;
            fight.volume -= speed * Time.deltaTime;

            end.volume = Mathf.Clamp(end.volume, 0, maxVolume);
            start.volume = Mathf.Clamp(start.volume, 0, maxVolume);

            yield return new WaitForEndOfFrame();
        }

        end.volume = maxVolume;
        fight.volume = 0;
    }
}


