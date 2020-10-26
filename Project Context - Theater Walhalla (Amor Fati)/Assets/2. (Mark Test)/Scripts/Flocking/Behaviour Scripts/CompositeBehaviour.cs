using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Compostie")]
public class CompositeBehaviour : FlockBehaviour
{
    public FlockBehaviour[] Behaviours;
    public float[] Weights;

    public override Vector3 CalculateMove(FlockAgent _agent, List<Transform> _context, Flock _flock)
    {
        //Handle data mismatch
        if (Behaviours.Length != Weights.Length) { Debug.LogError($"Data mismatch in {name}", this); return Vector3.zero; }

        //Set up move
        Vector3 _move = Vector3.zero;

        //Iterate through behaviours
        for (int i = 0; i < Behaviours.Length; i++)
        {
            Vector3 _partialMove = Behaviours[i].CalculateMove(_agent, _context, _flock) * Weights[i];

            if(_partialMove != Vector3.zero)
            {
                if(_partialMove.sqrMagnitude > Weights[i] * Weights[i])
                {
                    _partialMove.Normalize();
                    _partialMove *= Weights[i];
                }

                _move += _partialMove;
            }
        }

        return _move;
    }
}
