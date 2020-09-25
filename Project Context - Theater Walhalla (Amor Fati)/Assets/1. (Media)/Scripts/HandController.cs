using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private int score = 0;
    private Animator animator;

    private enum States { Like, Dislike }
    private States currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ToggleState();
        }
    }

    private void FixedUpdate()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 3f; // Set this to be the distance you want the object to be placed in front of the camera.
        transform.position = Camera.main.ScreenToWorldPoint(temp);
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

    private void ToggleState()
    {
        currentState = currentState == States.Dislike ? States.Like : States.Dislike;

    }

    public void Click()
    {
        animator.SetTrigger("Click");
    }

    public void AfterClick()
    {

    }
}
