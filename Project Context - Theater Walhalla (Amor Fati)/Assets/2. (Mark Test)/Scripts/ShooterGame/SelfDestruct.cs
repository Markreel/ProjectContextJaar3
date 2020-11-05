using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private bool startOnAwake = true;
    [SerializeField] private float selfDestructTime = 3f;
    [SerializeField] private UnityEvent onDestruction;

    private void Awake()
    {
        if (startOnAwake) { StartSelfDestruction(); }
    }

    public void StartSelfDestruction()
    {
        StartCoroutine(IEStartSelfDestruction());
    }

    private IEnumerator IEStartSelfDestruction()
    {
        yield return new WaitForSeconds(selfDestructTime);

        onDestruction.Invoke();
        Destroy(gameObject);
        yield return null;
    }
}
