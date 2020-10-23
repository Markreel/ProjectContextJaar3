using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeScaleY : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] int offset;
    [SerializeField] float multiplyer;
    [SerializeField] float minimumSize;
    [SerializeField] float maxSize;
    [SerializeField] float speed;

    void Start(){
        audiovisual = GameObject.Find("AudioVisualizer").GetComponent<AudioVisualizerScript>();
        offset = Random.Range(10,128);
    }
    void Update()
    {
        target = Mathf.Lerp(target, Mathf.Clamp((audiovisual.spectrum[offset] * multiplyer + minimumSize),0,maxSize), speed*Time.deltaTime);

        transform.localScale = new Vector3(transform.localScale.x,target,transform.localScale.z);
        Debug.Log(target);
    }
}
