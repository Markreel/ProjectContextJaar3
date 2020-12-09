using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public TimerHandler TimerHandler;
        public AudioManager AudioManager;

        public ObjectPool objectPool;

        public System.Action OnUpdate;
        public System.Action OnFixedUpdate;

        protected virtual void Awake()
        {
            Instance = Instance ?? this;

            TimerHandler = GetComponentInChildren<TimerHandler>();
            AudioManager = GetComponentInChildren<AudioManager>();
            objectPool = GetComponentInChildren<ObjectPool>();
        }

        protected virtual void Start()
        {
            AudioManager.OnStart(objectPool);
            objectPool.OnStart(this);

            OnUpdate += TimerHandler.OnUpdate;
        }

        protected virtual void Update()
        {
            OnUpdate?.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
}
