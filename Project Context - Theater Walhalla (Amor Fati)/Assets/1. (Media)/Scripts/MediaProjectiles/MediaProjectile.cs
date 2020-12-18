using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MediaProjectile : MonoBehaviour, IFocusable
{
    [SerializeField] protected GameObject likeHighlight;
    [SerializeField] private GameObject dislikeHighlight;
    [SerializeField] private Renderer renderer;

    [HideInInspector] public MediaTopic MediaTopic;
    [HideInInspector] public Material Material;

    private void FixedUpdate()
    {
        //transform.Translate(-transform.forward / 10f * MP_Spawner.Instance.SpeedMultiplier);

        if(transform.position.z < -10) { Destroy(gameObject); }
    }

    public void SetData(Material _mat, MediaTopic _mediaTopic)
    {
        renderer.material = Material = _mat;
        MediaTopic = _mediaTopic;
    }

    public virtual void Focus(Vector2 _mousePos)
    {
        bool _value = _mousePos.x < transform.position.x;

        likeHighlight.SetActive(_value);
        dislikeHighlight.SetActive(!_value);
    }

    public virtual void Unfocus(Vector2 _mousePos)
    {
        likeHighlight.SetActive(false);
        dislikeHighlight.SetActive(false);
    }
}