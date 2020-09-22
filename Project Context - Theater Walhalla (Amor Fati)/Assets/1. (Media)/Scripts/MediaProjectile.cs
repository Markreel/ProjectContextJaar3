using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaProjectile : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(transform.forward);
    }
}
