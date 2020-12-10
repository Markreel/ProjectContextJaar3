using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playExample : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip banenliedTotaal;
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
        source.Play();
        this.GetComponent<Image>().sprite = stop;
    }

    void StopExample()
    {
        source.Pause();
        this.GetComponent<Image>().sprite = play;
    }
    #endregion

    #region Public methods
    public void ExampleButton()
    {
        source.clip = banenliedTotaal;
        if (source.isPlaying) StopExample();
        else PlayExample();
    }
    #endregion
}
