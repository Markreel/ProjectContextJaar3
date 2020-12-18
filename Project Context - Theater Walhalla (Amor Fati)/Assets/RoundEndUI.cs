using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PoolingAndAudio;

namespace ShooterGame
{
    public class RoundEndUI : MonoBehaviour
    {
        [Header("Curtains")]
        public Image leftCurtain;
        public Image rightCurtain;

        [Header("GoudenBel")]
        public GameObject goudenBelBonusUIElement1;
        public GameObject goudenBelBonusUIElement2;
        public GameObject goudenBelBonusUIElement3;
        public GameObject goudenBelBonusCrossOutUIElement1;
        public GameObject goudenBelBonusCrossOutUIElement2;
        public GameObject goudenBelBonusCrossOutUIElement3;
        public float sfxDelay = 1f;

        [Header("HUD")]
        public GameObject goudenBelBonusUIHUDElement;

        public int goudenBelBonusPunten;
        public AudioClip goudenBelBonusClip;

        [Header("Fati")]
        public GameObject fatiBonusUIElement;
        public GameObject fatiBonusCrossOutUIElement;
        public int fatiBonusPunten;
        public AudioClip fatiBonusClip;

        public GameObject scoreBoardContainer;

        public TextMeshProUGUI currentBubbleHitsText;
        public int currentBubbleHitsInt;
        public int bubblesHit;

        public TextMeshProUGUI maxBubbleCountText;
        public int maxBubbleCountInt;


        public TextMeshProUGUI currentCoinsText;

        [Header("Coins")]
        public int currentCoins;
        public int bubbleCoinsEarned;
        public int goudenBelCoinsEarned;
        public int fatiCoinsEarned;

        public GameObject hudUI;

        //public RoundManager roundManagerScript;

        [SerializeField] private ShooterController shooterControllerScript;

        [HideInInspector] public bool hasFinishedAllRounds;

        [SerializeField] public ChangeGameMusic changeGameMusicScript;

        [SerializeField] private GameObject blackImage;

        private NieuwScoreManager nieuwScoreManager;

        public AudioClip wrongSFX;

        private AudioSource source;

        public AudioSource alexSource;
        public AudioClip alexIntro;
        public AudioClip alexHierIsJeBonus;
        public AudioClip wowWatEenScore;

        public Tutorial tutorial;

        public GameObject introSign;

        public GameObject gameOverScreen;

        public ParticleSystem particleSystemCoinsUI;

        public ParticleSystem fatiCoinsParticle1;
        public ParticleSystem fatiCoinsParticle2;


        // Start is called before the first frame update
        void Start()
        {
            nieuwScoreManager = GameManager.Instance.gameObject.GetComponentInChildren<NieuwScoreManager>();

            //Moves the scoreboard up so it can move down later.
            //Set object Y to 1050, which is just out of screen.
            //Why we do this? So we don't have to manually move it out of the screen everytime we are done with editing the scoreboard in Unity.
            scoreBoardContainer.transform.localPosition = new Vector3(0, 1050, 0);

            source = GetComponent<AudioSource>();


            //We should set the coin text here to our coin score we got from the bubble game.


            //Set our Max Bubbles hit to the max hittable bubbles in the round amount. 
            maxBubbleCountText.text = maxBubbleCountInt.ToString();





            blackImage.SetActive(false);

            StartCoroutine(IntroAnimation()); 

            shooterControllerScript.enabled = false;
            tutorial.enabled = false;



        }

        public IEnumerator IntroAnimation()
        {
            yield return new WaitForSeconds(3f);
            //Lower Sign
            LeanTween.moveLocalY(introSign.gameObject, -60, 2).setEaseInExpo();
            yield return new WaitForSeconds(4f);
            //Raise Sign
            LeanTween.moveLocalY(introSign.gameObject, 350, 2).setEaseInExpo();
            yield return new WaitForSeconds(3f);

            tutorial.enabled = true;
            shooterControllerScript.enabled = true;


            //Left Curtain

            LeanTween.moveLocalX(leftCurtain.gameObject, -967, 2).setEaseInExpo();


            //Right Curtain

            LeanTween.moveLocalX(rightCurtain.gameObject, 978, 2).setEaseInExpo();



            yield return null; 

        }





