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

        [Header("References: ")]
        [SerializeField] private ShootingTarget shootingTargetPrefab;
        [SerializeField] private BaseDestructable destructablePrefab;

        private ObjectPool objectPool;

        public void OnStart(ObjectPool _objectPool)
        {
            objectPool = _objectPool;
            StartCoroutine(IEExecuteRoundBehaviour(Rounds[0]));
        }

        private IEnumerator IEExecuteRoundBehaviour(Round _round)
        {
            float _roundTime = _round.Duration;
            while (_roundTime > 0f)
            {
                _roundTime -= Time.deltaTime;
                //zet UI timer gelijk aan de _roundTimer waarde

                foreach (var _track in _round.TargetTracks)
                {
                    if(_track.SpawnTimer > 0f)
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
                        SpawnEnvironmentPart(_round,_track, _track.PooledObjectKey);
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

            Debug.Log("Round is over!");
            //Ronde is voorbij

            yield return null;
        }

        private void SpawnTarget(Round _round, Track _track)
        {
            PooledObject _po = objectPool.SpawnFromPool("Target", _track.SpawnPoint.position, Vector3.zero);
            _po.GameObject.GetComponent<ShootingTarget>().Init(_round, _track);
        }

        private void SpawnEnvironmentPart(Round _round, Track _track, string _key)
        {
            PooledObject _po = objectPool.SpawnFromPool(_key, _track.SpawnPoint.position, Vector3.zero);
            _po.GameObject.GetComponent<BaseDestructable>().Init(_round, _track);
        }

        private void SpawnBackgroundPart(Round _round, Track _track, string _key)
        {
            PooledObject _po = objectPool.SpawnFromPool(_key, _track.SpawnPoint.position, Vector3.zero);
            _po.GameObject.GetComponent<BaseSolid>().Init(_round, _track);
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