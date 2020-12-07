using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playExample : MonoBehaviour
{
    [SerializeField] AudioSource example;
    [SerializeField] Sprite play;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite stop;

    #region Private methods
    private void Start()
    {
        this.GetComponent<Image>().sprite = play;
    }

    void PlayExample()
    {
        example.Play();
        this.GetComponent<Image>().sprite = stop;
    }

    void StopExample()
    {
        example.Pause();
        this.GetComponent<Image>().sprite = play;
    }
    #endregion

    #region Public methods
    public void ExampleButton()
    {
        if (example.isPlaying) StopExample();
        else PlayExample();
    }
    #endregion
}