        public IEnumerator ResetGoudenBelHUD_UI()
        {
            yield return new WaitForSeconds(5f);
            goudenBelBonusUIHUDElement.gameObject.SetActive(false);
            yield return null;
        }

        public IEnumerator GoudenBelHUD_UI()
        {
            goudenBelBonusUIHUDElement.transform.localScale = new Vector3(5, 5, 5);
            goudenBelBonusUIHUDElement.gameObject.SetActive(true);
            //Scale the icon down into it's correct place.
            goudenBelBonusUIHUDElement.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
            yield return new WaitForSeconds(0.75f);
            source.clip = goudenBelBonusClip;
            source.Play();
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

            alexSource.clip = alexIntro;
            alexSource.Play();

            StartCoroutine(BubbleCount(0.5f));
        }



        private IEnumerator BubbleCount(float delay = 0.0f)
        {
            if (delay != 0)
                yield return new WaitForSeconds(delay);
            bubblesHit = nieuwScoreManager.bellenGeraakt;




            //Counts up our bubbleAmount.
            LeanTween.value(gameObject, StartCountingBubbles, currentBubbleHitsInt, bubblesHit, 5f).setEaseInExpo();

            //Calculate how much coins we should get. 100 is hardcoded but should be changed into the coin value.
            currentCoins = bubblesHit * 100 + currentCoins;
            //LeanTween.value(gameObject, StartAddingCoins, currentCoins, bubbleCoinsEarned, 8f).setEaseInExpo();

            yield return new WaitForSeconds(6);
            source.clip = goudenBelBonusClip;
            source.Play();
            yield return new WaitForSeconds(1.2f);

            particleSystemCoinsUI.transform.rotation = Quaternion.Euler(0,0,0);
            particleSystemCoinsUI.transform.localPosition = new Vector3(-144, 249, -500);           
            particleSystemCoinsUI.Play(); 




            currentCoinsText.text = currentCoins.ToString();

            //When done with counting:
            //Invoke("GoudenBelBonus", 9f);
            StartCoroutine(GoudenBelBonus(2f));
        }

        //These functions are multiple times in the script. Required to start counting up.
        void StartCountingBubbles(float val)
        {
            currentBubbleHitsText.text = Mathf.Round(val).ToString();
        }
        //void StartAddingCoins(float val)
        //{

        //    currentCoinsText.text = Mathf.Round(val).ToString();
        //}




