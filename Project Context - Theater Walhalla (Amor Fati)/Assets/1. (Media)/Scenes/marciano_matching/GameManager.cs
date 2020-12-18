using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarcianoMatching
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] string selected;
        [SerializeField] GameObject[] objects;

        [SerializeField] GameObject[] selectedobjects;


        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Debug.Log("You selected " + hit.transform.name);
                }

                if (selected == "")
                {
                    selected = hit.transform.name;
                    hit.collider.SendMessage("Selected");
                    selectedobjects[0] = hit.collider.gameObject;
                }
                else if (selected == hit.transform.name)
                {
                    hit.collider.SendMessage("Selected");
                    selectedobjects[1] = hit.collider.gameObject;
                    Debug.Log("goodshot");
                    selected = "";
                }
                else
                {
                    selectedobjects[1] = hit.collider.gameObject;

                    selectedobjects[0].SendMessage("Deselect");
                    selectedobjects[1].SendMessage("Deselect");

                    Debug.Log("wrong");
                    selected = "";
                }
            }

        }

    }
}
