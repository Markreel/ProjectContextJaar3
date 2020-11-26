using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PoolingAndAudio;

namespace ShooterGame
{
    [RequireComponent(typeof(Animator))]
    public class Weight : MonoBehaviour, IShootable
    {
        public UnityAction OnIntroDone { get; private set; }
        public UnityAction OnFallDone { get; private set; }

        [SerializeField] private GameObject target;
        [SerializeField] private GameObject ropeTop;
        [SerializeField] private GameObject ropeBottom;

        [Header("Intro: ")]
        [SerializeField] private float offScreenYPos;
        [SerializeField] private float inScreenYPos;

        [SerializeField] private float introDuration;
        [SerializeField] private AnimationCurve introCurve;

        [Header("Audio: ")]
        [SerializeField] private AudioClip fallingClip;
        [SerializeField] private AudioClip hittingClip;

        private bool isShot = false;
        private Animator animator;
        private Rigidbody rb;
        private Vector3 startPos;

        private void Awake()
        {
            startPos = transform.position;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        public void ResetValues()
        {
            rb.isKinematic = true;
            target.SetActive(true);
            transform.position = startPos;
            animator.SetTrigger("Reset");
        }

        public void DoIntro()
        {
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

        private IEnumerator IEFall()
        {
            Vector3 _startPos = Vector3.zero;

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

            OnFallDone?.Invoke();
            yield return null;
        }

        private void OnTriggerEnter(Collider _other)
        {
            FatiBoss _fatiBoss = _other.GetComponent<FatiBoss>();
            if(_fatiBoss != null)
            {
                _fatiBoss.GetHit();
                GameManager.Instance.AudioManager.Play2DAudio(hittingClip);
                //spawn cartoony pow in fatiBoss.GetHit()
                ResetValues();
            }
        }
        //private void beweegHorizontaal

        public void OnShot(GameObject _shooter)
        {
            if (isShot) { return; }

            isShot = true;

            //stop horizontale beweging
            animator.SetTrigger("Fall");

            //rb.isKinematic = false;
            target.SetActive(false);
        }
    }
}