        private IEnumerator GoudenBelBonus(float delay = 0.0f)
        {
            if (delay != 0)
                yield return new WaitForSeconds(delay);

            alexSource.clip = alexHierIsJeBonus;
            alexSource.Play();


            if (nieuwScoreManager.goudenBellenGeraakt == 0)
            {
                goudenBelBonusCrossOutUIElement1.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusCrossOutUIElement1.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusCrossOutUIElement1.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = wrongSFX;
                source.Play();
                yield return new WaitForSeconds(1.8f);



                goudenBelBonusCrossOutUIElement2.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusCrossOutUIElement2.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusCrossOutUIElement2.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = wrongSFX;
                source.Play();
                yield return new WaitForSeconds(1.8f);



                goudenBelBonusCrossOutUIElement3.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusCrossOutUIElement3.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusCrossOutUIElement3.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = wrongSFX;
                source.Play();
                yield return new WaitForSeconds(1.8f);

            }
            if (nieuwScoreManager.goudenBellenGeraakt == 1)
            {

                goudenBelBonusUIElement1.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusUIElement1.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusUIElement1.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = goudenBelBonusClip;
                source.Play();
                yield return new WaitForSeconds(sfxDelay);
                currentCoins += goudenBelBonusPunten;
                currentCoinsText.text = currentCoins.ToString();

                particleSystemCoinsUI.transform.rotation = Quaternion.Euler(0, 180, 0);
                particleSystemCoinsUI.transform.localPosition = new Vector3(-144, 3, -500);
                particleSystemCoinsUI.Play();


                yield return new WaitForSeconds(1.8f);


                //Place two wrong X. 
                goudenBelBonusCrossOutUIElement2.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusCrossOutUIElement2.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusCrossOutUIElement2.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = wrongSFX;
                source.Play();
                yield return new WaitForSeconds(2f);

                goudenBelBonusCrossOutUIElement3.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusCrossOutUIElement3.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusCrossOutUIElement3.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = wrongSFX;
                source.Play();
                yield return new WaitForSeconds(1.8f);

            }
            if (nieuwScoreManager.goudenBellenGeraakt == 2)
            {

                goudenBelBonusUIElement1.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusUIElement1.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusUIElement1.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = goudenBelBonusClip;
                source.Play();
                yield return new WaitForSeconds(sfxDelay);
                currentCoins += goudenBelBonusPunten;
                currentCoinsText.text = currentCoins.ToString();
                particleSystemCoinsUI.transform.rotation = Quaternion.Euler(0, 180, 0);
                particleSystemCoinsUI.transform.localPosition = new Vector3(-144, 3, -500);
                particleSystemCoinsUI.Play();

                yield return new WaitForSeconds(1.8f);



                goudenBelBonusUIElement2.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusUIElement2.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusUIElement2.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = goudenBelBonusClip;
                source.Play();
                yield return new WaitForSeconds(sfxDelay);
                currentCoins += goudenBelBonusPunten;
                currentCoinsText.text = currentCoins.ToString();

                particleSystemCoinsUI.transform.rotation = Quaternion.Euler(0, 0, 0);
                particleSystemCoinsUI.transform.localPosition = new Vector3(-2, 3, -500);
                particleSystemCoinsUI.Play();


                yield return new WaitForSeconds(1.8f);

                //Place one wrong X. 
                goudenBelBonusCrossOutUIElement3.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusCrossOutUIElement3.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusCrossOutUIElement3.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = wrongSFX;
                source.Play();
                yield return new WaitForSeconds(1.8f);

            }
            if (nieuwScoreManager.goudenBellenGeraakt == 3)
            {

                goudenBelBonusUIElement1.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusUIElement1.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusUIElement1.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = goudenBelBonusClip;
                source.Play();
                yield return new WaitForSeconds(sfxDelay);
                currentCoins += goudenBelBonusPunten;
                currentCoinsText.text = currentCoins.ToString();

                particleSystemCoinsUI.transform.rotation = Quaternion.Euler(0, 180, 0);
                particleSystemCoinsUI.transform.localPosition = new Vector3(-144, 3, -500);
                particleSystemCoinsUI.Play();
                yield return new WaitForSeconds(1.8f);


                goudenBelBonusUIElement2.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusUIElement2.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusUIElement2.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = goudenBelBonusClip;
                source.Play();
                yield return new WaitForSeconds(sfxDelay);
                currentCoins += goudenBelBonusPunten;
                currentCoinsText.text = currentCoins.ToString();
                particleSystemCoinsUI.transform.rotation = Quaternion.Euler(0, 0, 0);
                particleSystemCoinsUI.transform.localPosition = new Vector3(-2, 3, -500);
                particleSystemCoinsUI.Play();
                yield return new WaitForSeconds(1.8f);


                goudenBelBonusUIElement3.transform.localScale = new Vector3(5, 5, 5);
                goudenBelBonusUIElement3.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                goudenBelBonusUIElement3.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = goudenBelBonusClip;
                source.Play();
                yield return new WaitForSeconds(sfxDelay);
                currentCoins += goudenBelBonusPunten;
                currentCoinsText.text = currentCoins.ToString();
                particleSystemCoinsUI.transform.rotation = Quaternion.Euler(0, 0, 0);
                particleSystemCoinsUI.transform.localPosition = new Vector3(146, 3, -500);
                particleSystemCoinsUI.Play();

                yield return new WaitForSeconds(1.8f);


            }


            //Should add in a small delay here, so it starts counting after the above animation is done.
            //Should probably rewrite this to an ienumator. 

            //Calculates how much coins we should add to the coins amount. This so we can tween towards it. 
            //goudenBelCoinsEarned = currentCoins + goudenBelBonusPunten;
            //LeanTween.value(gameObject, StartCountingGoudenBelBonus, currentCoins, goudenBelCoinsEarned, 1f).setEaseInExpo();




            StartCoroutine(FatiBonus(1f));

            yield return null;
        }

