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

        private Vector3 startPos;
        private Coroutine refreshRoutine;

        private void Awake()
        {
            startPos = transform.position;
        }

        protected override void SelfDestruct()
        {
            gameObject.SetActive(false);
            Refresh();
        }

        private void Refresh()
        {
            if(refreshRoutine != null) { StopCoroutine(refreshRoutine); }
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

        private IEnumerator IERefresh()
        {
            yield return new WaitForSeconds(refreshDelay);

            ResetParts();
            gameObject.SetActive(true);

            float _key = 0;
            while (_key < 1f)
            {
                _key += Time.fixedDeltaTime / refreshDuration;
                float _evaluatedKey = refreshCurve.Evaluate(_key);

                transform.position = Vector3.Lerp(startPos + refreshLocationOffset, startPos, _evaluatedKey);
            }

            yield return null;
        }
    }

}