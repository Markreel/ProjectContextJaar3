using UnityEngine;

public class VisualizeScale : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] float multiplyer;
    [SerializeField] float minimumSize;

    [SerializeField] float maxSize;
    void Update()
    {
        target = Mathf.Clamp((audiovisual.spectrum[0] * multiplyer + minimumSize),0,maxSize);

        transform.localScale = new Vector3(target,target,target);
        
    }
}
