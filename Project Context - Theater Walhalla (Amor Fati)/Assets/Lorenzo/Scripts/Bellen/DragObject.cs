using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 previousPos;
    private Vector3 velocity;
    [HideInInspector] public float velocityMulti;
    [Range(0, 1)]
    public float velocityDecay;

    public bool canDrag;

    private float zCoord;

    public MeshFilter BoundariesMesh;

    public GameObject popParticle; 

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
            Vector3 mousePos = GetMouseWorldPos() + offset;
            Vector3 min = BoundariesMesh.mesh.bounds.min;
            Vector3 max = BoundariesMesh.mesh.bounds.max;
            Vector3 boundPos = BoundariesMesh.transform.position;
            Vector3 scale = BoundariesMesh.transform.lossyScale;
            previousPos = transform.position;
            transform.position = new Vector3(
                Mathf.Clamp(mousePos.x, boundPos.x + min.x * scale.x, boundPos.x + max.x * scale.x),
                Mathf.Clamp(mousePos.y, boundPos.y + min.y * scale.y, boundPos.y + max.y * scale.y),
              -3.785588f);


        }
    }

    private void OnMouseUp()
    {
        if (canDrag)
        {
            previousPos = transform.position;
            transform.position = GetMouseWorldPos() + offset;
            velocity = transform.position - previousPos;
            velocityMulti = (velocity.magnitude / 10) + 0.05f;
        }


    }

    private void FixedUpdate()
    {
        if (canDrag)
        {
            transform.position += velocity.normalized * velocityMulti;
            velocityMulti = Mathf.Clamp(velocityMulti - Time.deltaTime * velocityDecay, 0, Mathf.Infinity);
        }

        
        Vector3 min = BoundariesMesh.mesh.bounds.min;
        Vector3 max = BoundariesMesh.mesh.bounds.max;
        Vector3 boundPos = BoundariesMesh.transform.position;
        Vector3 scale = BoundariesMesh.transform.lossyScale;
        transform.position = new Vector3(
    Mathf.Clamp(transform.position.x, boundPos.x + min.x * scale.x, boundPos.x + max.x * scale.x),
    Mathf.Clamp(transform.position.y, boundPos.y + min.y * scale.y, boundPos.y + max.y * scale.y),
 -3.785588f);



    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag != "FatiBellenMinigame")
        {
            velocity = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal);
        }
 


        if (collision.gameObject.tag == "FatiBellenMinigame")
        {
            Instantiate(popParticle, transform.position, transform.rotation);
            Destroy(gameObject);
           
        }



    }




}
