using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/BetterAvoidance")]
public class BetterAvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent _agent, List<Transform> _context, Flock _flock)
    {
        //if no neighbors, return no adjustments
        if (_context.Count == 0) { return _agent.transform.forward; }

        //Create average point
        Vector3 _avoidanceMove = Vector3.zero;
        int _nAvoid = 0;
        List<Transform> filteredContext = (Filter == null) ? _context : Filter.Filter(_agent, _context);
        foreach (Transform _item in filteredContext)
        {
            Vector3 closestPoint = _item.gameObject.GetComponent<Collider>().ClosestPoint(_agent.transform.position);
            if (Vector3.SqrMagnitude(closestPoint - _agent.transform.position) < _flock.SquareAvoidanceRadius)
            {
                _nAvoid++;
                _avoidanceMove += _agent.transform.position - closestPoint;
            }
        }

        if(_nAvoid > 0f) { _avoidanceMove /= _nAvoid; }

        return _avoidanceMove;
    }
}
