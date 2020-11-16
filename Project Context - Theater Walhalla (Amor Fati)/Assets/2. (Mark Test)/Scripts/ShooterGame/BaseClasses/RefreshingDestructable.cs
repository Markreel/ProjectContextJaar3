using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class RefreshingDestructable : BaseDestructable
    {
        [SerializeField] private float refreshDelay;
        [SerializeField] private Vector3 refreshLocationOffset = Vector3.down;
        [SerializeField] private AnimationCurve refreshCurve;
        [SerializeField] private float refreshDuration;

        private bool isRefreshing = false;
        private Vector3 startPos;
        private Coroutine refreshRoutine;

        private void Awake()
        {
            startPos = transform.position;
        }

        private void Refresh()
        {
            if (isRefreshing) { return; }
            isRefreshing = true;

            if (refreshRoutine != null) { StopCoroutine(refreshRoutine); }
            refreshRoutine = StartCoroutine(IERefresh());
        }

        private void ResetParts()
        {
            foreach (var _part in unactiveParts)
            {
                _part.Init();
                activeParts.Add(_part);
            }
            unactiveParts.Clear();
        }

        private void SetActiveColliderOnParts(bool _value)
        {
            foreach (var _part in activeParts)
            {
                _part.SetActiveCollider(_value);
            }
        }

        protected override void SelfDestruct()
        {
            
        }

        private IEnumerator IERefresh()
        {
            yield return new WaitForSeconds(refreshDelay);

            ResetParts();
            SetActiveColliderOnParts(false);

            float _key = 0;
            while (_key < 1f)
            {
                _key += Time.fixedDeltaTime / refreshDuration;
                float _evaluatedKey = refreshCurve.Evaluate(_key);

                transform.position = Vector3.Lerp(startPos + refreshLocationOffset, startPos, _evaluatedKey);
                yield return null;
            }

            SetActiveColliderOnParts(true);
            isRefreshing = false;
            yield return null;
        }

        public override void DestroyPart(BaseDestructablePart _targetedPart)
        {
            base.DestroyPart(_targetedPart);

            Refresh();
        }
    }

}