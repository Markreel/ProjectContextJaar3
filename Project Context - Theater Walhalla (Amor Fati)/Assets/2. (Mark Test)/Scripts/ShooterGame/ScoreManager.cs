using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private float multiplierDecayRate = 0.001f;
        [SerializeField] private float multiplierAdvanceRate = 0.2f;
        [SerializeField] private int multiplierMax;

        private int totalScore;
        private int currentScore;
        private int multiplier = 1;
        private float multiplierProgress;

        private UIManager uiManager;

        private void DecayMultiplierProgress()
        {
            if (multiplierProgress > 1)
            {
                multiplierProgress -= multiplierDecayRate * Time.fixedDeltaTime;
                
                if(multiplierProgress <= 0)
                {
                    if(multiplier > 1)
                    {
                        multiplier--;
                        multiplierProgress = 1;
                    }
                    else
                    {
                        multiplierProgress = 1;
                    }
                }

                uiManager.UpdateMultiplierVisuals(multiplier, multiplierProgress);
            }
        }

        public void OnStart(UIManager _uiManager)
        {
            uiManager = _uiManager;
        }

        public void OnUpdate()
        {
            DecayMultiplierProgress();
        }

        public void AddScore(int _score)
        {
            currentScore += _score * multiplier;
            uiManager.UpdateScoreVisuals(currentScore);
            DataCollectionManager.Instance.SetScore(currentScore);
        }

        public void AdvanceMultiplierProgression()
        {
            multiplierProgress += multiplierAdvanceRate;

            if (multiplierProgress >= 1)
            {
                if (multiplier < multiplierMax)
                {
                    multiplier++;
                    multiplierProgress = 0.25f;
                }
                else
                {
                    multiplierProgress = 1f;
                }
            }

            uiManager.UpdateMultiplierVisuals(multiplier, multiplierProgress);
        }
    }
}