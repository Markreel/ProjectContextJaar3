using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOnWater : MonoBehaviour
{
    [SerializeField] private float floatPadding = 1f;
    [SerializeField] private LayerMask layerMask;

    private void FixedUpdate()
    {
        RaycastHit _hit;

        if(Physics.Raycast(transform.position, Vector3.down, out _hit, floatPadding, layerMask.value))
        {
            transform.up = _hit.normal;
            transform.position = _hit.point;
        }
    }
}
