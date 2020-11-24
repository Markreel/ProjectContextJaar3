using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;
using System;

namespace ShooterGame
{
    public class BaseDestructable : MonoBehaviour, IDestructable
    {
        [SerializeField] protected List<BaseDestructablePart> activeParts = new List<BaseDestructablePart>();
        protected List<BaseDestructablePart> unactiveParts = new List<BaseDestructablePart>();

        protected virtual void SelfDestruct()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public virtual void DestroyPart(BaseDestructablePart _targetedPart)
        {
            bool _isTargetPartInList = false;
            foreach (var _part in activeParts)
            {
                if (_part == _targetedPart) { _isTargetPartInList = true; break; }
            }
            if (_isTargetPartInList) { _targetedPart.Destruct(this); }

            activeParts.Remove(_targetedPart);
            unactiveParts.Add(_targetedPart);

            if (activeParts.Count == 0) { SelfDestruct(); }
        }

    }
}
