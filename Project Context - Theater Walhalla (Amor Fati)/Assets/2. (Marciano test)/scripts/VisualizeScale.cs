using UnityEngine;

public class VisualizeScale : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] float multiplyer;
    [SerializeField] float minimumSize;
    [SerializeField] float maxSize;
    [SerializeField] float speed;
    void Update()
    {
        target = Mathf.Lerp(target, Mathf.Clamp((audiovisual.spectrum[0] * multiplyer + minimumSize),0,maxSize), speed*Time.deltaTime);

        transform.localScale = new Vector3(target,target,target);
        
    }
}
