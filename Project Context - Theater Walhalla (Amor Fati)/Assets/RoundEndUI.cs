using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ShooterGame
{
    public class RoundEndUI : MonoBehaviour
    {
        [Header("Curtains: ")]
        public Image leftCurtain;
        public Image rightCurtain;

        [Header("Bonuses: ")]
        public GameObject goudenBelBonusUIElement;
        public int goudenBelBonusPunten;
        public GameObject fatiBonusUIElement;
        public int fatiBonusPunten;

        [Space]

        public GameObject scoreBoardContainer;

        [Header("Bubbles hit UI: ")]
        public TextMeshProUGUI currentBubbleHitsText;
        public int currentBubbleHitsInt;
        public int bubblesHit;

        public TextMeshProUGUI maxBubbleCountText;
        public int maxBubbleCountInt;

        [Header("Endscreen UI: ")]
        public TextMeshProUGUI currentCoinsText;
        public int currentCoins;
        public int bubbleCoinsEarned;
        public int goudenBelCoinsEarned;
        public int fatiCoinsEarned;

        public GameObject hudUI;

        //public RoundManager roundManagerScript;

        [SerializeField] private ShooterController shooterControllerScript;

        [HideInInspector] public bool hasFinishedAllRounds;



        // Start is called before the first frame update
        void Start()
        {
            //Moves the scoreboard up so it can move down later.
            //Set object Y to 1050, which is just out of screen.
            //Why we do this? So we don't have to manually move it out of the screen everytime we are done with editing the scoreboard in Unity.
            scoreBoardContainer.transform.localPosition = new Vector3(0, 1050, 0);


            //We should set the coin text here to our coin score we got from the bubble game.


            //Set our Max Bubbles hit to the max hittable bubbles in the round amount. 
            maxBubbleCountText.text = maxBubbleCountInt.ToString();


            //Just for Testing, call this function from an other script to start the round end UI. 
            //CloseCurtains();



        }

        public void lowerHud()
        {
            LeanTween.moveLocalY(hudUI.gameObject, -25, 2).setEaseInBack();
        }

        public void raiseHud()
        {
            LeanTween.moveLocalY(hudUI.gameObject, 350, 2).setEaseInBack();
        }


        public void CloseCurtains()
        {
            //Left Curtain
            //Move X to 9
            LeanTween.moveLocalX(leftCurtain.gameObject, 11, 2).setEaseInBack();


            //Right Curtain
            //Move X to 6
            LeanTween.moveLocalX(rightCurtain.gameObject, -12, 2).setEaseInBack();

            //Calls the LowerScoreBoard function after the curtains are done with closing + a small delay.
            Invoke("LowerScoreBoard", 2.2f);

            shooterControllerScript.enabled = false;
        }


        public void LowerScoreBoard()
        {
            //Scoreboard get's lowered to 0y.
            LeanTween.moveLocalY(scoreBoardContainer, 0, 2).setEaseInExpo();


            Invoke("BubbleCount", 0.5f);
        }



        public void BubbleCount()
        {
            //Fake Count for example.
            //Connect this to the system that counts how many bubbles are hit.

            //Counts up our bubbleAmount.
            LeanTween.value(gameObject, StartCountingBubbles, currentBubbleHitsInt, bubblesHit, 8f).setEaseInExpo();

            //Calculate how much coins we should get. 100 is hardcoded but should be changed into the coin value.
            bubbleCoinsEarned = bubblesHit * 100 + currentCoins;
            LeanTween.value(gameObject, StartAddingCoins, currentCoins, bubbleCoinsEarned, 8f).setEaseInExpo();

            //When done with counting:
            Invoke("GoudenBelBonus", 9f);
        }

        //These functions are multiple times in the script. Required to start counting up.
        void StartCountingBubbles(float val)
        {
            currentBubbleHitsText.text = Mathf.Round(val).ToString();
        }
        void StartAddingCoins(float val)
        {

            currentCoinsText.text = Mathf.Round(val).ToString();
        }




        public void GoudenBelBonus()
        {

            //Check here if you got the gouden bel.

            //If No then do a stripethrough animation.
            //GameObject setactive stripethrough animation.

            //If Yes then:
            goudenBelBonusUIElement.transform.localScale = new Vector3(5, 5, 5);
            goudenBelBonusUIElement.gameObject.SetActive(true);
            //Scale the icon down into it's correct place.
            goudenBelBonusUIElement.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();




            //Should add in a small delay here, so it starts counting after the above animation is done.
            //Should probably rewrite this to an ienumator. 

            //Calculates how much coins we should add to the coins amount. This so we can tween towards it. 
            goudenBelCoinsEarned = currentCoins + goudenBelBonusPunten;
            LeanTween.value(gameObject, StartCountingGoudenBelBonus, currentCoins, goudenBelCoinsEarned, 1f).setEaseInExpo();


            //Start Fati Bonus Check.
            Invoke("FatiBonus", 1.5f);

        }

        void StartCountingGoudenBelBonus(float val)
        {
            currentCoinsText.text = Mathf.Round(val).ToString();
        }

        public void FatiBonus()
        {
            //Check here if you got the Fati Bonus.
            //If No then do a stripethrough animation.
            //GameObject setactive stripethrough animation.

            //If Yes then:

            fatiBonusUIElement.transform.localScale = new Vector3(5, 5, 5);
            fatiBonusUIElement.gameObject.SetActive(true);
            //Scale the icon down into it's correct place.
            fatiBonusUIElement.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();

            //Calculates how much coins we should add to the coins amount. This so we can tween towards it. 
            fatiCoinsEarned = currentCoins + fatiBonusPunten;
            LeanTween.value(gameObject, StartCountingGoudenBelBonus, currentCoins, fatiCoinsEarned, 1f).setEaseInExpo();


            Invoke("RaiseScoreBoard", 3f);
        }

        void StartCountingFatiBonus(float val)
        {
            currentCoinsText.text = Mathf.Round(val).ToString();
        }

        public void RaiseScoreBoard()
        {
            //Scoreboard get's Raised back up.
            LeanTween.moveLocalY(scoreBoardContainer, 1050, 2.5f).setEaseInExpo();


            //keep the curtains closed after the last round so we can transition from there with a cut or fade to the footage. 


            Invoke("OpenCurtains", 3.5f);

        }

        public void OpenCurtains()
        {
            if (!hasFinishedAllRounds)
            {
                //Left Curtain

                LeanTween.moveLocalX(leftCurtain.gameObject, -967, 2).setEaseInExpo();


                //Right Curtain

                LeanTween.moveLocalX(rightCurtain.gameObject, 978, 2).setEaseInExpo();

                //roundManagerScript.nextRound();

                lowerHud();

                shooterControllerScript.enabled = true;


            }

        }

        public void RoundCloseCurtainsTrigger()
        {
            Invoke("RoundCloseCurtains", 4f);

        }

        public void RoundCloseCurtains()
        {
            //Left Curtain
            LeanTween.moveLocalX(leftCurtain.gameObject, 11, 2).setEaseInExpo();
            //Right Curtain
            LeanTween.moveLocalX(rightCurtain.gameObject, -12, 2).setEaseInExpo();

            shooterControllerScript.enabled = false;

            Invoke("RoundOpenCurtains", 3f); 

        }

        public void RoundOpenCurtains()
        {
            //Left Curtain
            LeanTween.moveLocalX(leftCurtain.gameObject, -967, 2).setEaseInExpo();


            //Right Curtain
            LeanTween.moveLocalX(rightCurtain.gameObject, 978, 2).setEaseInExpo();

            shooterControllerScript.enabled = true;
        }
    }

   
}