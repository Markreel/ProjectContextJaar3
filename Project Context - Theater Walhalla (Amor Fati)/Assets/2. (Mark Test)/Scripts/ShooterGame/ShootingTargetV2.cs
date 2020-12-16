using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class ShootingTargetV2 : MonoBehaviour, IShootable
    {
        [Header("Settings: ")]
        [SerializeField] private bool useFallAnimation = true;
        [SerializeField] private bool isLooping;
        [SerializeField] private GameObject heightAdjuster;
        [SerializeField] private float minHeightAdjustment;
        [SerializeField] private float maxHeightAdjustment;
        [SerializeField] private float heightAdjustmentDuration;
        [SerializeField] private AnimationCurve heightAdjustmentCurve;
        [Space]
        [SerializeField] private float activationRange = 5;
        [SerializeField] private float minActivationDelay;
        [SerializeField] private float maxActivationDelay;

        [Space]

        [SerializeField] private int scoreValue;
        [SerializeField] private AudioClip audioOnHit1;
        [SerializeField] private AudioClip audioOnHit2;

        [SerializeField] private List<AudioClip> amorClips;
        [SerializeField] private AudioClip goudenBelHitClip;
        [SerializeField] private AudioClip GoudenBelIntro;
        [SerializeField] private AudioClip AlexGoudenBelMis;


        [Space]

        [SerializeField] private bool isGoldenBubble = false;

        private Animator animator;
        private RoundManager targetManager;
        private bool isShot = false;
        private bool isActivated = false;

        private ScoreManager scoreManager;

        private NieuwScoreManager nieuwScoreManager;

        [SerializeField] public RoundEndUI roundEndUI;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            scoreManager = GameManager.Instance.gameObject.GetComponentInChildren<ScoreManager>();
            nieuwScoreManager = GameManager.Instance.gameObject.GetComponentInChildren<NieuwScoreManager>();
            SetStartingHeight();
        }

        private void Update()
        {
            if (!isActivated) { CheckForActivation(); }
        }

        private void CheckForActivation()
        {
            if (Camera.main.transform.position.x > transform.position.x - activationRange &&
                Camera.main.transform.position.x < transform.position.x + activationRange)
            {
                isActivated = true;
                Invoke("StartHeightAdjustment", Random.Range(minActivationDelay, maxActivationDelay));
                if (isGoldenBubble)
                {
                    GameManager.Instance.AudioManager.SpawnAudioComponent(transform, GoudenBelIntro);
                }
               
            }
        }






        private void SetStartingHeight()
        {
            float _evaluatedTick = heightAdjustmentCurve.Evaluate(0);
            Vector3 _startHeight = heightAdjuster.transform.localPosition;
            _startHeight.y = Mathf.Lerp(minHeightAdjustment, maxHeightAdjustment, _evaluatedTick);
            heightAdjuster.transform.localPosition = _startHeight;
        }

        private void StartHeightAdjustment()
        {
            if (!gameObject.activeInHierarchy) { return; }
            StartCoroutine(IEAdjustHeight());
        }

        private IEnumerator IEAdjustHeight()
        {
            float _tick = 0f;

            while (_tick < 1f)
            {
                _tick += Time.deltaTime / heightAdjustmentDuration;
                float _evaluatedTick = heightAdjustmentCurve.Evaluate(_tick);

                Vector3 _newPos = heightAdjuster.transform.localPosition;
                _newPos.y = Mathf.Lerp(minHeightAdjustment, maxHeightAdjustment, _evaluatedTick);

                heightAdjuster.transform.localPosition = _newPos;
                yield return null;
            }

            if (isLooping) { StartCoroutine(IEAdjustHeight()); }
            if (isGoldenBubble) { GameManager.Instance.AudioManager.SpawnAudioComponent(transform, AlexGoudenBelMis); }
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

        public void SelfDestruct()
        {
            gameObject.SetActive(false);
        }

        public void OnShot(GameObject _shooter)
        {
            if (isShot) { return; }

            nieuwScoreManager.bellenGeraakt++; 

            GameManager.Instance.AudioManager.SpawnAudioComponent(transform, audioOnHit1);
            GameManager.Instance.AudioManager.SpawnAudioComponent(transform, audioOnHit2);

            if (!isGoldenBubble)
            {



            }


            GameManager.Instance.objectPool.SpawnFromPool("CoinParticle", _shooter.transform.position, Vector3.left * 90);

            scoreManager.AddScore(scoreValue);
            scoreManager.AdvanceMultiplierProgression();

            isShot = true;

            if (isGoldenBubble)
            {

                Debug.Log("ik ben een goude bel");
                
               
                GameManager.Instance.AudioManager.SpawnAudioComponent(transform, goudenBelHitClip);
                nieuwScoreManager.goudenBellenGeraakt++;

                roundEndUI.StartCoroutine(roundEndUI.GoudenBelHUD_UI()); 



            }




            if (useFallAnimation) { animator.SetTrigger("FallOver"); }
            else { SelfDestruct(); }
        }
    }
}
