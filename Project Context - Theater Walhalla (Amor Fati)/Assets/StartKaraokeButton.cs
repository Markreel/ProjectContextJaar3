using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class StartKaraokeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Image image;
    [SerializeField] public Sprite start_idle;
    [SerializeField] public Sprite start_active;
    [SerializeField] public Sprite stop_idle;
    [SerializeField] public Sprite stop_active;

    [HideInInspector] public Sprite idle;
    [HideInInspector] public Sprite active;

    public void OnEnable()
    {
        image.sprite = idle;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = active;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = idle;
    }
}
