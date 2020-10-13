using System.Collections;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioVisualizerScript : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] public float target;
    [SerializeField] private float multiplyer;
    [SerializeField] private float minimumSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float speed;

    public float[] spectrum = new float[512];

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        target = Mathf.Lerp(target, Mathf.Clamp((spectrum[0] * multiplyer + minimumSize),0,maxSize), speed*Time.deltaTime);
    }
}
