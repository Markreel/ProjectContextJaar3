using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;
using System;

namespace ShooterGame
{
    public class BaseSolid : MonoBehaviour, ISolid
    {
        [SerializeField] private AudioClip audioOnHit;

        public void OnShot()
        {
            GameManager.Instance.AudioManager.SpawnAudioComponent(transform, audioOnHit);
        }
    }
}
