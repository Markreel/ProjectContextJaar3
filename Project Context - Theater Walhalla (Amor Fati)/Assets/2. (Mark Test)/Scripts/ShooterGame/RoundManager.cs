using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class RoundManager : MonoBehaviour
    {
        [Header("Settings per Round: ")]
        [SerializeField] public List<Round> Rounds = new List<Round>();
        [Space]
        [SerializeField] private Vector3 cameraStartPos;
        [SerializeField] private Vector3 cameraEndPos;

        [Header("Boss Fight")]
        [SerializeField] private FatiBoss fati;
        [SerializeField] private Weight weight;
        [SerializeField] private float bossFightDuration;

        [Header("References: ")]
        [SerializeField] private ShootingTarget shootingTargetPrefab;
        [SerializeField] private BaseDestructable destructablePrefab;

        private int roundIndex;
        private Coroutine roundRoutine;

        private Camera cam;
        private ObjectPool objectPool;
        private ScoreManager scoreManager;
        private UIManager uiManager;

        public void OnStart(ObjectPool _objectPool, ScoreManager _scoreManager, UIManager _uiManager)
        {
            objectPool = _objectPool;
            scoreManager = _scoreManager;
            uiManager = _uiManager;
            cam = Camera.main;

            fati?.Init(uiManager, this);

            
        }

        public void TutorialFinished()
        {
            roundRoutine = StartCoroutine(IEExecuteRoundBehaviour(Rounds[0]));
        }




        private void LerpCamera(float _t)
        {
            cam.gameObject.transform.position = Vector3.Lerp(cameraStartPos, cameraEndPos, _t);
        }

        private IEnumerator IEExecuteRoundBehaviour(Round _round)
        {
            //_round.RoundData.PopulateBubbleCountFromParent(_round.BubblesParent.transform);

            float _roundTime = _round.Duration;
            while (_roundTime > 0f)
            {
                _roundTime -= Time.deltaTime;
                uiManager.UpdateTimerVisual((int)_roundTime);

                LerpCamera(1f / _round.Duration * (-_roundTime + _round.Duration));

                yield return null;
            }

            fati.DoIntro(weight.DoIntro);

            _roundTime = bossFightDuration;
            while (_roundTime > 0f)
            {
                _roundTime -= Time.deltaTime;
                uiManager.UpdateTimerVisual((int)_roundTime);
                yield return null;
            }

            fati.DoAttack(uiManager.OpenGameOverWindow);

            //Time.timeScale = 0;
            //DataCollectionManager.Instance.PostData();
            //uiManager.OpenRoundEndedWindow();

            yield return null;
        }

        public void StopRound()
        {
            if(roundRoutine != null) { StopCoroutine(roundRoutine); }          
        }
    }

    [System.Serializable]
    public class Round
    {
        public float Duration;
        public GameObject EnvironmentParent;
        public GameObject BubblesParent;
        public RoundData RoundData;
    }

    [System.Serializable]
    public class RoundData
    {
        [HideInInspector] public bool GoldenBubbleBonus;
        [HideInInspector] public bool FatiBonus;
        [HideInInspector] public int amountOfBubblesInScene;
        [HideInInspector] public int amountOfBubblesShot;

        public void PopulateBubbleCountFromParent(Transform _parent)
        {
            amountOfBubblesInScene = _parent.GetComponentsInChildren<ShootingTargetV2>().Length - 1;
            Debug.Log(amountOfBubblesInScene);
        }
    }

    [System.Serializable]
    public class Track
    {
        [Header("Main Settings: ")]
        public string PooledObjectKey;
        public float SpawnDelay;
        [HideInInspector] public float SpawnTimer;
        public Transform SpawnPoint;
        public Transform EndPoint;
        [Header("Horizontal Movement: ")]
        public float HorizontalMovementDuration;
        public AnimationCurve HorizontalMovementCurve;
        public AnimationCurve HorizontalHeightCurve;
        [Header("Vertical Movement: ")]
        public float VerticalMovementSpeed;
        public AnimationCurve VerticalMovementCurve;

    }
}