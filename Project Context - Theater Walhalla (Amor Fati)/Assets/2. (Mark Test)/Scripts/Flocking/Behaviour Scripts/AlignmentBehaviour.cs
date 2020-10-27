using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent _agent, List<Transform> _context, Flock _flock)
    {
        //if no neighbors, maintain current allignment
        if (_context.Count == 0) { return _agent.transform.forward; }

        //Create average point
        Vector3 _allignmentMove = Vector3.zero;
        List<Transform> filteredContext = (Filter == null) ? _context : Filter.Filter(_agent, _context);
        foreach (Transform _item in filteredContext)
        {
            _allignmentMove += _item.forward;
        }
        if (filteredContext.Count != 0) { _allignmentMove /= filteredContext.Count; }


        return _allignmentMove;
    }
}
