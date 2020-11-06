using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

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
