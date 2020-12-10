using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.XR.WSA.Input;

public class StartButton : ButtonFunctionality
{
    [SerializeField] GameObject microphoneObject;
    [SerializeField] GameObject bord;
    [SerializeField] Slider progressBar;
    GameObject frontStart;
    GameObject buttonStart;
    Vector3 startBord;

    public override void Awake()
    {
        base.Awake();
        microphoneObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        frontStart = this.gameObject;
        buttonStart = frontStart.transform.parent.gameObject;
        startBord = bord.transform.position;
    }

    private void OnMouseDown()
    {
        base.microphoneControl.StartKaraoke();

        // Update UI
        bord.transform.position = new Vector3(startBord.x, startBord.y + 0.5f, startBord.z);
        bord.transform.rotation = new Quaternion(0, 0, 0, 0);
        buttonStart.SetActive(false);
        progressBar.gameObject.SetActive(true);
    }
} 
