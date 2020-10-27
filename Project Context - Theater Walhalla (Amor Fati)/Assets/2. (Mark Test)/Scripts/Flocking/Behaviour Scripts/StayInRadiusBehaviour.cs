using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay in Radius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius = 15f;

    public override Vector3 CalculateMove(FlockAgent _agent, List<Transform> _context, Flock _flock)
    {
        Vector3 _centerOffset = center - _agent.transform.position;
        float _t = _centerOffset.magnitude / radius;

        if(_t < 0.9f) { return _agent.transform.forward; }

        return _centerOffset * _t * _t;
    }
}
