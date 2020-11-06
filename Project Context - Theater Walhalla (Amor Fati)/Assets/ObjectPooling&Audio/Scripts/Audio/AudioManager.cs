using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    public class AudioManager : MonoBehaviour
    {
        private ObjectPool objectPool;

        public void OnStart(ObjectPool _op)
        {
            objectPool = _op;
        }

        public void SpawnAudioComponent(Transform _t, AudioClip _clip)
        {
            if(_clip == null) { return; }

            PooledObject _po = objectPool.SpawnFromPool("AC_SingleClipRandomPitch", _t.position, _t.eulerAngles);
            AudioComponent _ac = _po.GameObject.GetComponent<AudioComponent>();

            List<AudioClip> _clips = new List<AudioClip>();
            _clips.Add(_clip);

            _ac.Reset();
            _ac.DisableAfterPlaying = true;
            _ac.AddAudioClipCollection("", _clips);
            _ac.Play();
        }

        public void SpawnAudioComponent(Transform _t, List<AudioClip> _clips)
        {
            PooledObject _po = objectPool.SpawnFromPool("AC_RandomClip", _t.position, _t.eulerAngles);
            AudioComponent _ac = _po.GameObject.GetComponent<AudioComponent>();

            _ac.Reset();
            _ac.DisableAfterPlaying = true;
            _ac.AddAudioClipCollection("", _clips);
            _ac.Play();
        }
    }
}
