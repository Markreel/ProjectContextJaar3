using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                GameObject _obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
                _obj.transform.LookAt(_hit.point);
            }
        }
    }
}
