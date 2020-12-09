﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{


    public class Tutorial : MonoBehaviour
    {
        public int targetAmount;
        public int targetsHit;

        public AudioSource audioSource;

        public RoundManager roundManager;


        // Update is called once per frame
        void Update()
        {

            if (Input.GetMouseButton(0))
            {
                //yeah I hate this too, but layermasks suck. 
                int layer_mask = LayerMask.GetMask("TutorialTarget");

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
                {
                    Debug.Log("Hit");

                    //Makes sure we dont hit the same target multiple times.
                    hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                    //Counts up one target.
                    targetsHit++;
                    //Moves the target down.
                    LeanTween.moveLocalY(hit.transform.gameObject, -4, 1f).setEaseInExpo();

                    audioSource.Play();
                }
                //else
                //{
                //    Debug.Log("Miss");
                //}
            }


            if (targetsHit == targetAmount)
            {
                Debug.Log("HitAll");

                //This is really bad, but it makes it so this if statement is only called once as the targets hit is now over the targetamount. 
                targetsHit++;


                //Invoke the next round after a small delay. (After feedback that you did the right thing.
                Invoke("StartNextRound", 2f);


                roundManager.TutorialFinished(); 
                //Probably should put some sort of feedback here that you did the right thing. 

                //Like changing the top text to: Goed gedaan hoor!




            }
        }


        public void StartNextRound()
        {
            Debug.Log("NextRound");
        }

    }
}
