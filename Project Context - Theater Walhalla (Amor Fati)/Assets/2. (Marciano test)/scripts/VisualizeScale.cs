using UnityEngine;
using System.Collections;

public class VisualizeScale : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] float multiplyer;
    [SerializeField] float minimumSize;
    [SerializeField] float maxSize;
    [SerializeField] float speed;
    [SerializeField] int offset = 0;
    Vector3 scaleOffset;
    [SerializeField] float morphSpeed = 2f;
    Vector3 sinX;
    Vector3 sinOffset;
    [SerializeField] float minScaleSize;

    void Start()
    {
        //audiovisual = GameObject.Find("AudioVisualizer").GetComponent<AudioVisualizerScript>();
        sinX = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
    }

    void Update()
    {
        //sin wave offset X
        sinX.x += 1 * Time.deltaTime * morphSpeed ;
        scaleOffset.x = ((0.5f * Mathf.Sin(sinX.x))+ .75f);

        //sin wave offset Y
        sinX.y += 1 * Time.deltaTime * morphSpeed ;
        scaleOffset.y = ((0.5f * Mathf.Sin(sinX.y))+ .75f);

        //sin wave offset Z
        sinX.z += 1 * Time.deltaTime * morphSpeed;
        scaleOffset.z = ((0.5f * Mathf.Sin(sinX.z))+ .75f);

        //Debug.Log(scaleOffset);



        target = Mathf.Lerp(target, Mathf.Clamp((audiovisual.spectrum[offset] * multiplyer + minimumSize),0,maxSize), speed*Time.deltaTime);

        //scaleOffset.x = Mathf.Round(scaleOffset.x * 10f) / 10f;
        //scaleOffset.y = Mathf.Round(scaleOffset.y * 10f) / 10f;
        //scaleOffset.z = Mathf.Round(scaleOffset.z * 10f) / 10f;

        

        transform.localScale = new Vector3(Mathf.Clamp((target* scaleOffset.x), minScaleSize, maxSize), Mathf.Clamp((target * scaleOffset.y), minScaleSize, maxSize), Mathf.Clamp((target * scaleOffset.z), minScaleSize, maxSize));      
    }

}
