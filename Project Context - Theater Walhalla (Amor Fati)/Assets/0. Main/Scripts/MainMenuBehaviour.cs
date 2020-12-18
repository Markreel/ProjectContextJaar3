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

    public void HelheimMedia()
    {
        Application.OpenURL("http://helheimmedia.com/");
    }

    public void Extras()
    {
        Application.OpenURL("https://amorfatigame.nl/extra.html");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
