using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent _agent, List<Transform> _original)
    {
        List<Transform> filtered = new List<Transform>();

        foreach (Transform _item in _original)
        {
            FlockAgent _itemAgent = _item.GetComponent<FlockAgent>();
            if(_itemAgent != null && _itemAgent.AgentFlock == _agent.AgentFlock) { filtered.Add(_item); }
        }


        return filtered;
    }
}
