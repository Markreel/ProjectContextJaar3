using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speedMulitplier = 10f;

        private bool isMoving = true;

        private Vector3 previousPosition = Vector3.zero;

        private void Awake()
        {
            previousPosition = transform.position;
        }

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
                    //Debug.Log("I FOUND A SHOOTABLE");
                    _shootable.OnShot(gameObject);
                }

                //Check for Destructable
                IDestructable _destructable = _hit.collider.GetComponentInParent<IDestructable>();
                BaseDestructablePart _destructionPart = _hit.collider.GetComponentInChildren<BaseDestructablePart>();
                if (_destructable != null && _destructionPart != null)
                {
                    _destructable.DestroyPart(_destructionPart);
                }

                //Check for Solid
                ISolid _solid = _hit.collider.GetComponent<ISolid>();
                if (_solid != null)
                {
                    transform.position = _hit.point;
                    isMoving = false;
                }
            }

            previousPosition = transform.position;
        }

        private void OnCollisionEnter(Collision _collision)
        {
            ////Check for Shootable
            ////Debug.Log(_collision.collider.name);
            //IShootable _shootable = _collision.collider.GetComponentInParent<IShootable>();
            //if(_shootable != null)
            //{
            //    //Debug.Log("I FOUND A SHOOTABLE");
            //    _shootable.OnShot(gameObject);
            //}

            ////Check for Destructable
            //IDestructable _destructable = _collision.collider.GetComponentInParent<IDestructable>();
            //BaseDestructablePart _destructionPart = _collision.collider.GetComponentInChildren<BaseDestructablePart>();
            //if(_destructable != null && _destructionPart != null)
            //{
            //    _destructable.DestroyPart(_destructionPart);
            //}

            ////Check for Solid
            //ISolid _solid = _collision.collider.GetComponent<ISolid>();
            //if(_solid != null)
            //{
            //    transform.position = _collision.contacts[0].point;
            //    isMoving = false;
            //}
        }
    }
}