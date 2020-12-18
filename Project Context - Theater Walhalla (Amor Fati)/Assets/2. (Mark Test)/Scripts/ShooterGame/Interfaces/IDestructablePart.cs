using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShooterGame
{
    public interface IDestructablePart
    {
        void Destruct(IDestructable _parent);
    }
}
