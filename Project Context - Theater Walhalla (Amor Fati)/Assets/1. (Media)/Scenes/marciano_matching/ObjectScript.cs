using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    [SerializeField] Vector3 target;
    [SerializeField] Vector3 origin;
    [SerializeField] Vector3 offset;

    void Start(){
        origin = transform.position;
        target = transform.position + offset;
    }

    public void Selected(){
                Debug.Log(gameObject.name + " selected");

        transform.position = target;

    }

    public void Deselect(){
        Debug.Log(gameObject.name + " deselected");
        transform.position = origin;
    }
}
