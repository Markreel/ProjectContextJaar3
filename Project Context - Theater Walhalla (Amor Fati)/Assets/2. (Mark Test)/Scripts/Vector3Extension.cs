using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 Vector3Random(Vector3 _a, Vector3 _b)
    {
        return new Vector3(Random.Range(_a.x, _b.x), Random.Range(_a.y, _b.y), Random.Range(_a.z, _b.z));
    }
}
