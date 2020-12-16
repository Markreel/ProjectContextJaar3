using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    public class AudioManager : MonoBehaviour
    {
        private ObjectPool objectPool;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }



        public void OnStart(ObjectPool _op)
        {
            objectPool = _op;
        }

        public void Play2DAudio(AudioClip _clip)
        {
            audioSource.PlayOneShot(_clip);
        }

        public void SpawnAudioComponent(Transform _t, AudioClip _clip)
        {
            if(_clip == null) { return; }

            PooledObject _po = objectPool.SpawnFromPool("AC_SingleClipRandomPitch", _t.position, _t.eulerAngles);
            AudioComponent _ac = _po.GameObject.GetComponent<AudioComponent>();

            List<AudioClip> _clips = new List<AudioClip>();
            _clips.Add(_clip);

            _ac.ResetValues();
            _ac.DisableAfterPlaying = true;
            _ac.AddAudioClipCollection("", _clips);
            _ac.Play();
        }

        public void SpawnAudioComponent(Transform _t, List<AudioClip> _clips)
        {
            PooledObject _po = objectPool.SpawnFromPool("AC_RandomClip", _t.position, _t.eulerAngles);
            AudioComponent _ac = _po.GameObject.GetComponent<AudioComponent>();

            _ac.ResetValues();
            _ac.DisableAfterPlaying = true;
            _ac.AddAudioClipCollection("", _clips);
            _ac.Play();
        }

    }
}
