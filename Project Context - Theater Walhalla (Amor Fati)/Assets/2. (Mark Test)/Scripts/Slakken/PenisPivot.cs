using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenisPivot : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        transform.position -= Vector3.up * movementSpeed / 100f;
    }
}
