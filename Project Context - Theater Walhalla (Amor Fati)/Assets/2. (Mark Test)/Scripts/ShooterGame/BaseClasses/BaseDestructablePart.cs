using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PoolingAndAudio;

namespace ShooterGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseDestructablePart : MonoBehaviour, IDestructablePart
    {
        [SerializeField] private List<BaseDestructablePart> linkedParts = new List<BaseDestructablePart>();
        [SerializeField] private AudioClip audioOnHit;
        [SerializeField] private float lifeTime = 3;

        private Transform parent;
        private Vector3 localStartPos;
        private Quaternion localStartRot;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            parent = transform.parent;
            localStartPos = transform.localPosition;
            localStartRot = transform.localRotation;
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
            rb.isKinematic = true;
        }

        public void Init()
        {
            GameManager.Instance.TimerHandler.StopTimer($"BaseDestructionPart Destroy {GetInstanceID()}");
            rb.isKinematic = true;
            transform.parent = parent;
            transform.localPosition = localStartPos;
            transform.localRotation = localStartRot;
            gameObject.SetActive(true);
        }

        public void Destruct(IDestructable _parent)
        {
            foreach (var _part in linkedParts)
            {
                _parent.DestroyPart(_part);
            }

            GameManager.Instance.AudioManager.SpawnAudioComponent(transform, audioOnHit);

            rb.isKinematic = false;
            transform.parent = transform.parent.parent;
            rb.AddForce((Random.insideUnitSphere + Vector3.up) * 200f);
            transform.LookAt(transform.position + Random.insideUnitSphere);

            GameManager.Instance.TimerHandler.StartTimer($"BaseDestructionPart Destroy {GetInstanceID()}", lifeTime, Deactivate);
        }
    }
}
