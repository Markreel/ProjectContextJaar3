using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wifiAnimationUI : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(FlipScale());
    }

    IEnumerator FlipScale()
    {
        Vector3 target = new Vector3(0, 0, 20);
        transform.rotation = Quaternion.Euler(target);
        yield return new WaitForSeconds(1);
        target = new Vector3(0, 0, -20);
        transform.rotation = Quaternion.Euler(target);
        yield return new WaitForSeconds(1);
        StartCoroutine(FlipScale());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
