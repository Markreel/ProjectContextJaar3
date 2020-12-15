﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ShooterGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private RoundEndedWindow roundEndedWindow1;

        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private Image multiplierProgressBar;
        [SerializeField] private GameObject roundEndedWindow;
        [SerializeField] private TextMeshProUGUI roundEndedScoreText;
        [Space]
        [SerializeField] private GameObject gameOverWindow;


        [SerializeField] private RoundEndUI roundEndUIScript;

        [SerializeField] private GameObject HUD;




        public void UpdateTimerVisual(int _timeInSeconds)
        {
            int _minutes = Mathf.Clamp((_timeInSeconds - _timeInSeconds % 60) / 60, 0, 60);
            int _seconds = Mathf.Clamp(_timeInSeconds - 60 * _minutes, 0, 60);

            //Debug.Log($"TimeInSeconds: {_timeInSeconds} | Minutes: {_minutes} | Seconds{ _seconds}");

            //timerText.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);
            timerText.text = _timeInSeconds.ToString();
        }

        public void UpdateScoreVisuals(int _value)
        {
            scoreText.text = _value.ToString("0000000");
        }

        public void UpdateMultiplierVisuals(int _multiplier, float _progress)
        {
            multiplierText.text = "x" + _multiplier.ToString();
            multiplierProgressBar.fillAmount = _progress;
        }

        private IEnumerable IEAnimateText()
        {
            float _key = 0;

            while (_key < 1)
            {
                _key += Time.deltaTime;


            }

            yield return null;
        }

        public void OpenRoundEndedWindow(float _delay)
        {
            Invoke("OpenRoundEndedWindow", _delay);
        }

        public void OpenRoundEndedWindow()
        {
            roundEndedScoreText.text = scoreText.text;
            //roundEndedWindow.SetActive(true);
            roundEndUIScript.CloseCurtains();
            roundEndUIScript.raiseHud(); 
            //HUD.SetActive(false);
          
            
        }

        public void OpenGameOverWindow(float _delay)
        {
            Invoke("OpenGameOverWindow", _delay);
        }

        public void OpenGameOverWindow()
        {
            gameOverWindow.SetActive(true);
        }
    }

    [System.Serializable]
    public class RoundEndedWindow
    {
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI TargetsText;
        public Image FatiBonusImage;
        public Image GoldenBubbleBonusImage;
        public TextMeshProUGUI FinalScoreText;

        public IEnumerator IECalculateScore(RoundData _roundData)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = canvasGroup.blocksRaycasts = true;

            TargetsText.text = $"{_roundData.amountOfBubblesShot} / {_roundData.amountOfBubblesInScene}";
            FatiBonusImage.gameObject.SetActive(_roundData.FatiBonus);
            GoldenBubbleBonusImage.gameObject.SetActive(_roundData.GoldenBubbleBonus);

            FinalScoreText.text = $"{_roundData.amountOfBubblesShot}"; //werk dit uit met de fati en goudebel bonus

            yield return null;
        }
    }
}
