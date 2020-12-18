using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class menuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Sprites
    [SerializeField] public Sprite menu_idle;
    [SerializeField] public Sprite menu_active;
    [SerializeField] public Sprite afsluiten_idle;
    [SerializeField] public Sprite afsluiten_active;

    // Set the sprite that should be used, based on current state
    [HideInInspector] public Sprite idle;
    [HideInInspector] public Sprite active;

    public void OnEnable()
    {
        this.GetComponent<Image>().sprite = idle;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = active;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = idle;
    }
}
