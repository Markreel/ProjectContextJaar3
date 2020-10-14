using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnapBubble : MonoBehaviour
{
    public bool containsBubble = false;

    private TextMeshPro sleepHierJeBubbelText;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        sleepHierJeBubbelText = GetComponentInChildren<TextMeshPro>();
        meshRenderer = GetComponent<MeshRenderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!containsBubble)
        {
            //So only one bubble can be added per snap zone. 
            containsBubble = true;
            //So we can no longer move the bubble.
            other.gameObject.GetComponent<DragObject>().canDrag = false;
            //Turn off the collider of the other object, unless you want the bell to majorly glitch out.
            other.gameObject.GetComponent<SphereCollider>().enabled = false; 
            //Snap the Users bubble to the snap bubble.
            LeanTween.move(other.gameObject, transform.position, 0.5f);
            //We do not want the sleep hier je bubble text to also be displayed through your own input.
            sleepHierJeBubbelText.enabled = false;
            //To avoid Z fighting of the material.
            meshRenderer.enabled = false; 
        }
        
    }
}
