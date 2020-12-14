using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class playExample : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip banenliedTotaal;
    [SerializeField] Sprite play;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite stop;
    [SerializeField] Image image;
    [SerializeField] Sprite idle;
    [SerializeField] Sprite active;

    #region Private methods
    private void Start()
    {
        this.GetComponent<Image>().sprite = play;
    }

    public void OnEnable()
    {
        image.sprite = idle;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = active;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = idle;
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
