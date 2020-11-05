using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{

    public class TargetManager : MonoBehaviour
    {
        [Header("Settings: ")]
        [SerializeField] private Transform spawnPoint1;
        [SerializeField] private Transform endPoint1;
        [SerializeField] private Transform spawnPoint2;
        [SerializeField] private Transform endPoint2;
        [Space]
        [SerializeField] private AnimationCurve normalHorizontalMovementCurve;
        [SerializeField] private AnimationCurve horizontalHeightCurve1;
        [SerializeField] private float horizontalMovementSpeed1;
        [SerializeField] private AnimationCurve horizontalHeightCurve2;
        [SerializeField] private float horizontalMovementSpeed2;

        [Header("Verticles Movement: ")]
        [SerializeField] private float verticalMovementSpeed1;
        [SerializeField] private float verticalMovementSpeed2;
        [SerializeField] private AnimationCurve normalVerticalMovementCurve;
        [SerializeField] private AnimationCurve upDownVerticalMovementCurve;
        [SerializeField] private AnimationCurve crazyVerticalMovementCurve;
        [Space]
        [SerializeField] private float spawnDelay1;
        [SerializeField] private float spawnDelay2;

        [Header("References: ")]
        [SerializeField] private ShootingTarget shootingTargetPrefab;

        private void Start()
        {
            StartCoroutine(IESpawnTargets(1));
            StartCoroutine(IESpawnTargets(2));
        }

        public float GetHorizontalMovementSpeed(int _index)
        {
            switch (_index)
            {
                default:
                case 1:
                    return horizontalMovementSpeed1;
                case 2:
                    return horizontalMovementSpeed2;
            }
        }

        public float GetVerticalMovementSpeed(int _index)
        {
            switch (_index)
            {
                default:
                case 1:
                    return verticalMovementSpeed1;
                case 2:
                    return verticalMovementSpeed2;
            }
        }

        public float GetHeightCurve(int _index, float _t)
        {
            switch (_index)
            {
                default:
                case 1:
                    return horizontalHeightCurve1.Evaluate(_t);
                case 2:
                    return horizontalHeightCurve2.Evaluate(_t);
            }
        }

        private IEnumerator IESpawnTargets(int _index)
        {
            float _delay = 0f;
            AnimationCurve _curve;

            switch (_index)
            {
                default:
                case 1:
                    _delay = spawnDelay1;
                    _curve = normalVerticalMovementCurve;
                    break;
                case 2:
                    _delay = spawnDelay2;
                    _curve = upDownVerticalMovementCurve;
                    break;
            }

            yield return new WaitForSeconds(_delay);

            SpawnTarget(_index, normalHorizontalMovementCurve, _curve);

            StartCoroutine(IESpawnTargets(_index));
            yield return null;
        }

        private void SpawnTarget(int _index, AnimationCurve _horMoveCurve, AnimationCurve _verMoveCurve)
        {
            Vector3 _startPos;
            Vector3 _endPos;

            switch (_index)
            {
                default:
                case 1:
                    _startPos = spawnPoint1.position;
                    _endPos = endPoint1.position;
                    break;
                case 2:
                    _startPos = spawnPoint2.position;
                    _endPos = endPoint2.position;
                    break;
            }

            ShootingTarget _target = Instantiate(shootingTargetPrefab, _startPos, shootingTargetPrefab.transform.rotation, transform);

            _target.Init(this, _index, _startPos, _endPos, _horMoveCurve, _verMoveCurve);
        }

    }
}