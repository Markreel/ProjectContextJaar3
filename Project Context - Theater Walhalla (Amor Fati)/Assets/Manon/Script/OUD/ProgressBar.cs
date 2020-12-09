using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityWebGLMicrophone;

public class ProgressBar : MonoBehaviour
{
    #region Private variables
    // Slider
    AudioSource bullshitbanen;
    public Slider slider;
    public float targetProgress = 0;
    private float speed;
    [SerializeField] float FillSpeed = 0.5f;
    [SerializeField] MicrophoneControle microphoneControl;
    #endregion

    #region Private methods

    // TO DO:
    // - Met time samples ipv autodetection to make sure it records 6 clips (at the right time; recordings-size should then also be 6) (gaat mis aan begin)
    // - Clips bijknippen (alleen als er gepraat wordt; niet standaardtijd)
    // - Quitbutton
    // - Exporteren video: video en audio combineren (ffmpeg?)
    // - Design 
    // - (evt) Make sure recording has approx. same volume as song

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    #endregion

    #region Public methods
    // Add progress to the bar
    public IEnumerator IncrementProgress(float updateProgress)
    {
        // Advance slider
        if (slider.value < targetProgress)
        {
            slider.value = bullshitbanen.time / bullshitbanen.clip.length;
            yield return null;
        }
        yield return null;
    }
    #endregion
}
