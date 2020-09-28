using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    public static Debugger Instance;

    public TextMeshProUGUI DebugText;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayText(string _text)
    {
        DebugText.text = _text;
    }
}
