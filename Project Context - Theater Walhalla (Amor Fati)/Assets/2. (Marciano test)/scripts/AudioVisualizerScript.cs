using System.Collections;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioVisualizerScript : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    public float[] spectrum = new float[128];

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
    }
}
