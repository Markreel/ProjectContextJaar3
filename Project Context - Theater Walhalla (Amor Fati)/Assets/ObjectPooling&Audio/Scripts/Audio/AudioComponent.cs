using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioComponent : MonoBehaviour
    {
        public float Pitch { set { audioSource.pitch = value; } protected get { return audioSource.pitch; } }
        protected AudioSource audioSource;

        [Header("Main Settings: ")]
        [SerializeField] public bool DisableAfterPlaying = false;
        [SerializeField] protected bool playOnEnable = false;
        [Space]
        [SerializeField] protected bool loop;
        [SerializeField] protected float delayPerLoop;
        [Space]
        [SerializeField] protected bool randomPitch;
        [SerializeField] protected float randomPitchMin;
        [SerializeField] protected float randomPitchMax;
        [Space]
        [SerializeField] protected List<AudioClipCollection> clipCollections = new List<AudioClipCollection>();

        protected float randomPitchValue;

        protected virtual void Awake()
        {
            audioSource = audioSource ?? GetComponent<AudioSource>();
        }

        protected virtual void OnEnable()
        {
            GameManager.Instance.OnUpdate += OnUpdate;
            if (playOnEnable) { Play(); }
        }

        protected virtual void OnDisable()
        {
            GameManager.Instance.OnUpdate -= OnUpdate;
        }

        protected void SetRandomPitchValue()
        {
            randomPitchValue = Random.Range(randomPitchMin, randomPitchMax);
        }

        protected void Disable()
        {
            gameObject.SetActive(false);
        }

        public virtual void Play(int _index)
        {
            Play();
        }

        public virtual void Play()
        {
            //Stop behaviour if the GameObject isn't active
            if (!gameObject.activeInHierarchy) { return; }

            //Stop audio if already playing
            if (audioSource.isPlaying) { Stop(); }

            //Set random pitch
            if (randomPitch) { SetRandomPitchValue(); audioSource.pitch = randomPitchValue * Time.timeScale; }
            else { audioSource.pitch = Time.timeScale; }

            //Play audio
            audioSource.Play();

            //Start timer to disable after playing
            if (DisableAfterPlaying) { Invoke("Disable", audioSource.clip.length); }

            //Do loop
            if (loop) { GameManager.Instance.TimerHandler.StartTimer("AudioComponentPlay" + GetInstanceID(), audioSource.clip.length + delayPerLoop, Play); }

            //HOUD ER REKENING MEE DAT DE TIMER NU ALLEEN OP DE LENGTH VAN DE CLIP GAAT EN DIE LENGTE HOUD GEEN REKENING MET DE PITCH. IETS OM NAAR TE KIJKEN
        }

        public virtual void Pause()
        {
            audioSource.Pause();
        }

        public virtual void Stop()
        {
            audioSource.Stop();
        }

        public virtual void OnUpdate()
        {
            audioSource.pitch = randomPitch ? randomPitchValue * Time.timeScale : Time.timeScale;
        }

        public virtual void AddAudioClipCollection(string _name, List<AudioClip> _clips)
        {
            AudioClipCollection _acc = new AudioClipCollection(_name, _clips);
            clipCollections.Add(_acc);
        }

        public virtual void Reset()
        {
            DisableAfterPlaying = false;
            playOnEnable = false;

            loop = false;
            delayPerLoop = 0;

            randomPitch = false;
            randomPitchMin = 0;
            randomPitchMax = 0;
            randomPitchValue = 1;
            Pitch = 1;

            clipCollections.Clear();
        }
    }

    [System.Serializable]
    public class AudioClipCollection
    {
        public string Name = "New Collection";
        public List<AudioClip> AudioClips = new List<AudioClip>();

        public AudioClipCollection(string _name, List<AudioClip> _clips)
        {
            Name = _name;
            AudioClips = _clips;
        }
    }
}