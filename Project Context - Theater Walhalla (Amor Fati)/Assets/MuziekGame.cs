﻿using ShooterGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MuziekGame : MonoBehaviour
{
    #region Unity fields
    [Header("UI Buttons")]
    [SerializeField] Button voorbeeldButton;
    [SerializeField] Button startButton;
    [SerializeField] Button downloadButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] menuButton menuButton;
    [SerializeField] Button playButton;
    [SerializeField] Button startKaraokeButton;
    [SerializeField] Button playBackButton;
    [SerializeField] Button terugButton;
    [SerializeField] Sprite play;
    [SerializeField] GameObject postRecording;

    [Header("Tekst")]
    [SerializeField] GameObject karaokeTekst;
    [SerializeField] GameObject geenMicrofoon;

    [Header("General")]
    [SerializeField] RecordingsHandler recordingsHandler;
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip tutorial;
    AudioSource audioSource;
    #endregion

    #region Private variables
    enum GameState { menu, recording, downloading };
    int currentState;

    #endregion

    #region Private methods

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentState = (int)GameState.menu;
        UpdateUI(currentState);
    }

    void UpdateUI(int state)
    {
        switch (state)
        {
            case (int)GameState.menu:
                // Stop audio
                if (audioSource.isPlaying) audioSource.Stop();

                // Active elements
                voorbeeldButton.gameObject.SetActive(true);
                startButton.transform.localScale = new Vector3(1, 1, 1);
                startButton.gameObject.SetActive(true);
                tutorialButton.gameObject.SetActive(true);
                menuButton.idle = menuButton.afsluiten_idle;
                menuButton.GetComponent<Image>().sprite = menuButton.idle;
                menuButton.active = menuButton.afsluiten_active;
                menuButton.gameObject.SetActive(true);

                // Deactivate elements
                startKaraokeButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                downloadButton.gameObject.SetActive(false);
                postRecording.SetActive(false);
                karaokeTekst.SetActive(false);
                recordingsHandler.metronoomAnimator.SetBool("IsPlaying", false);
                terugButton.gameObject.SetActive(false);
                geenMicrofoon.SetActive(false);

                // Turn off any playing audio
                if (recordingsHandler._audio.isPlaying) recordingsHandler._audio.Stop();
                if (recordingsHandler.source.isPlaying) recordingsHandler.source.Stop();

                break;

            case (int)GameState.recording:

                // Stop audio
                if (audioSource.isPlaying) audioSource.Stop();

                // Check if the game can start 
                bool microphonePresent = recordingsHandler.MicrophoneSetup();

                // If there is a microphone, load all elements
                if (microphonePresent)
                {
                    // Transition
                    StartCoroutine(TransitionToRecording());

                    // Deactive elements
                    startButton.gameObject.SetActive(false);
                    voorbeeldButton.gameObject.SetActive(false);
                    playButton.gameObject.SetActive(false);
                    downloadButton.gameObject.SetActive(false);
                    postRecording.SetActive(false);
                    geenMicrofoon.SetActive(false);

                    // Play welkomstwoordje als je hier voor t eerst bent
                    if (recordingsHandler.recording == null)
                    {
                        audioSource.clip = intro;
                        audioSource.Play();
                        terugButton.gameObject.SetActive(false);
                    }

                    // Terug naar downloaden als je voor de tweede keer iets aan het opnemen bent
                    else terugButton.gameObject.SetActive(true);

                    // Active elements
                    karaokeTekst.SetActive(true);
                    menuButton.idle = menuButton.menu_idle;
                    menuButton.GetComponent<Image>().sprite = menuButton.idle;
                    menuButton.active = menuButton.menu_active;
                }

                // Else, show warning that there is no mic
                else
                {
                    // Deactive elements
                    startButton.gameObject.SetActive(false);

                    // Activate elements
                    geenMicrofoon.SetActive(true);
                    menuButton.idle = menuButton.menu_idle;
                    menuButton.GetComponent<Image>().sprite = menuButton.idle;
                    menuButton.active = menuButton.menu_active;
                }

                break;


            case (int)GameState.downloading:

                // Stop audio
                if (audioSource.isPlaying) audioSource.Stop();

                // Deactivate elements
                startButton.gameObject.SetActive(false);
                voorbeeldButton.gameObject.SetActive(false);
                startKaraokeButton.gameObject.SetActive(false);
                karaokeTekst.SetActive(false);
                recordingsHandler.metronoomAnimator.SetBool("IsPlaying", false);
                terugButton.gameObject.SetActive(false);
                geenMicrofoon.SetActive(false);

                // Activate elements
                playBackButton.GetComponent<Image>().sprite = play;
                postRecording.SetActive(true);
                playButton.gameObject.SetActive(true);
                downloadButton.gameObject.SetActive(true);
                menuButton.idle = menuButton.menu_idle;
                menuButton.GetComponent<Image>().sprite = menuButton.idle;
                menuButton.active = menuButton.menu_active;
                break;
        }
    }

    IEnumerator TransitionToRecording()
    {
        // Make start button disappear
        LeanTween.scale(startButton.gameObject, new Vector3(0, 0, 0), 0.2f).setEaseInOutExpo();
        startKaraokeButton.gameObject.SetActive(true);
        startKaraokeButton.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(startKaraokeButton.gameObject, new Vector3(1, 1, 1), 0.2f).setEaseInExpo();
        yield break;
    }

    private void Update()
    {
        // Quit application
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    #endregion

    #region Public methods
    public void RecordingState()
    {
        currentState = (int)GameState.recording;
        UpdateUI(currentState);
    }

    public void DownloadState()
    {
        currentState = (int)GameState.downloading;
        UpdateUI(currentState);
    }
    
    public void Tutorial()
    {
        if (audioSource.isPlaying && audioSource.clip == intro)
        {
            audioSource.Stop();
            audioSource.clip = tutorial;
            audioSource.Play();
        }

        else if (audioSource.isPlaying && audioSource.clip == tutorial)
        {
            audioSource.Stop();
        }

        else if (!audioSource.isPlaying)
        {
            audioSource.clip = tutorial;
            audioSource.Play();
        }
    }

    public void StopIntroAudio()
    {
        if (audioSource.isPlaying) audioSource.Stop();
    }

    public void ReturnToMenu()
    {
        if (currentState != (int)GameState.menu)
        {
            // Stop the karaoke
            recordingsHandler.StopAllCoroutines();
            currentState = (int)GameState.menu;
            UpdateUI(currentState);
        }
       
        else if (currentState == (int)GameState.menu)
        {
            Application.Quit();
        }
    }

    #endregion
}
