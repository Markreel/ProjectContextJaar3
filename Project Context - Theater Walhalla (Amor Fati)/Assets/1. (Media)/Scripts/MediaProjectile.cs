using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MediaProjectile : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private TextMeshPro text;

    private void FixedUpdate()
    {
        //transform.Translate(-transform.forward / 10f * MP_Spawner.Instance.SpeedMultiplier);

        if(transform.position.z < -10) { Destroy(gameObject); }
    }

    public void SetData(Material _mat, string _text)
    {
        renderer.material = _mat;
        text.text = _text;
    }
}