using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TV_Instructions : MonoBehaviour
{
    public GameObject title;
    public GameObject tutorial;

    [Header ("Title")]
    public Vector3 introStartingSize;
    public Vector3 introEndSize;
    public float introFadetime;

    [Header ("Tutorial")]
    public Vector3 tutorialStartingSize;
    public Vector3 tutorialEndSize;
    public float tutorialFadetime;


    // Start is called before the first frame update
    void Start()
    {
        title.transform.localScale = introStartingSize;
        tutorial.transform.localScale = tutorialStartingSize;
    }

    public void FadeInTitle()
    {
        LeanTween.scale(title, introEndSize, introFadetime).setEaseInCubic();
    }

    public void FadeOutTitle()
    {
        LeanTween.scale(title, introStartingSize, introFadetime).setEaseOutCubic();
    }

    public void FadeInInstructions()
    {
        LeanTween.scale(tutorial, tutorialEndSize, tutorialFadetime).setEaseInCubic();
    }

    public void FadeOutInstructions()
    {
        LeanTween.scale(tutorial, tutorialStartingSize, tutorialFadetime).setEaseOutCubic();
    }










}

