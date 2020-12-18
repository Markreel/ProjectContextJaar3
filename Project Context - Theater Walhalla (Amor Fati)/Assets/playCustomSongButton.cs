using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class playCustomSongButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
    [SerializeField] Sprite idle;
    [SerializeField] Sprite active;

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
