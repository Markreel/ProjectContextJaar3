using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")]
public class PhysicsLayerFilter : ContextFilter
{
    [SerializeField] private LayerMask mask;

    public override List<Transform> Filter(FlockAgent _agent, List<Transform> _original)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform _item in _original)
        {
            if (mask == (mask | (1 << _item.gameObject.layer)))
            {
                filtered.Add(_item);
            }
        }


        return filtered;
    }
}
