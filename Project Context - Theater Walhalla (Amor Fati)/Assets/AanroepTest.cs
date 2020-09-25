using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AanroepTest : MonoBehaviour
{

    [SerializeField] ServerManager serverManager;

    void Start()
    {
        serverManager = GetComponent<ServerManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            
        serverManager.labels[0] = "playerName";
        serverManager.values[0] = "Willem-Jan Renger";

        serverManager.labels[1] = "SelectedThings";
        serverManager.values[1] = "10";
    
        StartCoroutine(serverManager.postData());
        }
    }
}
