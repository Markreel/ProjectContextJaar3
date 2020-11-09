using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visualizeBubble : MonoBehaviour
{
    AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] float multiplier;
    [SerializeField] float minimumSize;
    [SerializeField] float speed;
    [SerializeField] float maxSize;

    [SerializeField] float downSpeed;

    float newMax;

    [SerializeField] Material _material;
    void Start()
    {
        audiovisual = GetComponent<AudioVisualizerScript>();
        
        _material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        target = Mathf.Lerp(target, Mathf.Clamp((audiovisual.spectrum[0] * multiplier + minimumSize),0,maxSize), speed*Time.deltaTime);

        if (target>newMax)
        {
            newMax = target;
        }

        _material.SetFloat("_Amount",newMax);

        newMax -= downSpeed*Time.deltaTime;
    }
}
