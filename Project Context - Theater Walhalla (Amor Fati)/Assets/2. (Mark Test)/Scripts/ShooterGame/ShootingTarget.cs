using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class ShootingTarget : MonoBehaviour, IShootable
    {
        [SerializeField] private GameObject heightAdjuster;
        [SerializeField] private float minHeightAdjustment;
        [SerializeField] private float maxHeightAdjustment;

        private Animator animator;
        private TargetManager targetManager;
        private bool isShot = false;

        private int index;
        private AnimationCurve horizontalMovementCurve;
        private AnimationCurve verticalMovementCurve;

        private Vector3 startPos;
        private Vector3 endPos;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(TargetManager _targetManager, int _index, Vector3 _startPos, Vector3 _endPos, AnimationCurve _horMoveCurve, AnimationCurve _verMoveCurve)
        {
            targetManager = _targetManager;
            index = _index;

            startPos = _startPos;
            endPos = _endPos;

            horizontalMovementCurve = _horMoveCurve;
            verticalMovementCurve = _verMoveCurve;

            StartCoroutine(IEMove());
            StartCoroutine(IEAdjustHeight());
            //StartCoroutine(IEAdjustRotation());
        }

        private IEnumerator IEMove()
        {
            float _tick = 0f;

            while (_tick < 1f)
            {
                _tick += Time.deltaTime / targetManager.GetHorizontalMovementSpeed(index);
                float _evaluatedTick = horizontalMovementCurve.Evaluate(_tick);

                Vector3 _pos = Vector3.Lerp(startPos, endPos, _evaluatedTick);
                _pos.y += (targetManager.GetHeightCurve(index, _evaluatedTick) / 2f) - 0.5f;
                transform.position = _pos;



                yield return null;
            }

            Destroy(gameObject);
            yield return null;
        }

        private IEnumerator IEAdjustHeight()
        {
            float _tick = 0f;

            while (_tick < 1f)
            {
                _tick += Time.deltaTime / targetManager.GetVerticalMovementSpeed(index);
                float _evaluatedTick = verticalMovementCurve.Evaluate(_tick);

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

        public void OnShot(GameObject _shooter)
        {
            if (isShot) { return; }
            animator.SetTrigger("FallOver");
            isShot = true;
        }
    }
}
