﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class tutorialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite idle;
    [SerializeField] Sprite active;

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = active;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = idle;
    }
}