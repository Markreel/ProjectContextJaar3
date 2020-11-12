using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PoolingAndAudio;
using System;

namespace ShooterGame
{
    public class Projectile : MonoBehaviour, IPooledObject
    {
        public string Key { get; set; }
        private Action<string, GameObject> onDestruction;

        [SerializeField] private float speedMulitplier = 10f;
        [SerializeField] private float lifeTime = 5f;

        private bool isMoving = true;
        private Vector3 previousPosition = Vector3.zero;

        private void FixedUpdate()
        {
            CheckCollision();            

            if (isMoving)
            {
                transform.position += transform.forward * Time.deltaTime * speedMulitplier;
            }
        }

        private void CheckCollision()
        {
            Vector3 _dir = transform.position - previousPosition;
            RaycastHit[] _hits = Physics.RaycastAll(transform.position, _dir, _dir.magnitude);

            foreach (var _hit in _hits)
            {
                IShootable _shootable = _hit.collider.GetComponentInParent<IShootable>();
                if (_shootable != null)
                {
                    _shootable.OnShot(gameObject);
                    DataCollectionManager.Instance.TargetsHit();
                }

                //Check for Destructable
                IDestructable _destructable = _hit.collider.GetComponentInParent<IDestructable>();
                BaseDestructablePart _destructionPart = _hit.collider.GetComponentInChildren<BaseDestructablePart>();
                if (_destructable != null && _destructionPart != null)
                {
                    _destructable.DestroyPart(_destructionPart);
                    DataCollectionManager.Instance.BreakablesHit();
                }

                //Check for Solid
                ISolid _solid = _hit.collider.GetComponent<ISolid>();
                if (_solid != null)
                {
                    _solid.OnShot();
                    transform.position = _hit.point;
                    isMoving = false;
                }
            }

            previousPosition = transform.position;
        }

        private void SelfDestruct()
        {
            onDestruction.Invoke(Key,gameObject);
        }

        public void OnObjectSpawn()
        {
            previousPosition = transform.position;
            isMoving = true;
            GameManager.Instance.TimerHandler.StartTimer($"ProjectileDestruction_{GetInstanceID()}", lifeTime, SelfDestruct);
        }

        public void OnObjectDespawn()
        {
            
        }

        public void SetUpOnDestruction(Action<string, GameObject> _action)
        {
            onDestruction += _action;
        }

    }
}