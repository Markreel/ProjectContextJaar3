using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image audioImage;
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Slider slider;

    private RectTransform rectTransform;
    private Vector2 mousePos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Update()
    {
        if(slider.value == 0) { audioImage.sprite = mutedSprite; }
        else if(audioImage.sprite != unmutedSprite) { audioImage.sprite = unmutedSprite; }
    }

    public void ToggleMute()
    {
        if(slider.value > 0f) { slider.value = 0f; }
        else { slider.value = 0.75f; }
    }

    //private void OnMouseOver()
    //{
    //    rectTransform.sizeDelta = new Vector2(400, 100);
    //    slider.enabled = true;
    //}

    //private void OnMouseExit()
    //{
    //    rectTransform.sizeDelta = new Vector2(100, 100);
    //    slider.enabled = false;
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.sizeDelta = new Vector2(400, 100);
        slider.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.sizeDelta = new Vector2(100, 100);
        slider.gameObject.SetActive(false);
    }
}
