using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShooterGame
{
    [RequireComponent(typeof(Rigidbody), typeof(SelfDestruct))]
    public class BaseDestructablePart : MonoBehaviour, IDestructablePart
    {
        [SerializeField] private List<BaseDestructablePart> childParts = new List<BaseDestructablePart>();
        [SerializeField] private UnityEvent onDestruction;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Destruct(IDestructable _parent)
        {
            foreach (var _part in childParts)
            {
                _parent.DestroyPart(_part);
            }

            rb.isKinematic = false;
            transform.parent = transform.parent.parent;
            rb.AddForce((Random.insideUnitSphere + Vector3.up) * 200f);
            transform.LookAt(transform.position + Random.insideUnitSphere);
            onDestruction.Invoke();
        }
    }
}
