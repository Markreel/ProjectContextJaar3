using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeScaleY : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] int offset;
    float rawOffset;
    float multiplyer = 30;
    float minimumSize = 0.25f;
    float maxSize = 1f;
    float speed = 20;

    float morphSpeed = 20;
    int maxOffset = 100;

    void Start(){
        audiovisual = GameObject.Find("AudioVisualizer").GetComponent<AudioVisualizerScript>();
        rawOffset = Random.Range(10, maxOffset);
    }
    void Update()
    {
        target = Mathf.Lerp(target, Mathf.Clamp((audiovisual.spectrum[offset] * multiplyer + minimumSize),0,maxSize), speed*Time.deltaTime);

        transform.localScale = new Vector3(transform.localScale.x,target,transform.localScale.z);
        //transform.localScale = new Vector3(target, target, target);

        //Debug.Log(target);

    }

    private void FixedUpdate()
    {
        rawOffset += morphSpeed * Time.fixedDeltaTime;
        float thisToInt = Mathf.Round(rawOffset);
        offset = (int) thisToInt;
        if (rawOffset > maxOffset)
        {
            rawOffset = 0;
        }
    }
}
