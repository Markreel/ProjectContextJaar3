using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private int score = 0;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 3f; // Set this to be the distance you want the object to be placed in front of the camera.
        this.transform.position = Camera.main.ScreenToWorldPoint(temp);    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<MediaProjectile>() != null)
        {
            score++;
            Debug.Log("score: " + score);
            Destroy(other.gameObject);
        }
    }

    public void Click()
    {
        animator.SetTrigger("Click");
    }

    public void AfterClick()
    {

    }
}
