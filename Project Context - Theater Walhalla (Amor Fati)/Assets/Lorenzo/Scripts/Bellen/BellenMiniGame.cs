using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class BellenMiniGame : MonoBehaviour
{
    public List<SnapBubble> requiredBubbles;
    public List<GameObject> bubbles;

    public GameObject canvas;

    [HideInInspector] public int filledInBubbles;

    public Button newBubbleButton;
    public Image neBubbleButtonImage;

    public TextMeshProUGUI titleText;

    //Spawning Bubbles
    public Transform bubbleSpawnLocation;
    public GameObject bubblePrefab;

    public bool canSpawnBubble;

    //Fati
    private FatiFirstMinigame fatiManager;

    public MeshFilter BoundariesMesh;

    [Header("Coin Generation")]
    //Coin Generation.
    public bool generatingCoins;

    public TextMeshProUGUI coinText;
    public float coinAmount;
    public TextMeshProUGUI coinMultiplierText;
    public int coinMuliplierInt;
    public Image coinImage;
    public GameObject coinUIRoot;

    [Header("EndScreen")]
    public GameObject playerScore;
    public GameObject scoreBoard;
    public TextMeshProUGUI endScreenPlayerScore;
    public float endScreenPlayerScoreFloat;

    public int coinTweenID;


    // Start is called before the first frame update
    void Start()
    {
        //We get all the requiredbubbles by checking how many sleep je bel hier fields we have.
        GetComponentsInChildren(false, requiredBubbles);
        fatiManager = GetComponentInChildren<FatiFirstMinigame>();

        canSpawnBubble = true;

        //Hides the UI elements by making them 0 scale so we can tween them.
        coinImage.gameObject.transform.localScale = new Vector3(0, 0, 0);
        coinText.gameObject.transform.localScale = new Vector3(0, 0, 0);
        coinMultiplierText.gameObject.transform.localScale = new Vector3(0, 0, 0);

        //Turns of our EndScreen at the start of the game.
        playerScore.SetActive(false);
        playerScore.transform.localScale = new Vector3(0, 0, 0);
        scoreBoard.SetActive(false);
        scoreBoard.transform.localScale = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //To keep the amount of fill in fields possibly dynamic instead of harcoded.
        if (filledInBubbles == requiredBubbles.Count)
        {
            //Do something to change the nieuwe bel into the start game button.
            newBubbleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";
        }

        if (generatingCoins)
        {

            coinText.text = Mathf.Round(coinAmount += Time.deltaTime * coinMuliplierInt).ToString();
            coinMultiplierText.text = "X " + coinMuliplierInt.ToString();
        }

        //If all bubbles have been destroyed...
        if (filledInBubbles == 0 && generatingCoins)
        {
            generatingCoins = false;
            StartCoroutine(EndScreen());
            //Stop fati from attacking.
            fatiManager.enabled = false;
        }

    }


    public void StartGame()
    {
        //When we have the required amount of bubbles the button will instead trigger this function.
        if (filledInBubbles == requiredBubbles.Count)
        {
            //DO SOME SORT OF FADE OUT ANIMATION FOR THE BUTTON HERE.
            newBubbleButton.transform.LeanMoveY(newBubbleButton.transform.position.y - 200, 2f).setEaseOutCubic();

            //Fade out the title
            LeanTween.scale(titleText.gameObject, new Vector3(0, 0, 0), 0.75f);


            foreach (var bubble in bubbles)
            {
                //Makes it so we can no longer type in the Bubble.
                bubble.GetComponentInChildren<TMP_InputField>().interactable = false;
                //Makes it possible to drag our Bubble again.
                bubble.GetComponent<SphereCollider>().enabled = true;
                bubble.GetComponent<DragObject>().canDrag = true;
                //Makes it so our bubbles dont float away with the velocity they retained.
                bubble.GetComponent<DragObject>().velocityMulti = 0;
                //Scales down our bubble to make it easier to dodge Fati.
                LeanTween.scale(bubble, new Vector3(0.65f, 0.65f, 0.65f), 0.75f);
                //Start the Fati attack.
                fatiManager.StartFirstAttack();
                //Set our coin multiplier to the amount of bubbles.
                coinMuliplierInt++;
            }

            StartCoinGeneration();

        }
    }

    public void SpawnBubble()
    {
        //Only allow to spawn a bubble if we do not have the max amount of bubbles and we do not currently have a bubble not dragged into the sleep je bubbel hier.
        if (canSpawnBubble && filledInBubbles < requiredBubbles.Count)
        {
            //bubbles.Add(Instantiate(bubblePrefab, bubbleSpawnLocation));
            //DragObject dragObject =

            GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnLocation);
            bubble.transform.localScale = new Vector3(0, 0, 0);
            LeanTween.scale(bubble, new Vector3(1, 1, 1), 0.5f).setEaseInExpo();
            bubbles.Add(bubble);

            DragObject dragObject = bubble.GetComponent<DragObject>();
            if (dragObject != null) { dragObject.BoundariesMesh = BoundariesMesh; }

            //Before we can spawn a new one this value needs to be set to true again by the SnapBubble Script.
            canSpawnBubble = false;

        }
    }

    public void StartCoinGeneration()
    {
        //Starts coin generation and shows our coin UI.
        generatingCoins = true;
        LeanTween.scale(coinImage.gameObject, new Vector3(1, 1, 1), 1f);
        LeanTween.scale(coinText.gameObject, new Vector3(1, 1, 1), 1f);
        LeanTween.scale(coinMultiplierText.gameObject, new Vector3(1, 1, 1), 1f);

    }

    IEnumerator EndScreen()
    {
        //Shows our Player Scoreboard.
        playerScore.SetActive(true);
        LeanTween.scale(playerScore, new Vector3(1, 1, 1), 1f).setEaseInCubic();

        //Move the Coin UI to the middle of the screen.
        LeanTween.moveLocalY(coinUIRoot, -5, 2f).setEaseInExpo();

        //Return time should be the same as it takes for the coin UI to move.
        yield return new WaitForSeconds(2f);

        StartCoroutine(DrainCoins());






    }

    IEnumerator DrainCoins()
    {
        //Fills up our scoreboard coin UI.
        LeanTween.value(gameObject, givePlayerCoins, endScreenPlayerScoreFloat, coinAmount, 6f).setEaseInExpo();

        //Drains out the coins in the Coin UI. 
        LeanTween.value(gameObject, drainCoinScore, coinAmount, 0f, 6f).setEaseInExpo();

        //Return time should be the same as it takes for the coins to drain + a second or two to not make it instant.
        yield return new WaitForSeconds(7.25f);
        StartCoroutine(ShowScoreBoard());
    }

    //Function required to use the value sadly. 
    void givePlayerCoins(float val)
    {
        endScreenPlayerScore.text = Mathf.Round(val).ToString();
    }

    void drainCoinScore(float val)
    {
        coinText.text = Mathf.Round(val).ToString();
    }

    IEnumerator ShowScoreBoard()
    {
        scoreBoard.SetActive(true);
        LeanTween.scale(scoreBoard, new Vector3(1, 1, 1), 1f).setEaseInCubic();

        //However long you want the scoreboard to exist. Could also replaced by a continue button.
        //yield return new WaitForSeconds(2f);
        yield break;
    }



}