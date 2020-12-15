using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EpisodeManager : MonoBehaviour
{
    public static EpisodeManager Instance;

    [SerializeField] private List<Episode> Episodes;
    [SerializeField] private OnlineVideoManager onlineVideoManager;

    private int episodeIndex;

    public void Awake()
    {
        Instance = Instance ?? (this);
        if(Instance != this) { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    public void NextEpisode()
    {
        LoadEpisode(SaveFileManager.SaveFileData.EpisodeIndex);
        SaveFileManager.SaveFileData.EpisodeIndex++;
    }

    public void LoadEpisode(int _index)
    {
        //Check if the episode index is out of the episode list range
        if(Episodes.Count <= _index) {
            Debug.LogWarning($"Episode index ({_index}) is out of range."); return; }

        //Check if the episode with the given index is null
        if(Episodes[_index] == null) {
            Debug.LogWarning($"Episode with index of {_index} is null."); return; }

        //Store the episode in a local variable
        Episode _episode = Episodes[_index];

        //Check if the scene index of the episode is out of range
        if(SceneManager.sceneCountInBuildSettings <= _episode.SceneIndex) {
            Debug.LogWarning($"Scene index of {_episode.SceneIndex} is out of range."); return; }

        //Check if the scene we want to open is already opened (if not, we open the scene)
        if(SceneManager.GetActiveScene().buildIndex != _episode.SceneIndex)
        {
            SceneManager.LoadScene(_episode.SceneIndex);
        }

        //Check if the episode is a video episode (if so, load the correct video)
        if(_episode is VideoEpisode)
        {
            onlineVideoManager.LoadVideo(((VideoEpisode)_episode).VideoUrl, _index);
        }

        //Check if the episode is a game episode
        if(_episode is GameEpisode) { onlineVideoManager.StopAllActions(); }

    }
}