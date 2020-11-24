using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShooterGame
{
    [RequireComponent(typeof(Animator))]
    public class Weight : MonoBehaviour, IShootable
    {
        public UnityAction OnIntroDone { get; private set; }

        [SerializeField] private GameObject target;
        [SerializeField] private GameObject ropeTop;
        [SerializeField] private GameObject ropeBottom;

        [Header("Intro: ")]
        [SerializeField] private float offScreenYPos;
        [SerializeField] private float inScreenYPos;

        [SerializeField] private float introDuration;
        [SerializeField] private AnimationCurve introCurve;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void ResetValues()
        {
            target.SetActive(true);
            animator.SetTrigger("Reset");
        }

        public void DoIntro(UnityAction _onIntroDone = null)
        {
            OnIntroDone += _onIntroDone;
            animator.SetTrigger("Intro");
        }

        private IEnumerator IEIntro()
        {
            Vector3 _startPos = Vector3.zero;
            _startPos.z = transform.localPosition.z;
            _startPos.y = offScreenYPos;

            Vector3 _endPos = _startPos;
            _startPos.y = inScreenYPos;

            float _key = 0;
            while (_key < 1f)
            {
                _key += Time.fixedDeltaTime / introDuration;
                float _evaluatedKey = introCurve.Evaluate(_key);
            
                transform.position = Vector3.Lerp(_startPos, _endPos, _evaluatedKey);
                yield return null;
            }
            OnIntroDone?.Invoke();
            yield return null;
        }

        //private void beweegHorizontaal

        public void OnShot(GameObject _shooter)
        {
            //stop horizontale beweging
            animator.SetTrigger("Fall");
            target.SetActive(false);
        }
    }
}
