using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 previousPos;
    private Vector3 velocity;
    [HideInInspector] public float velocityMulti;
    [Range(0, 1)]
    public float velocityDecay;
    public bool canDrag, protectGame;
    private float zCoord;
    public float fixedY; // for the protectgame
    float fixedZ = -3.785588f; // for the intro

    // Mouse boundaries
    public MeshFilter BoundariesMesh;
    private Vector3 min, max, boundPos, scale;

    // Particles
    public GameObject popParticle;

    public void SetBounds()
    {
        min = BoundariesMesh.mesh.bounds.min;
        max = BoundariesMesh.mesh.bounds.max;
        boundPos = BoundariesMesh.transform.position;
        scale = BoundariesMesh.transform.lossyScale;
    }

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
            SetBounds();
            Vector3 mousePos = GetMouseWorldPos() + offset;

            // Use xz-plane for the protect game
            if (protectGame)
            {
                transform.position = new Vector3(
                    Mathf.Clamp(mousePos.x, boundPos.x + min.x * scale.x, boundPos.x + max.x * scale.x),
                    fixedY,
                    Mathf.Clamp(mousePos.z, boundPos.z + min.z * scale.z, boundPos.z + max.z * scale.z));
            }

            // Use xy-plane for the intro
            else if (!protectGame)
            {
                transform.position = new Vector3(
                    Mathf.Clamp(mousePos.x, boundPos.x + min.x * scale.x, boundPos.x + max.x * scale.x),
                    Mathf.Clamp(mousePos.y, boundPos.y + min.y * scale.y, boundPos.y + max.y * scale.y),
                    fixedZ);
            }
        }
    }

    private void OnMouseUp()
    {
        if (canDrag)
        {
            if (!protectGame)
            {
                previousPos = transform.position;
                transform.position = GetMouseWorldPos() + offset;
                velocity = transform.position - previousPos;
                velocityMulti = (velocity.magnitude / 10) + 0.05f;
            }
            if (protectGame)
            {
                previousPos = transform.position;
                transform.position = GetMouseWorldPos() + offset;
                velocity = transform.position - previousPos;
                velocityMulti = (velocity.magnitude / 10) + 0.05f;
            }
        }
    }

    private void FixedUpdate()
    {
        // Update the boundaries
        SetBounds();

        // Update position with velocity
        if (canDrag)
        {
            transform.position += velocity.normalized * velocityMulti;
            velocityMulti = Mathf.Clamp(velocityMulti - Time.deltaTime * velocityDecay, 0, Mathf.Infinity);
        }

        // For the game, keep bells within bounding box of xz-plane
        if (protectGame)
        {
            transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, boundPos.x + min.x * scale.x, boundPos.x + max.x * scale.x),
                    fixedY,
                    Mathf.Clamp(transform.position.z, boundPos.z + min.z * scale.z, boundPos.z + max.z * scale.z));
        }

        // For the introduction, keep bells within bounding box of xy-plane
        else if (!protectGame)
        {
            transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, boundPos.x + min.x * scale.x, boundPos.x + max.x * scale.x),
                    Mathf.Clamp(transform.position.y, boundPos.y + min.y * scale.y, boundPos.y + max.y * scale.y),
                    fixedZ);
        }
    }

    // Handle collisions
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag != "FatiBellenMinigame")
        {
            velocity = Vector3.Reflect(velocity.normalized, collision.contacts[0].normal);
        }

        if (collision.gameObject.tag == "FatiBellenMinigame")
        {
            Instantiate(popParticle, transform.position, transform.rotation);
            Destroy(gameObject);
            gameObject.GetComponentInParent<BellenMiniGame>().coinMuliplierInt -= 1;
            gameObject.GetComponentInParent<BellenMiniGame>().filledInBubbles -= 1;
        }
    }
}
