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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) { Time.timeScale -= Time.timeScale > 1 ? 1 : 0.1f; }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { Time.timeScale += Time.timeScale > 1 ? 1 : 0.1f; }
        if (Input.GetKeyDown(KeyCode.Space)) { Time.timeScale = 1; }
    }

    public void DisplayText(string _text)
    {
        DebugText.text = _text;
    }
}
