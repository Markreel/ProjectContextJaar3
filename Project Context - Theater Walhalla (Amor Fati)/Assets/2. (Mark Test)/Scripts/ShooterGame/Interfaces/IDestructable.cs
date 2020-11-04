using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public interface IDestructable
    {
        void DestroyPart(BaseDestructablePart _targetedPart);
    }
}