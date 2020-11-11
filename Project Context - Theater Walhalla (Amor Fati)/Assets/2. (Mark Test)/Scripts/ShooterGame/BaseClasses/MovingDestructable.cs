using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class MovingDestructable : BaseDestructable, IPooledObject
    {
        [SerializeField] private bool useHeightAdjustment;
        [SerializeField] private GameObject heightAdjuster;
        [SerializeField] private float minHeightAdjustment;
        [SerializeField] private float maxHeightAdjustment;

        public string Key { get; set; }
        private System.Action<string, GameObject> onDestruction;

        private Round round;
        private Track track;

        protected override void SelfDestruct()
        {
            onDestruction.Invoke(Key, gameObject);
        }

        private IEnumerator IEMove()
        {
            float _tick = 0f;
            while (_tick < 1f)
            {
                _tick += Time.fixedDeltaTime / track.HorizontalMovementDuration;
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

        public void Init(Round _round, Track _track)
        {
            round = _round;
            track = _track;

            StartCoroutine(IEMove());
            if (useHeightAdjustment) { StartCoroutine(IEAdjustHeight()); }
        }

        public void OnObjectDespawn()
        {

        }

        public void OnObjectSpawn()
        {
            foreach (var _part in unactiveParts)
            {
                _part.Init();
                activeParts.Add(_part);
            }
            unactiveParts.Clear();
        }

        public void SetUpOnDestruction(System.Action<string, GameObject> _action)
        {
            onDestruction += _action;
        }
    }
}
