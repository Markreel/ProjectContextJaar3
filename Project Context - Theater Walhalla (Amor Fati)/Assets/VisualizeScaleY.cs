using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeScaleY : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] float multiplyer;
    [SerializeField] float minimumSize;
    void Update()
    {
        target = audiovisual.spectrum[0] * multiplyer + minimumSize;

        transform.localScale = new Vector3(transform.localScale.x,target,transform.localScale.z);
        
    }
}
