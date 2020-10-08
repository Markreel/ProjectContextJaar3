using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpObjectPos : LerpBaseClass
{
    [SerializeField] private Vector3 posA;
    [SerializeField] private Vector3 posB;

    public override void Lerp(float _t)
    {
        transform.position = Vector3.Lerp(posA, posB, _t);
    }
}
