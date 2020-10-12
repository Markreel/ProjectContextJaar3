using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeLight : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] float multiplyer;
    [SerializeField] float minimumSize;
    [SerializeField] Light locallight;

    [SerializeField] float maxSize;
    void Start()
    {
        locallight = GetComponent<Light>();
    }

    void Update()
    {
        target = Mathf.Clamp((audiovisual.spectrum[0] * multiplyer + minimumSize),0,maxSize);

        locallight.intensity = target;
        
    }
}
