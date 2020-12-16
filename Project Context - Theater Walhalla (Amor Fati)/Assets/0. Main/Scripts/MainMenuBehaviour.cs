using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private CanvasGroup sceneSelectWindow;

    public void StartGame()
    {
        EpisodeManager.Instance.LoadCurrentEpisode();
    }

    public void ToggleSceneSelectWindow(bool _value)
    {
        sceneSelectWindow.gameObject.SetActive(_value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
