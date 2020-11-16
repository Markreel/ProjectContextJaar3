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

        private Collider col;
        private Transform parent;
        private Vector3 localStartPos;
        private Vector3 localStartScale;
        private Quaternion localStartRot;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();

            parent = transform.parent;
            localStartPos = transform.localPosition;
            localStartScale = transform.localScale;
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
            transform.localScale = localStartScale;
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
            transform.localScale /= 2;
            rb.AddForce((Random.insideUnitSphere + Vector3.up) * 200f);
            transform.LookAt(transform.position + Random.insideUnitSphere);

            GameManager.Instance.TimerHandler.StartTimer($"BaseDestructionPart Destroy {GetInstanceID()}", lifeTime, Deactivate);
        }

        public void SetActiveCollider(bool _value)
        {
            col.enabled = _value;
        }
    }
}
