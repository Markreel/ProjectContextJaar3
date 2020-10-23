using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LookAtAxis { x, y, z}

public class LookAt : MonoBehaviour
{
    [SerializeField] private bool lookAtMainCamera = true;
    [SerializeField] private Transform lookAtTransform;
    [Space]
    [SerializeField] private LookAtAxis axis;
    [SerializeField] private bool inverted = false;
    [Space]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    //#TODO: Add option for local rotation

    private void Update()
    {
        if (lookAtMainCamera) { LookAtTransform(Camera.main.transform); }
        else if (lookAtTransform != null) { LookAtTransform(lookAtTransform); }
    }

    private void LookAtTransform(Transform _transform)
    {

        Vector3 _dir = (transform.position - _transform.position).normalized;
        if (lookAtMainCamera) { _dir = -Camera.main.transform.forward; }

        switch (axis)
        {
            case LookAtAxis.x:
                transform.right = inverted ? -_dir : _dir;
                break;

            case LookAtAxis.y:
                transform.up = inverted ? -_dir : _dir;
                break;

            default:
            case LookAtAxis.z:
                transform.forward = inverted ? -_dir : _dir;
                break;
        }

        Vector3 _eulerAngles = transform.eulerAngles;
        if (lockX) { _eulerAngles.x = 0; }
        if (lockY) { _eulerAngles.y = 0; }
        if (lockZ) { _eulerAngles.z = 0; }

        transform.eulerAngles = _eulerAngles;
    }

}
