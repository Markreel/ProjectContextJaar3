using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PoolingAndAudio
{
    public enum TimerCheckMethod { StopExistingTimer, WaitForExistingTimer }

    public class TimerHandler : MonoBehaviour
    {
        private Dictionary<string, Timer> TimerDictionary = new Dictionary<string, Timer>();

        public void OnUpdate()
        {
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            List<KeyValuePair<string, Timer>> _completedTimers = new List<KeyValuePair<string, Timer>>();
            foreach (KeyValuePair<string, Timer> _timer in TimerDictionary)
            {
                _timer.Value.TimePassed += Time.deltaTime;

                if (_timer.Value.TimePassed >= _timer.Value.Duration) { _completedTimers.Add(_timer); }
            }

            foreach (var _completedTimer in _completedTimers)
            {
                FinishTimer(_completedTimer);
            }
        }

        private void FinishTimer(KeyValuePair<string, Timer> _timer)
        {
            StopTimer(_timer.Key);
            _timer.Value.OnTimerFinished?.Invoke();
        }

        public void StartTimer(string _key, float _duration, UnityAction _onTimerFinished, TimerCheckMethod _checkMethod = TimerCheckMethod.StopExistingTimer)
        {
            Timer _timer = new Timer(_duration, _onTimerFinished);

            switch (_checkMethod)
            {
                default:
                case TimerCheckMethod.StopExistingTimer:
                    StopTimer(_key);
                    TimerDictionary.Add(_key, _timer);
                    break;
                case TimerCheckMethod.WaitForExistingTimer:
                    if (!TimerDictionary.ContainsKey(_key)) { TimerDictionary.Add(_key, _timer); }
                    break;
            }
        }

        public void StopTimer(string _key)
        {
            if (TimerDictionary.ContainsKey(_key)) { TimerDictionary.Remove(_key); }
        }
    }

    [System.Serializable]
    public class Timer
    {
        public Timer(float _duration, UnityAction _onTimerFinished)
        {
            Duration = _duration;
            OnTimerFinished = _onTimerFinished;
        }

        public float Duration;
        public float TimePassed;
        public UnityAction OnTimerFinished;
    }
}