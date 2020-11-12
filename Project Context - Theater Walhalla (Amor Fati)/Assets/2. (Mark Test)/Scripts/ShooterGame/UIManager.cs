using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ShooterGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private Image multiplierProgressBar;
        [SerializeField] private GameObject roundEndedWindow;
        [SerializeField] private TextMeshProUGUI roundEndedScoreText;

        public void UpdateTimerVisual(int _timeInSeconds)
        {
            int _minutes = Mathf.Clamp((_timeInSeconds - _timeInSeconds % 60) / 60, 0, 60);
            int _seconds = Mathf.Clamp(_timeInSeconds - 60 * _minutes, 0, 60);

            Debug.Log($"TimeInSeconds: {_timeInSeconds} | Minutes: {_minutes} | Seconds{ _seconds}");

            timerText.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);
        }

        public void UpdateScoreVisuals(int _value)
        {
            scoreText.text = _value.ToString("0000000");
        }

        public void UpdateMultiplierVisuals(int _multiplier, float _progress)
        {
            multiplierText.text = _multiplier.ToString();
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

        public void OpenRoundEndedWindow()
        {
            roundEndedScoreText.text = scoreText.text;
            roundEndedWindow.SetActive(true);
        }

    }
}
