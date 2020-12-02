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

    private float yCoord, zCoord;

    // LETS FIX IT DAMMIT
    Vector3 startPos; // previousPos
    Vector3 dist; // offSet

    // Mouse boundaries
    public MeshFilter BoundariesMeshIntro;
    public MeshFilter BoundariesMeshGame;
    private Vector3 minIntro, maxIntro, boundPosIntro, scaleIntro;
    private Vector3 minGame, maxGame, boundPosGame, scaleGame;

    public GameObject popParticle;

    public float newHeight;

    public void SetBounds()
    {
        // Introduction
        minIntro = BoundariesMeshIntro.mesh.bounds.min;
        maxIntro = BoundariesMeshIntro.mesh.bounds.max;
        boundPosIntro = BoundariesMeshIntro.transform.position;
        scaleIntro = BoundariesMeshIntro.transform.lossyScale;

        // Protect game
        minGame = BoundariesMeshGame.mesh.bounds.min;
        maxGame = BoundariesMeshGame.mesh.bounds.max;
        boundPosGame = BoundariesMeshGame.transform.position;
        scaleGame = BoundariesMeshGame.transform.lossyScale;
    }

    private void OnMouseDown()
    {
        if (!protectGame)
        {
            Debug.Log($"Position: {gameObject.transform.position} and camera: {Camera.main.WorldToScreenPoint(gameObject.transform.position)}");
            zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Debug.Log($"Mousepostion: {GetMouseWorldPos()}");
            offset = gameObject.transform.position - GetMouseWorldPos();
        }

        if (protectGame)
        {
            /*
            Debug.Log($"3D start position is {gameObject.transform.position}");
            startPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            Debug.Log($"Camera start position is {startPos}");
            Debug.Log($"Translating back to camera is {Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, startPos.y, Input.mousePosition.y))}");
            dist = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, startPos.y, Input.mousePosition.y));
            Debug.Log($"Travelled distance is {dist}"); */
           // Debug.Log($"Current position1: {gameObject.transform.position}");
            Debug.Log($"Position: {gameObject.transform.position} and camera: {Camera.main.WorldToScreenPoint(gameObject.transform.position)}");
            yCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).y; // TODO: Zonder "gameobject" ervoor proberen
           // Debug.Log($"Current position2: {gameObject.transform.position}");
            Debug.Log($"Mousepostion: {GetMouseWorldPos()}");
            offset = gameObject.transform.position - GetMouseWorldPos();
           // Debug.Log($"Current position3: {gameObject.transform.position}");
            /* Make pretty */
        }

        //Debug.Log("Offset: " + offset);
    }

    private Vector3 GetMouseWorldPos()
    {
      //  Debug.Log($"Current position4: {gameObject.transform.position}");
        Vector3 mousePoint = Input.mousePosition;

        Vector3 mousePointTru = mousePoint;

        Debug.Log($"1 Onaangepaste muis is {mousePointTru}, aangepaste is {mousePoint}");
        // Debug.Log($"Current position5 {gameObject.transform.position}");
        Debug.Log("mousePoint: " + mousePoint);
        if (!protectGame) { mousePoint.z = zCoord;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

       
        //Debug.Log($"Current position6: {gameObject.transform.position}");
        if (protectGame)
        {

          //  Debug.Log($"Current position7: {gameObject.transform.position}");
            mousePoint.z = zCoord;
            Debug.Log($"2 Onaangepaste muis is {mousePointTru}, aangepaste is {mousePoint}");
            //mousePoint.z = mousePoint.y;
            //mousePoint.y = yCoord;
            //  Debug.Log($"Current position8: {gameObject.transform.position}");

        }
        Debug.Log("mousePoint after correction: " + mousePoint);
        Debug.Log("Mousepoint to world space: " + Camera.main.ScreenToWorldPoint(mousePoint));
        mousePointTru = Camera.main.ScreenToWorldPoint(mousePointTru);

        if (protectGame)
        {
           // Debug.Log($"Current position9: {gameObject.transform.position}");
            mousePoint = Camera.main.ScreenToWorldPoint(mousePoint);

            Debug.Log($"3 Onaangepaste muis is {mousePointTru}, aangepaste is {mousePoint}");
           // Debug.Log($"Current position10: {gameObject.transform.position}");
                     
           // mousePoint.z = mousePoint.y; // att 1
            Debug.Log($"4 Onaangepaste muis is {mousePointTru}, aangepaste is {mousePoint}");
            //  mousePoint.z = yCoord;

            // Debug.Log($"Current position11: {gameObject.transform.position}");
            mousePoint.y = yCoord; // att 1
            Debug.Log($"5 Onaangepaste muis is {mousePointTru}, aangepaste is {mousePoint}");
            //  mousePoint.y = newHeight;

            //  Debug.Log($"Current position12: {gameObject.transform.position}");
            Debug.Log("mousepoint after second correction: " + mousePoint);
            return mousePoint;
        }
        // Debug.Log($"Current position13: {gameObject.transform.position}");
        Debug.Log("Komt ie ooit hier?");
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        if (canDrag)
        {
            SetBounds();

          //  previousPos = transform.position; // WEG?!

            // Use xz-plane for the protect game
            if (protectGame)
            {
                Debug.Log($"OG position is {gameObject.transform.position}");
                Vector3 mousePos = GetMouseWorldPos() + offset;
                Debug.Log($"Mouse position is {GetMouseWorldPos()}, " +
                    $"Offset is {offset}," +
                    $"so mousePos to work with is {mousePos}");
                Debug.Log($"mousepos is {mousePos}");



                //Vector3 lastPos = new Vector3(Input.mousePosition.x, startPos.y, Input.mousePosition.y);
                //Debug.Log($"But we say the lastPos is {lastPos}");
                //Debug.Log($"Which is {Camera.main.ScreenToWorldPoint(lastPos)} in camera space");
                // transform.position = Camera.main.ScreenToWorldPoint(lastPos) + dist;
                transform.position = mousePos;
                Debug.Log($"Yielding the following position: {transform.position}"); 
                /* TODO 
                // TODO: Clamp OR use InputMousePosX ipv mousePos
                transform.position = new Vector3(mousePos.x,
                    yCoord,
                    mousePos.y); */

                //Plane plane = new Plane(Vector3.up, 0);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //float distance;
                //if (plane.Raycast(ray, out distance))
                //{
                //    transform.position = ray.GetPoint(distance);
                //}
                //Debug.Log($"Dragging in xz from mousepostion {mousePos}");
                //Debug.Log($"Clamp {mousePos.x} between {boundPosIntro.x + minIntro.x * scaleIntro.x} and {boundPosIntro.x + maxIntro.x * scaleIntro.x}");
                //Debug.Log($"Clamp {mousePos.y - 23} between {boundPosIntro.z + minIntro.z * scaleIntro.z} and {boundPosIntro.z + maxIntro.z * scaleIntro.z}");
                //    transform.position = new Vector3(
                //Mathf.Clamp(mousePos.x, boundPosIntro.x + minIntro.x * scaleIntro.x, boundPosIntro.x + maxIntro.x * scaleIntro.x),
                //newHeight,
                //Mathf.Clamp(mousePos.y - 23, boundPosIntro.z + minIntro.z * scaleIntro.z, boundPosIntro.z + maxIntro.z * scaleIntro.z));
            }

            // Use xy-plane for the intro
            else
            {
                Debug.Log($"OG position is {gameObject.transform.position}");
                Vector3 mousePos = GetMouseWorldPos() + offset;
                Debug.Log($"Mouse position is {GetMouseWorldPos()}, " +
                    $"Offset is {offset}," +
                    $"so mousePos to work with is {mousePos}");
                Debug.Log($"Dragging in xy from mousepostion {mousePos}");
                Debug.Log($"Clamp {mousePos.x} between {boundPosIntro.x + minIntro.x * scaleIntro.x} and {boundPosIntro.x + maxIntro.x * scaleIntro.x}");
                Debug.Log($"Clamp {mousePos.y} between {boundPosIntro.y + minIntro.y * scaleIntro.y} and {boundPosIntro.y + maxIntro.y * scaleIntro.y}");
                transform.position = new Vector3(
                    Mathf.Clamp(mousePos.x, boundPosIntro.x + minIntro.x * scaleIntro.x, boundPosIntro.x + maxIntro.x * scaleIntro.x),
                    Mathf.Clamp(mousePos.y, boundPosIntro.y + minIntro.y * scaleIntro.y, boundPosIntro.y + maxIntro.y * scaleIntro.y),
                  -3.785588f);
            }

            Debug.Log($"Yields final position {transform.position}");
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
              //  previousPos = transform.position;
               // transform.position = GetMouseWorldPos() + offset; // TRY WITH CAMERAMAINSCREENTOWORLDPOINT 
                // TODO ADD VELOCITY
              //  previousPos = ne
            }
        }


    }

    private void FixedUpdate()
    {
        //Vector3 min = BoundariesMeshIntro.mesh.bounds.min;
        //Vector3 max = BoundariesMeshIntro.mesh.bounds.max;
        //Vector3 boundPos = BoundariesMeshIntro.transform.position;
        //Vector3 scale = BoundariesMeshIntro.transform.lossyScale;
        // Keep track of the position
       // Debug.Log($"Current position: {gameObject.transform.position}");

        // Update the boundaries
        SetBounds();

        if (canDrag && !protectGame)
        {
            // Debug.Log("Velocity: " + velocityMulti + ", And other velocity?: " + velocity);
            transform.position += velocity.normalized * velocityMulti;
            velocityMulti = Mathf.Clamp(velocityMulti - Time.deltaTime * velocityDecay, 0, Mathf.Infinity);
        }

        // Debug
        //Debug.Log("BoundariesMesh: " + BoundariesMeshIntro.mesh.bounds);
        //Debug.Log("Min: " + minIntro);
        //Debug.Log("Max: " + maxIntro);
        //Debug.Log("Pos: " + boundPosIntro);
        //Debug.Log("Scale: " + scaleIntro);
        //Debug.Log("X clamp: " + (boundPosIntro.x + minIntro.x * scaleIntro.x, boundPosIntro.x + maxIntro.x * scaleIntro.x));
        //Debug.Log("Y clamp: " + (boundPosIntro.y + minIntro.y * scaleIntro.y, boundPosIntro.y + maxIntro.y * scaleIntro.y));
        //Debug.Log("Z clamp: " + (boundPosIntro.z + minIntro.z * scaleIntro.z, boundPosIntro.z + maxIntro.z * scaleIntro.z));

        // For the game, use bounding box for the game (xz-plane)
        if (protectGame)
        {
           // Debug.Log("(protect game) Position before: " + transform.position);
            //transform.position = new Vector3(
            //Mathf.Clamp(transform.position.x, boundPosIntro.x + minIntro.x * scaleIntro.x, boundPosIntro.x + maxIntro.x * scaleIntro.x),
            //newHeight,
            //Mathf.Clamp(transform.position.z, boundPosIntro.z + minIntro.z * scaleIntro.z, boundPosIntro.z + maxIntro.z * scaleIntro.z));
           // Debug.Log("(protect game) Postion after: " + transform.position);
        }

        // For the introduction, use bounding box for the intro (xy-plane)
        else
        {
          //  Debug.Log("(intro) Postion before: " + transform.position);
            transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, boundPosIntro.x + minIntro.x * scaleIntro.x, boundPosIntro.x + maxIntro.x * scaleIntro.x),
        Mathf.Clamp(transform.position.y, boundPosIntro.y + minIntro.y * scaleIntro.y, boundPosIntro.y + maxIntro.y * scaleIntro.y),
     -3.785588f);
          //  Debug.Log("(intro) Postion after: " + transform.position);
        }
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
            gameObject.GetComponentInParent<BellenMiniGame>().coinMuliplierInt -= 1;
            gameObject.GetComponentInParent<BellenMiniGame>().filledInBubbles -= 1;


        }



    }




}
