﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class ShooterController : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private LayerMask layerMask;
        [Space]
        [SerializeField] private float windUpDuration;
        [SerializeField] private float firingRate = 0.5f;
        [SerializeField] private float overheatThreshhold;

        private float currentWindUp;
        private float currentFiringRate;
        private float currentHeat;

        private ObjectPool objectPool;

        public void OnStart(ObjectPool _op)
        {
            objectPool = _op;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (currentFiringRate <= 0f) { Shoot(); }
            }

            if(currentFiringRate > 0f) { currentFiringRate -= Time.deltaTime; }
        }

        private void Shoot()
        {
            currentFiringRate = firingRate;

            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, layerMask.value))
            {
                PooledObject _po = objectPool.SpawnFromPool("Projectile", transform.position, Vector3.zero);
                _po?.GameObject.transform.LookAt(_hit.point);

                //GameObject _obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
                //_obj.transform.LookAt(_hit.point);
            }
        }
    }
}
