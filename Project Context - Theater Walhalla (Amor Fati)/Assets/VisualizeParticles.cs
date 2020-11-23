using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeParticles : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] int offset;
    [SerializeField] float multiplyer = 30;
    [SerializeField] float minimumSize = 0.25f;
    [SerializeField] float maxSize = .5f;
    [SerializeField] float speed = 20;
    [SerializeField] ParticleSystem particles;

    void Start()
    {
        //audiovisual = GameObject.Find("AudioVisualizer").GetComponent<AudioVisualizerScript>();
    }
    void Update()
    {
        target = Mathf.Lerp(target, Mathf.Clamp((audiovisual.spectrum[offset] * multiplyer + minimumSize), 0, maxSize), speed * Time.deltaTime);
        //Debug.Log(target);
        var emission = particles.emission;
        emission.rateOverTime = target;
    }
}
