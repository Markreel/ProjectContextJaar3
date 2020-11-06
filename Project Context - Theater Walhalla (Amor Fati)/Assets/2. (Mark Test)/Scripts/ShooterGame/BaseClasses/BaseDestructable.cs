using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class BaseDestructable : MonoBehaviour, IDestructable
    {
        [SerializeField] private List<BaseDestructablePart> parts = new List<BaseDestructablePart>();

        public void DestroyPart(BaseDestructablePart _targetedPart)
        {
            bool _isTargetPartInList = false;

            foreach (var _part in parts)
            {
                if (_part == _targetedPart) { _isTargetPartInList = true; break; }
            }
            if (_isTargetPartInList) { _targetedPart.Destruct(this); }

            parts.Remove(_targetedPart);

            if (parts.Count == 0) { Destroy(gameObject); }
        }
    }
}
