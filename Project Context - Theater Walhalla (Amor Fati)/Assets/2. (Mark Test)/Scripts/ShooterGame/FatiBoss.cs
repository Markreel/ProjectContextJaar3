using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PoolingAndAudio;

namespace ShooterGame
{
    public class FatiBoss : MonoBehaviour
    {
        public UnityAction OnIntroDone { get; private set; }
        public UnityAction OnAttackDone { get; private set; }

        [Header("Stun Effect: ")]
        [SerializeField] private GameObject stunEffect;
        [SerializeField] private GameObject bonk;
        [SerializeField] private AudioClip chirpClip;

        [Header("Intro: ")]
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 endPos;

        [SerializeField] private float introDuration;
        [SerializeField] private AnimationCurve introCurve;

        [Header("Attack: ")]
        [SerializeField] private float attackDuration;
        [SerializeField] private AnimationCurve attackCurve;

        private bool isIntroducing = false;
        private Coroutine currentRoutine;
        private UIManager uiManager;
        private RoundManager targetManager;


        [SerializeField] private AudioSource source;
        private int currentVolume = 1; 
       

        public void Init(UIManager _uiManager, RoundManager _targetManager)
        {
            uiManager = _uiManager;
            targetManager = _targetManager;

            

        }

        public void ResetValues()
        {
            isIntroducing = false;
            stunEffect.SetActive(false);
            transform.position = startPos;
            if (currentRoutine != null) { StopCoroutine(currentRoutine); }

        }

        public void GetHit()
        {
            targetManager.StopRound();

            stunEffect.SetActive(true);
            bonk.SetActive(true);
            bonk.GetComponent<Animator>().Play("Base Layer.BONK", 0, 0);
            GameManager.Instance.AudioManager.Play2DAudio(chirpClip);

            uiManager.OpenRoundEndedWindow(2);

            
            //LeanTween.value(gameObject, LowerVolume, currentVolume, 0, 3f);
            

        }


        void LowerVolume(float val)
        {
            source.volume = val;
        }





        public void DoIntro(UnityAction _onIntroDone = null)
        {
            OnIntroDone += _onIntroDone;
            if (currentRoutine != null) { StopCoroutine(currentRoutine); isIntroducing = false; }
            currentRoutine = StartCoroutine(IEIntro());

            source.Play();
        }

        public void DoAttack(UnityAction _onAttackDone = null)
        {
            OnAttackDone += _onAttackDone;
            if (currentRoutine != null) { StopCoroutine(currentRoutine); isIntroducing = false; }
            currentRoutine = StartCoroutine(IEAttack());
        }

        private IEnumerator IEIntro()
        {
            isIntroducing = true;

            float _key = 0;
            while (_key < 1f)
            {
                _key += Time.fixedDeltaTime / introDuration;
                float _evaluatedKey = introCurve.Evaluate(_key);

                transform.position = Vector3.Lerp(startPos, endPos, _evaluatedKey);

                yield return null;
            }

            isIntroducing = false;
            OnIntroDone?.Invoke();
            yield return null;
        }

        private IEnumerator IEAttack()
        {
            float _key = 0;
            while (_key < 1f)
            {
                _key += Time.fixedDeltaTime / attackDuration;
                float _evaluatedKey = attackCurve.Evaluate(_key);

                transform.position = Vector3.Lerp(endPos, Camera.main.transform.position, _evaluatedKey);

                yield return null;
            }

            OnAttackDone?.Invoke();
            yield return null;
        }

        private void OnTriggerEnter(Collider _other)
        {
            //Smash through destructable objects during the intro animation
            if (isIntroducing)
            {
                IDestructable _destructable = _other.GetComponentInParent<IDestructable>();
                BaseDestructablePart _destructionPart = _other.GetComponentInChildren<BaseDestructablePart>();
                if (_destructable != null && _destructionPart != null)
                {
                    _destructable.DestroyPart(_destructionPart);
                    DataCollectionManager.Instance.BreakablesHit();
                }
            }
        }

    }
}
