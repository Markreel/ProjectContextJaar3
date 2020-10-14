using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class BellenMiniGame : MonoBehaviour
{
    public List<TMP_InputField> inputFields;

    public GameObject canvas;

    private int filledInBubbles;

    public Button startGameButton;
    public Image startGameButtonImage;

    public TextMeshProUGUI titleText;



    public Transform bubbleSpawnLocation;
    public GameObject bubblePrefab;




    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInChildren(false, inputFields);
        startGameButton.interactable = false;


    }

    // Update is called once per frame
    void Update()
    {
        //0 is required so users need to have all fields active at the same time, if that is not the case it resets to 0.
        filledInBubbles = 0;

        //Here we check if all the text fields have been filled in.
        foreach (var inputField in inputFields)
        {
            if (!string.IsNullOrEmpty(inputField.text))
            {
                filledInBubbles++;
            }
        }

        //When the amount of filled in text fields equals the total textfields the start game button appears.
        if (filledInBubbles == inputFields.Count)
        {
            startGameButton.interactable = true;
        }
        else
        {
            startGameButton.interactable = false;
        }




        

    }

    //Why not all the update code in this function? Well because otherwise we need to click on the start button before the graphic changes.
    public void StartGame()
    {
        //DO SOME SORT OF FADE OUT ANIMATION FOR THE BUTTON HERE.
        startGameButton.transform.LeanMoveY(startGameButton.transform.position.y - 200, 2f).setEaseOutCubic();

        //Fade out the title
        LeanTween.scale(titleText.gameObject, new Vector3(0, 0, 0), 0.75f);

        //Stops you from writing text after the game starts.
        foreach (var inputField in inputFields)
        {
            inputField.enabled = false;
            LeanTween.scale(inputField.transform.root.gameObject, new Vector3(0.75f, 0.75f, 0.75f), 1f);

        }


        inputFields[0].GetComponentInParent<DragObject>().canDrag = true;

    }


    public void SpawnBubble()
    {
        Instantiate(bubblePrefab, bubbleSpawnLocation);




    }




}
