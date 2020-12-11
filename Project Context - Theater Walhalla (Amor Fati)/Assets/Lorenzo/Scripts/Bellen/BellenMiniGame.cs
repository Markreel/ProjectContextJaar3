using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//using UnityEditorInternal;
using System.Runtime.InteropServices;

public class BellenMiniGame : MonoBehaviour
{
    public GameObject canvas;
    public GameObject boundaries;
    public GameObject camera1;
    public GameObject camera2;
    public AnimationCurve dropCurve;
    public AnimationCurve rotationCurve;
    public AnimationCurve dropCameraCurve;

    //Fati
    private FatiFirstMinigame fatiManager;

    public MeshFilter BoundariesMesh;

    // Bubbles
    [Header("Bubbles")]
    // Bubble variables
    public List<SnapBubble> requiredBubbles;
    public List<GameObject> bubbles;
    [HideInInspector] public int filledInBubbles;
    //Spawning Bubbles
    public Transform bubbleSpawnLocation;
    public GameObject bubblePrefab;
    public bool canSpawnBubble;

    //Coin Generation
    [Header("Coin Generation")]
    public bool generatingCoins;
    public TextMeshProUGUI coinText;
    public float coinAmount;
    public TextMeshProUGUI coinMultiplierText;
    public int coinMuliplierInt;
    public Image coinImage;
    public GameObject coinUIRoot;

    // UI
    [Header("StartScreen")]
    public Button newBubbleButton;
    public GameObject bellenblaasstok;
    public Image neBubbleButtonImage;
    public TextMeshProUGUI titleText;

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
        // Update counter
        bellenblaasstok.GetComponentInChildren<TextMeshPro>().text = $"{filledInBubbles}/{requiredBubbles.Count}";

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
            //Cleans up any left behind lines of Fati.
            fatiManager.lineRenderer.positionCount = 0;
        }
    }

    public void StartGame()
    {
        //When we have the required amount of bubbles the button will instead trigger this function.
        if (filledInBubbles == requiredBubbles.Count)
        {
            // Turn off bellenblaasstok image
            bellenblaasstok.SetActive(false);

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
                bubble.GetComponent<DragObject>().falling = true;
                // Set the variables allowing the bubble to change to a new position
                bubble.GetComponent<DragObject>().protectGame = true;
                bubble.GetComponent<DragObject>().SetBounds();
                //Makes it so our bubbles dont float away with the velocity they retained.
                bubble.GetComponent<DragObject>().velocityMulti = 0;
                //Set our coin multiplier to the amount of bubbles.
                coinMuliplierInt++;
            }

            // Move the game objects to second postion
            Vector3 canvasOffset = canvas.transform.position - gameObject.transform.position;
            Vector3 boundariesOffset = boundaries.transform.position - gameObject.transform.position;
            StartCoroutine(TransitionToGame(gameObject.transform.position, gameObject.transform.position + new Vector3(0, -8, 0), 
                canvasOffset, boundariesOffset, 3));
        }
    }

    public IEnumerator TransitionToGame(Vector3 startPos, Vector3 endPos, Vector3 canvasOffset, Vector3 boundariesOffset, float duration)
    {
        // Turn off UI for now
        canvas.SetActive(false);

        float tick = 0f;

        yield return new WaitForSeconds(1);

        // Move camera
        Invoke("MoveCamera", 2);

        // Lower the elements
        while (tick < 1)
        {
            tick += Time.deltaTime / (duration > 0 ? duration : 1);
            float _evaluatedTick = dropCurve.Evaluate(tick);
            canvas.transform.position = Vector3.Lerp(startPos + canvasOffset, endPos + canvasOffset, _evaluatedTick);
            boundaries.transform.position = Vector3.Lerp(startPos + boundariesOffset, endPos + boundariesOffset, _evaluatedTick);
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, _evaluatedTick);            
            yield return null;
        }

         // Rotate the elements
        canvas.transform.eulerAngles += new Vector3(90, 0, 0);
        gameObject.transform.eulerAngles += new Vector3(90, 0, 0);
        boundaries.transform.eulerAngles += new Vector3(90, 0, 0);

        // Give the bubbles a slight offset and shrink them
        foreach (var bubble in bubbles)
        {
            //Scales down our bubble to make it easier to dodge Fati.
            LeanTween.scale(bubble, new Vector3(0.65f, 0.65f, 0.65f), 0.75f);
            // Offset
            float zOffset = Random.Range(-1.0f, 1.0f);
            bubble.transform.position += new Vector3(0, 0, zOffset);
            // Set them on fixed height
            bubble.GetComponent<DragObject>().falling = false;
            bubble.GetComponent<DragObject>().fixedY = gameObject.transform.position.y;
        }

        yield break;
    }

    public void MoveCamera()
    {
        // Switch cameras
        camera1.SetActive(false);
        camera2.SetActive(true);
        float rotateDuration = 2f;
        float moveDuration = 2f;
        StartCoroutine(MoveSecondCamera(rotateDuration, moveDuration));
    }

    public IEnumerator MoveSecondCamera(float rotateDuration, float moveDuration)
    {
        yield return new WaitForSeconds(1);

        // Set positions
        Vector3 startRot = camera2.transform.eulerAngles;
        Vector3 endRot = camera2.transform.eulerAngles + new Vector3(90, 0, 0);
        Vector3 startPos = camera2.transform.position;
        Vector3 endPos = camera2.transform.position + new Vector3(0, -2f, 0);

        // Rotate the camera
        float tick = 0f;
        while (tick < 1)
        {
            float _evaluatedTick = rotationCurve.Evaluate(tick);
            tick += Time.deltaTime / (rotateDuration > 0 ? rotateDuration : 1);
            camera2.transform.eulerAngles = Vector3.Lerp(startRot, endRot, _evaluatedTick);
            yield return null;
        }

        // Lower the camera
        tick = 0f;
        while (tick < 1)
        {
            float _evaluatedTick = dropCameraCurve.Evaluate(tick);
            tick += Time.deltaTime / (moveDuration > 0 ? moveDuration : 1);
            camera2.transform.position = Vector3.Lerp(startPos, endPos, _evaluatedTick);
            yield return null;
        }

        canvas.SetActive(true);

        // Start the game
        StartProtectGame();

        yield break;        
    }

    public void StartProtectGame()
    {
        //Start the Fati attack.
        fatiManager.StartFirstAttack();

        // Start coin generation
        StartCoinGeneration();
    }

    public void SpawnBubble()
    {
        //Only allow to spawn a bubble if we do not have the max amount of bubbles and we do not currently have a bubble not dragged into the sleep je bubbel hier.
        if (canSpawnBubble && filledInBubbles < requiredBubbles.Count)
        {
            // Disable new bell button
            newBubbleButton.gameObject.SetActive(false);

            GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnLocation);
            bubble.transform.localScale = new Vector3(0, 0, 0);
            // Set variables for drag component
            bubble.GetComponent<DragObject>().fixedZ = this.transform.position.z;
            bubble.GetComponent<DragObject>().protectGame = false;
            LeanTween.scale(bubble, new Vector3(1, 1, 1), 0.5f).setEaseInExpo();
            bubbles.Add(bubble);

            DragObject dragObject = bubble.GetComponent<DragObject>();
            if (dragObject != null) dragObject.BoundariesMesh = BoundariesMesh;

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
    //Function required to use the value sadly. 
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