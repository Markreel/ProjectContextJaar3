using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonFunctionality : MonoBehaviour
{
    [SerializeField] internal MicrophoneControle microphoneControl;
    [SerializeField] internal GameObject exportScherm;

    GameObject buttonObject;
    bool pulsing;
    bool canStartPulsing;

    public virtual void Awake()
    {
        exportScherm.SetActive(false);
        canStartPulsing = true;
    }

   internal void OnMouseOver()
    {
        Debug.Log("Current obj: " + buttonObject);
        if (canStartPulsing)
        {
            pulsing = true;
            StartCoroutine(Pulse());
        }
    }

    internal void OnMouseExit()
    {
        pulsing = false;
        canStartPulsing = true;
    }

    internal IEnumerator Pulse()
    {
        // Set object
        buttonObject = this.gameObject.transform.parent.gameObject;

        // User feedback
        if (pulsing)
        {
            canStartPulsing = false;

            for (float i = 0f; i <= 1f; i += 0.1f)
            {
                buttonObject.transform.localScale = new Vector3(
                    (Mathf.Lerp(buttonObject.transform.localScale.x, buttonObject.transform.localScale.x + 0.005f, Mathf.SmoothStep(0f, 1f, i))),
                    (Mathf.Lerp(buttonObject.transform.localScale.y, buttonObject.transform.localScale.y + 0.005f, Mathf.SmoothStep(0f, 1f, i))),
                    (Mathf.Lerp(buttonObject.transform.localScale.z, buttonObject.transform.localScale.z + 0.005f, Mathf.SmoothStep(0f, 1f, i)))
                    );
                yield return new WaitForSeconds(0.015f);
            }

            for (float i = 0f; i <= 1f; i += 0.1f)
            {
                buttonObject.transform.localScale = new Vector3(
                    (Mathf.Lerp(buttonObject.transform.localScale.x, buttonObject.transform.localScale.x - 0.005f, Mathf.SmoothStep(0f, 1f, i))),
                    (Mathf.Lerp(buttonObject.transform.localScale.y, buttonObject.transform.localScale.y - 0.005f, Mathf.SmoothStep(0f, 1f, i))),
                    (Mathf.Lerp(buttonObject.transform.localScale.z, buttonObject.transform.localScale.z - 0.005f, Mathf.SmoothStep(0f, 1f, i)))
                    );
                yield return new WaitForSeconds(0.015f);
            }

            canStartPulsing = true;
        }
    }
}
