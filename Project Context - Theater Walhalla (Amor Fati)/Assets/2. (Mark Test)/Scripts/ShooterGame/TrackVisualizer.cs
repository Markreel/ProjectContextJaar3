using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackVisualizer : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (p1 != null && p2 != null) { Gizmos.DrawLine(p1.transform.position, p2.transform.position); }
    }
}
