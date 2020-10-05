using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TelevisionControl : MonoBehaviour
{
    public static TelevisionControl Instance;

    // Private variables
    private List<GameObject> televisionList = new List<GameObject>();
    private List<GameObject> randomizedTVList = new List<GameObject>();
    private int mediaCounter = 0;
    private MediaData mediaData;

    [SerializeField] private TelevisionGroup[] televisionGroups = new TelevisionGroup[4];

    // Adjustable fields
    [SerializeField] GameObject television;
    [SerializeField] float turnOffDuration;
    [SerializeField] AnimationCurve turnOffCurve;
    [SerializeField] float changeChannelDuration;
    [SerializeField] AnimationCurve changeChannelCurve;
    [SerializeField] int televisionCount = 20;
    [SerializeField] float horizontalDistance = 1.5f;
    [SerializeField] float verticalDistance = 1.5f;
    [SerializeField] Vector3 startPosition = new Vector3(0, 0, 0);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mediaData = DataManager.Instance.MediaData;
    }

    public void SpawnTelevisions()
    {
        for (int i = 0; i < televisionGroups.Length; i++)
        {
            int _count = televisionGroups[i].unassignedTelevisions.Count;
            for (int j = 0; j < _count; j++)
            {
                televisionGroups[i].unassignedTelevisions[0].TurnedOn = true;
                televisionGroups[i].unassignedTelevisions[0].tvName = "Television" + i + "|" + j;
                televisionGroups[i].unassignedTelevisions[0].SetScreen(mediaData.MediaItems[i][j]);

                televisionGroups[i].assignedTelevisions.Add(televisionGroups[i].unassignedTelevisions[0]);
                televisionGroups[i].unassignedTelevisions.RemoveAt(0);
            }
        }

        for (int i = 0; i < televisionGroups.Length; i++)
        {
            foreach (var _tvItem in televisionGroups[i].assignedTelevisions)
            {
                // Continue if there was no media item assigned to this object
                if (_tvItem.GetComponent<TelevisionItem>().mediaItem == null) continue;

                // Turn off if the assigned item was not liked
                if (!_tvItem.GetComponent<TelevisionItem>().mediaItem.Liked) { StartCoroutine(TurnOnOff(_tvItem, turnOffDuration, turnOffCurve)); }
            }
        }
    }

    // Turn tvs on or off
    private IEnumerator TurnOnOff(TelevisionItem _tvItem, float duration, AnimationCurve _curve)
    {
        //Give random delay to televisions that are turning off for the first time
        if(_curve == turnOffCurve) { yield return new WaitForSeconds(Random.Range(0f, 0.5f)); }

        float tick = 0f;

        // Do nothing if there was no media item assigned to this object
        if (_tvItem.mediaItem == null) yield return null;

        // If tv was on, turn it off
        if (_tvItem.TurnedOn)
        {
            while (tick < 1)
            {
                tick += Time.deltaTime / (duration > 0 ? duration : 1);
                float _evaluatedTick = _curve.Evaluate(tick);

                _tvItem.Renderer.material.SetColor("_BaseColor", Color.Lerp(_tvItem.mediaItem.Material.color, Color.black, _evaluatedTick));
                yield return null;
            }

            _tvItem.TurnedOn = false;
        }

        // If tv was off, turn it on
        else
        {

            while (tick < 1)
            {
                tick += Time.deltaTime / (duration > 0 ? duration : 1);
                float _evaluatedTick = _curve.Evaluate(tick);

                _tvItem.Renderer.material.SetColor("_BaseColor", Color.Lerp(Color.black, _tvItem.mediaItem.Material.GetColor("_BaseColor"), _evaluatedTick));                
                yield return null;
            }

            _tvItem.TurnedOn = true;
        }
    }

    // Called every frame
    private void Update()
    {
        // Change the on/off status if the TV's screen is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = CameraController.Instance.ActiveCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedItem = hit.collider.gameObject;

                // Check if user clicked on a screen
                if (clickedItem.transform.parent != null && clickedItem.transform.parent.parent != null && (clickedItem.transform.parent.parent).GetComponent<TelevisionItem>())
                {
                    GameObject tv = clickedItem.transform.parent.parent.gameObject;
                    Debug.Log("You clicked this item: " + tv.GetComponent<TelevisionItem>().tvName);
                    StartCoroutine(TurnOnOff(tv.GetComponent<TelevisionItem>(), changeChannelDuration, changeChannelCurve));
                }
            }
        }
    }
}

[System.Serializable]
public class TelevisionGroup
{
    public MediaTopic MediaTopic;
    public List<TelevisionItem> unassignedTelevisions = new List<TelevisionItem>();
    public List<TelevisionItem> assignedTelevisions = new List<TelevisionItem>();
}
