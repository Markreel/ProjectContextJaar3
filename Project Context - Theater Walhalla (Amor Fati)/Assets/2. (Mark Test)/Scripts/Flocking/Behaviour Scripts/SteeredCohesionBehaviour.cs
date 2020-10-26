using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Steered Cohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour
{
    [SerializeField] private float agentSmoothTime = 0.5f;
    private Vector3 currentVelocity;

    public override Vector3 CalculateMove(FlockAgent _agent, List<Transform> _context, Flock _flock)
    {
        //if no neighbors, return no adjustments
        if (_context.Count == 0) { return _agent.transform.forward; }

        //Create average point
        Vector3 _cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (Filter == null) ? _context : Filter.Filter(_agent, _context);
        foreach (Transform _item in filteredContext)
        {
            _cohesionMove += _item.position;
        }
        if (filteredContext.Count != 0) { _cohesionMove /= filteredContext.Count; }

        //Create offset from agent position
        _cohesionMove -= _agent.transform.position;
        if (float.IsNaN(currentVelocity.x) || float.IsNaN(currentVelocity.y) || float.IsNaN(currentVelocity.z)) { currentVelocity = Vector3.forward; } //Fix for NaN errors
        _cohesionMove = Vector3.SmoothDamp(_agent.transform.forward, _cohesionMove, ref currentVelocity, agentSmoothTime);

        return _cohesionMove;
    }
}
