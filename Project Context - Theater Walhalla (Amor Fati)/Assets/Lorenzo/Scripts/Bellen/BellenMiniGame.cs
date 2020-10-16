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

    //Coin Generation.
    public bool generatingCoins;

    public TextMeshProUGUI coinText;
    public float coinAmount;
    public TextMeshProUGUI coinMultiplierText;
    public int coinMuliplierInt; 
    public Image coinImage;






    // Start is called before the first frame update
    void Start()
    {
        //We get all the requiredbubbles by checking how many sleep je bel hier fields we have.
        GetComponentsInChildren(false, requiredBubbles);
        fatiManager = GetComponentInChildren<FatiFirstMinigame>();

        canSpawnBubble = true;

        //Hides the UI elements by making them 0 scale so we can tween them.
        coinImage.gameObject.transform.localScale = new Vector3(0,0,0);
        coinText.gameObject.transform.localScale = new Vector3(0,0,0);
        coinMultiplierText.gameObject.transform.localScale = new Vector3(0, 0, 0);


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

    }

    //Why not all the update code in this function? Well because otherwise we need to click on the start button before the graphic changes.
    public void StartGame()
    {
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
            LeanTween.scale(bubble, new Vector3(1, 1, 1), 1f).setEaseInExpo();
            bubbles.Add(bubble);

            DragObject dragObject = bubble.GetComponent<DragObject>();
            if (dragObject != null) { dragObject.BoundariesMesh = BoundariesMesh; }

            //Before we can spawn a new one this value needs to be set to true again by the SnapBubble Script.
            canSpawnBubble = false;

        }
    }

    public void StartCoinGeneration()
    {
        generatingCoins = true;
        LeanTween.scale(coinImage.gameObject, new Vector3(1, 1, 1), 1f);
        LeanTween.scale(coinText.gameObject, new Vector3(1, 1, 1), 1f);
        LeanTween.scale(coinMultiplierText.gameObject, new Vector3(1, 1, 1), 1f);

    }




}
