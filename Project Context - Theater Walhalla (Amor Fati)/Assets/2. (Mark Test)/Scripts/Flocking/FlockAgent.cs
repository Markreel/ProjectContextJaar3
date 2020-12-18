using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public Flock AgentFlock { get; private set; }

    public Collider AgentCollider { get; private set; }

    private void Awake()
    {
        AgentCollider = GetComponent<Collider>();
    }
    
    public void Initialize(Flock _flock)
    {
        AgentFlock = _flock;
    }

    public void Move(Vector3 _velocity)
    {
        transform.forward = _velocity;
        transform.position += _velocity * Time.deltaTime;
    }


}
