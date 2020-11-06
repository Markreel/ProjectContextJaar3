using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    public class AC_RandomClip : AudioComponent
    {
        public override void Play()
        {
            audioSource.clip = clipCollections[0].AudioClips[Random.Range(0, clipCollections[0].AudioClips.Count)];
            base.Play();
        }

        public override void Play(int _index)
        {
            audioSource.clip = clipCollections[_index].AudioClips[Random.Range(0, clipCollections[_index].AudioClips.Count)];
            base.Play();
        }
    }
}