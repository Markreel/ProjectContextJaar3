using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class downloadButton : ButtonFunctionality
{
    [SerializeField] Slider slider;
    private void OnMouseDown()
    {
        base.exportScherm.SetActive(true);
        slider.value = 0;
    }
}