        //void StartCountingGoudenBelBonus(float val)
        //{
        //    currentCoinsText.text = Mathf.Round(val).ToString();
        //}

        public IEnumerator FatiBonus(float delay = 0.0f)
        {
            if (delay != 0)
                yield return new WaitForSeconds(delay);



            if (nieuwScoreManager.fatiBonus == 0)
            {
                fatiBonusCrossOutUIElement.transform.localScale = new Vector3(5, 5, 5);
                fatiBonusCrossOutUIElement.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                fatiBonusCrossOutUIElement.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = wrongSFX;
                source.Play();


            }

            if (nieuwScoreManager.fatiBonus == 1)
            {
                fatiBonusUIElement.transform.localScale = new Vector3(5, 5, 5);
                fatiBonusUIElement.gameObject.SetActive(true);
                //Scale the icon down into it's correct place.
                fatiBonusUIElement.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();
                yield return new WaitForSeconds(0.75f);
                source.clip = goudenBelBonusClip;
                source.Play();
                yield return new WaitForSeconds(sfxDelay);
                //fatiCoinsEarned = currentCoins + fatiBonusPunten;
                //LeanTween.value(gameObject, StartCountingGoudenBelBonus, currentCoins, fatiCoinsEarned, 1f).setEaseInExpo();

                fatiCoinsParticle1.Play();
                fatiCoinsParticle2.Play();


                currentCoins = 1000000;
                currentCoinsText.text = currentCoins.ToString();

                alexSource.clip = wowWatEenScore;
                alexSource.Play();
            }








            //Check here if you got the Fati Bonus.
            //If No then do a stripethrough animation.
            //GameObject setactive stripethrough animation.

            //If Yes then:

            //fatiBonusUIElement.transform.localScale = new Vector3(5, 5, 5);
            //fatiBonusUIElement.gameObject.SetActive(true);
            ////Scale the icon down into it's correct place.
            //fatiBonusUIElement.LeanScale(new Vector3(1, 1, 1), 0.75f).setEaseInExpo();

            ////Calculates how much coins we should add to the coins amount. This so we can tween towards it. 
            //fatiCoinsEarned = currentCoins + fatiBonusPunten;
            //LeanTween.value(gameObject, StartCountingGoudenBelBonus, currentCoins, fatiCoinsEarned, 1f).setEaseInExpo();


            Invoke("RaiseScoreBoard", 3f);
        }

        //void StartCountingFatiBonus(float val)
        //{
        //    currentCoinsText.text = Mathf.Round(val).ToString();
        //}

        public void RaiseScoreBoard()
        {
            //Scoreboard get's Raised back up.
            LeanTween.moveLocalY(scoreBoardContainer, 1050, 2.5f).setEaseInExpo();

            

            //keep the curtains closed after the last round so we can transition from there with a cut or fade to the footage. 

            turnOnBlackImage();

            fatiCoinsParticle1.Stop();
            fatiCoinsParticle2.Stop(); 


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
            Invoke("RoundCloseCurtains", 2f);

        }

        public void RoundCloseCurtains()
        {
            //Left Curtain
            LeanTween.moveLocalX(leftCurtain.gameObject, 11, 3).setEaseInExpo();
            //Right Curtain
            LeanTween.moveLocalX(rightCurtain.gameObject, -12, 3).setEaseInExpo();

            //shooterControllerScript.enabled = false;

            Invoke("RoundOpenCurtains", 6f);

        }

        public void RoundOpenCurtains()
        {
            //Left Curtain
            LeanTween.moveLocalX(leftCurtain.gameObject, -967, 2).setEaseInExpo();


            //Right Curtain
            LeanTween.moveLocalX(rightCurtain.gameObject, 978, 2).setEaseInExpo();

            //shooterControllerScript.enabled = true;

            changeGameMusicScript.ChangeAudio();
        }

        public void turnOnBlackImage()
        {
            blackImage.SetActive(true);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            shooterControllerScript.enabled = false;

            Invoke("NextScene", 7f);
        }

        public void NextScene()
        {
            //Should probably fade the audo out here. 

            EpisodeManager.Instance.NextEpisode();

        }


        public void GameOverScreen()
        {
            gameOverScreen.SetActive(true); 
        }








    }


}