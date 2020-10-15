using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatiFirstMinigame : MonoBehaviour
{
    public float attackTimer;
    public bool canAttack;

    public float minAttackDelay = 3;
    public float maxAttackDelay = 6;
    public float attackSpeedUp;

    public float fatiMoveSpeed = 3;

    public List<Transform> currentFatiPath;

    public GameObject fati;

    private LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void StartFirstAttack()
    {
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        //If we can attack start counting down the timer.
        if (canAttack)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer < 0)
            {
                attackTimer = 0;

                canAttack = false;
                Debug.Log("triggered");
                CalculateFatiPath();
            }
        }


    }

    public void CalculateFatiPath()
    {
        //Choose a random path game object. 
        Transform randomObject = gameObject.transform.GetChild(Random.Range(0, gameObject.transform.childCount));

        //Here we set take the chosen path parent and take the starting and endpoint transform of them.
        randomObject.GetComponentsInChildren(false, currentFatiPath);

        StartCoroutine(DrawLine());


    }


    public IEnumerator DrawLine()
    {
        lineRenderer.SetPosition(0, currentFatiPath[1].transform.position);
        lineRenderer.SetPosition(1, currentFatiPath[2].transform.position);
        yield return new WaitForSeconds(2f);
        Attack();
    }




    public void Attack()
    {
        //Moves Fati to the startingPosition. We use 1 here because the first object in the list is the path's parent.
        fati.transform.position = currentFatiPath[1].transform.position;
        //Starts moving Fati to the end Position.
        LeanTween.move(fati, currentFatiPath[2], fatiMoveSpeed);
        currentFatiPath.Clear();




        //At the of the attack reset the timer to a random number between the min and max attack delay. We substract this by an attackspeedup variable to increase Fati's attack speed on every attack.
        attackTimer = Random.Range(minAttackDelay - attackSpeedUp, maxAttackDelay - attackSpeedUp);
        if (attackTimer < minAttackDelay)
        {
            attackTimer = minAttackDelay;
        }
        fatiMoveSpeed = attackTimer -1.5f;
        attackSpeedUp += Random.Range(0.1f, 0.3f);
        canAttack = true;


    }


}
