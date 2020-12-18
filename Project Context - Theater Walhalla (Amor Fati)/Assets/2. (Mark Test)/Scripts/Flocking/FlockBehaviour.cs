using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviour : ScriptableObject
{
    public abstract Vector3 CalculateMove(FlockAgent _agent, List<Transform> _context, Flock _flock);
}
