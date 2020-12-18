using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;
using System;

namespace ShooterGame
{
    public class ShootingTarget : MonoBehaviour, IShootable, IPooledObject
    {
        public string Key { get; set; }
        [SerializeField] private GameObject heightAdjuster;
        [SerializeField] private float minHeightAdjustment;
        [SerializeField] private float maxHeightAdjustment;

        [Space]

        [SerializeField] private int scoreValue;
        [SerializeField] private AudioClip audioOnHit1;
        [SerializeField] private AudioClip audioOnHit2;

        private Animator animator;
        private RoundManager targetManager;
        private bool isShot = false;

        private Round round;
        private Track track;
        private ScoreManager scoreManager;

        private Action<string, GameObject> onDestruction;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
       
        private IEnumerator IEMove()
        {
            float _tick = 0f;
            while (_tick < 1f)
            {
                _tick += Time.deltaTime / track.HorizontalMovementDuration;
                float _evaluatedTick = track.HorizontalMovementCurve.Evaluate(_tick);

                Vector3 _pos = Vector3.Lerp(track.SpawnPoint.position, track.EndPoint.position, _evaluatedTick);
                _pos.y += track.HorizontalHeightCurve.Evaluate(_evaluatedTick) / 2f - 0.5f;
                transform.position = _pos;

                yield return null;
            }

            SelfDestruct();
            yield return null;
        }

        private IEnumerator IEAdjustHeight()
        {
            float _tick = 0f;

            while (_tick < 1f)
            {
                _tick += Time.deltaTime / track.VerticalMovementSpeed;
                float _evaluatedTick = track.VerticalMovementCurve.Evaluate(_tick);

                Vector3 _newPos = heightAdjuster.transform.localPosition;
                _newPos.y = Mathf.Lerp(minHeightAdjustment, maxHeightAdjustment, _evaluatedTick);

                heightAdjuster.transform.localPosition = _newPos;
                yield return null;
            }

            StartCoroutine(IEAdjustHeight());
            yield return null;
        }

        private IEnumerator IEAdjustRotation()
        {
            float _tick = 0f;

            while (_tick < 1f)
            {
                _tick += Time.deltaTime / 0.5f;

                Vector3 _newAngle = Vector3.Lerp(Vector3.back * 45, Vector3.forward * 45, _tick);

                transform.localEulerAngles = _newAngle;
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            _tick = 0;
            while (_tick < 1f)
            {
                _tick += Time.deltaTime / 0.5f;

                Vector3 _newAngle = Vector3.Lerp(Vector3.forward * 45, Vector3.back * 45, _tick);

                transform.localEulerAngles = _newAngle;
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            StartCoroutine(IEAdjustRotation());
            yield return null;
        }

        public void Init(Round _round, Track _track, ScoreManager _scoreManager)
        {
            round = _round;
            track = _track;
            scoreManager = _scoreManager;

            isShot = false;
            
            StartCoroutine(IEMove());
            StartCoroutine(IEAdjustHeight());
        }

        public void SelfDestruct()
        {
            onDestruction.Invoke(Key, gameObject);
        }

        public void OnShot(GameObject _shooter)
        {
            if (isShot) { return; }

            GameManager.Instance.AudioManager.SpawnAudioComponent(transform, audioOnHit1);
            GameManager.Instance.AudioManager.SpawnAudioComponent(transform, audioOnHit2);

            scoreManager.AddScore(scoreValue);
            scoreManager.AdvanceMultiplierProgression();

            animator.SetTrigger("FallOver");
            isShot = true;
        }

        public void OnObjectSpawn()
        {
            
        }

        public void OnObjectDespawn()
        {
            StopAllCoroutines();
        }

        public void SetUpOnDestruction(Action<string, GameObject> _action)
        {
            onDestruction += _action;
        }
    }
}
