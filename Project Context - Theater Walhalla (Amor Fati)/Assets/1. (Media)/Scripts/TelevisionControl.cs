using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TelevisionControl : MonoBehaviour
{
    public static TelevisionControl Instance;
    public System.Random r = new System.Random();

    // Private variables
    private List<GameObject> televisionList = new List<GameObject>();
    private List<GameObject> randomizedTVList = new List<GameObject>();
    private int mediaCounter = 0;

    // Adjustable fields
    [SerializeField] GameObject television;
    [SerializeField] float turnOffDuration;
    [SerializeField] float changeChannelDuration;
    [SerializeField] int televisionCount = 20;
    [SerializeField] float horizontalDistance = 1.5f;
    [SerializeField] float verticalDistance = 1.5f;
    [SerializeField] Vector3 startPosition = new Vector3(0, 0, 0);

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Calculate the number of media items 
        // int itemsCount = DataManager.Instance.MediaData.MediaItems.Count; // gives Unity error
        
        int itemsCount = 0; // For debugging purposes

        // Generate the televisions
        for (int i = 0; i < televisionCount; i++)
        {
            // Generate dummy positions
            float xPos, yPos;
            if (i < televisionCount / 2)
            {
                xPos = startPosition.x + i * horizontalDistance;
                yPos = startPosition.y;
            }

           else
            {
                xPos = startPosition.x + (i - 10) * horizontalDistance;
                yPos = startPosition.y + verticalDistance;
            }

            Vector3 position = new Vector3(xPos, yPos, 0);

            // Create the televisions
            GameObject _television = Instantiate(television, position, transform.rotation);
            _television.GetComponent<TelevisionItem>().TurnedOn = true;
            // If there are media items present, assign them to the TV screens
            if (itemsCount > 0) _television.GetComponent<TelevisionItem>().SetScreen(mediaCounter % itemsCount);
            // Give the tv a name for debugging purposes
            _television.GetComponent<TelevisionItem>().tvName = "Television" + mediaCounter;
            televisionList.Add(_television);
            mediaCounter++;
        }

        // Turn the necessary tvs off
        foreach (GameObject t in televisionList)
        {
            // Continue if there was no media item assigned to this object
            if (t.GetComponent<TelevisionItem>().mediaItem == null) continue;
            // Turn off if the assigned item was not liked
            if (!t.GetComponent<TelevisionItem>().mediaItem.Liked) TurnOnOff(t, turnOffDuration);
        }
    }

    // Turn tvs on or off
    private IEnumerator TurnOnOff(GameObject t, float duration)
    {
        float tick = 0f;

        // Do nothing if there was no media item assigned to this object
        if (t.GetComponent<TelevisionItem>().mediaItem == null) yield return null;

        // If tv was on, turn it off
        if (t.GetComponent<TelevisionItem>().TurnedOn)
        {
            while (t.GetComponent<MeshRenderer>().material.color != Color.black)
            {
                tick += Time.deltaTime * duration;
                t.GetComponent<MeshRenderer>().material.color = Color.Lerp(t.GetComponent<TelevisionItem>().mediaItem.Material.color, Color.black, tick);
                yield return null;
            }

            t.GetComponent<TelevisionItem>().TurnedOn = false;
        }

        // If tv was off, turn it on
        if (!t.GetComponent<TelevisionItem>().TurnedOn)
        {
            while (t.GetComponent<MeshRenderer>().material.color != t.GetComponent<TelevisionItem>().mediaItem.Material.color)
            {
                tick += Time.deltaTime * duration;
                t.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.black, t.GetComponent<TelevisionItem>().mediaItem.Material.color, tick);
                yield return null;
            }

            t.GetComponent<TelevisionItem>().TurnedOn = true;
        }
    }

    // Called every frame
    private void Update()
    {
        // Change the on/off status if the TV's screen is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedItem = hit.collider.gameObject;

                // Check if user clicked on a screen
                if (clickedItem.transform.parent != null && clickedItem.transform.parent.parent != null && (clickedItem.transform.parent.parent).GetComponent<TelevisionItem>())
                {
                    GameObject tv = clickedItem.transform.parent.parent.gameObject;
                    Debug.Log("You clicked this item: " + tv.GetComponent<TelevisionItem>().tvName);
                    TurnOnOff(tv, changeChannelDuration);
                }
            }
        }
    }

    // Randomize TV list 
    private List<GameObject> randomizeTVList()
    {
        // Randomize the tv
        List<GameObject> randomizedTelevisionList = new List<GameObject>();
        List<GameObject> copyTelevisionList = televisionList;

        int likedCount = DataManager.Instance.MediaData.MediaItems.Count;

        while (randomizedTelevisionList.Count < likedCount)
        {
            int index = r.Next(0, copyTelevisionList.Count);
            randomizedTelevisionList.Add(copyTelevisionList[index]);
            copyTelevisionList.RemoveAt(index);
        }

        return randomizedTelevisionList;
    }
}
