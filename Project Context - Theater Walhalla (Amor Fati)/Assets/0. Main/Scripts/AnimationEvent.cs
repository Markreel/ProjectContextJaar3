using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> onInvoke = new List<UnityEvent>();

    public void InvokeEvent(int _index)
    {
        onInvoke[_index].Invoke();
    }
}
