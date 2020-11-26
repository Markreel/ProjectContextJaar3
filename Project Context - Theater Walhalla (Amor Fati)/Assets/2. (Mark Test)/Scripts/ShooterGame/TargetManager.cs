using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class TargetManager : MonoBehaviour
    {
        [Header("Settings per Round: ")]
        [SerializeField] public List<Round> Rounds = new List<Round>();
        [Space]
        [SerializeField] private Vector3 cameraStartPos;
        [SerializeField] private Vector3 cameraEndPos;

        [Header("Boss Fight")]
        [SerializeField] private FatiBoss fati;
        [SerializeField] private Weight weight;
        [SerializeField] private float bossFightDuration;

        [Header("References: ")]
        [SerializeField] private ShootingTarget shootingTargetPrefab;
        [SerializeField] private BaseDestructable destructablePrefab;

        private int roundIndex;
        private Coroutine roundRoutine;

        private Camera cam;
        private ObjectPool objectPool;
        private ScoreManager scoreManager;
        private UIManager uiManager;

        public void OnStart(ObjectPool _objectPool, ScoreManager _scoreManager, UIManager _uiManager)
        {
            objectPool = _objectPool;
            scoreManager = _scoreManager;
            uiManager = _uiManager;
            cam = Camera.main;

            fati?.Init(uiManager, this);

            roundRoutine = StartCoroutine(IEExecuteRoundBehaviour(Rounds[0]));
        }

        private void LerpCamera(float _t)
        {
            cam.gameObject.transform.position = Vector3.Lerp(cameraStartPos, cameraEndPos, _t);
        }

        private IEnumerator IEExecuteRoundBehaviour(Round _round)
        {
            float _roundTime = _round.Duration;
            while (_roundTime > 0f)
            {
                _roundTime -= Time.deltaTime;
                uiManager.UpdateTimerVisual((int)_roundTime);

                LerpCamera(1f / _round.Duration * (-_roundTime + _round.Duration));

                foreach (var _track in _round.TargetTracks)
                {
                    if (_track.SpawnTimer > 0f)
                    {
                        _track.SpawnTimer -= Time.deltaTime;
                    }
                    else
                    {
                        _track.SpawnTimer = _track.SpawnDelay;
                        SpawnTarget(_round, _track);
                    }
                }

                foreach (var _track in _round.EnvironmentTracks)
                {
                    if (_track.SpawnTimer > 0f)
                    {
                        _track.SpawnTimer -= Time.deltaTime;
                    }
                    else
                    {
                        _track.SpawnTimer = _track.SpawnDelay;
                        SpawnEnvironmentPart(_round, _track, _track.PooledObjectKey);
                    }
                }

                foreach (var _track in _round.BackgroundTracks)
                {
                    if (_track.SpawnTimer > 0f)
                    {
                        _track.SpawnTimer -= Time.deltaTime;
                    }
                    else
                    {
                        _track.SpawnTimer = _track.SpawnDelay;
                        SpawnBackgroundPart(_round, _track, _track.PooledObjectKey);
                    }
                }

                yield return null;
            }

            fati.DoIntro(weight.DoIntro);

            _roundTime = bossFightDuration;
            while (_roundTime > 0f)
            {
                _roundTime -= Time.deltaTime;
                uiManager.UpdateTimerVisual((int)_roundTime);
                yield return null;
            }

            fati.DoAttack(uiManager.OpenRoundEndedWindow);

            //Time.timeScale = 0;
            //DataCollectionManager.Instance.PostData();
            //uiManager.OpenRoundEndedWindow();

            yield return null;
        }

        public void StopRound()
        {
            if(roundRoutine != null) { StopCoroutine(roundRoutine); }          
        }

        private void SpawnTarget(Round _round, Track _track)
        {
            PooledObject _po = objectPool.SpawnFromPool("Target", _track.SpawnPoint.position, Vector3.zero);
            _po.GameObject.GetComponent<ShootingTarget>().Init(_round, _track, scoreManager);
        }

        private void SpawnEnvironmentPart(Round _round, Track _track, string _key)
        {
            //PooledObject _po = objectPool.SpawnFromPool(_key, _track.SpawnPoint.position, Vector3.zero);
            //_po.GameObject.GetComponent<MovingDestructable>().Init(_round, _track);
        }

        private void SpawnBackgroundPart(Round _round, Track _track, string _key)
        {
            PooledObject _po = objectPool.SpawnFromPool(_key, _track.SpawnPoint.position, Vector3.zero);
            _po.GameObject.GetComponent<MovingSolid>().Init(_round, _track);
        }
    }

    [System.Serializable]
    public class Round
    {
        public float Duration;
        public float SpeedMultiplier;
        public List<Track> TargetTracks;
        public List<Track> EnvironmentTracks;
        public List<Track> BackgroundTracks;
    }

    [System.Serializable]
    public class Track
    {
        [Header("Main Settings: ")]
        public string PooledObjectKey;
        public float SpawnDelay;
        [HideInInspector] public float SpawnTimer;
        public Transform SpawnPoint;
        public Transform EndPoint;
        [Header("Horizontal Movement: ")]
        public float HorizontalMovementDuration;
        public AnimationCurve HorizontalMovementCurve;
        public AnimationCurve HorizontalHeightCurve;
        [Header("Vertical Movement: ")]
        public float VerticalMovementSpeed;
        public AnimationCurve VerticalMovementCurve;

    }
}