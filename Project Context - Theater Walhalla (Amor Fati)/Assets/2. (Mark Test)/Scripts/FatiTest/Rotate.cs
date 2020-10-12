using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private void Update()
    {
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, transform.localEulerAngles + Vector3.up * speed, Time.deltaTime);
    }
}
