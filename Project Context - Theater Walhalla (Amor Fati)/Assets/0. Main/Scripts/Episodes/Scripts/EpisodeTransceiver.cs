using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeTransceiver : MonoBehaviour
{
    public void NextEpisode()
    {
        EpisodeManager.Instance.NextEpisode();
    }

    public void LoadEpisode(int _index)
    {
        EpisodeManager.Instance.LoadEpisode(_index);
    }
}
