using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolingAndAudio
{
    public class AC_SingleClip : AudioComponent
    {
        public override void Play()
        {
            audioSource.clip = clipCollections[0].AudioClips[0];
            base.Play();
        }

        public override void Play(int _index)
        {
            audioSource.clip = clipCollections[0].AudioClips[_index];
            base.Play();
        }
    }
}