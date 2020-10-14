using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 previousPos;
    private Vector3 velocity;
    private float velocityMulti;
    [Range (0,1)]
    public float velocityDecay; 

    public bool canDrag;



    private float zCoord;
    private void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        if (canDrag)
        {
            previousPos = transform.position;
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private void OnMouseUp()
    {
        if (canDrag)
        {
            previousPos = transform.position;
            transform.position = GetMouseWorldPos() + offset;
            velocity = transform.position - previousPos;
            velocityMulti = velocity.magnitude / 10;
        }
        

    }

    private void Update()
    {
        if (canDrag)
        {
            transform.position += velocity.normalized * velocityMulti;
            velocityMulti = Mathf.Clamp(velocityMulti - Time.deltaTime * velocityDecay, 0, Mathf.Infinity);
        }
    }






}
