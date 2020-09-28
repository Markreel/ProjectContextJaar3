using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MediaProjectile : MonoBehaviour, IFocusable
{
    [SerializeField] protected GameObject likeHighlight;
    [SerializeField] private GameObject dislikeHighlight;
    [SerializeField] private Renderer renderer;
    [SerializeField] private TextMeshPro text;

    [HideInInspector] public Material Material;

    private void FixedUpdate()
    {
        //transform.Translate(-transform.forward / 10f * MP_Spawner.Instance.SpeedMultiplier);

        if(transform.position.z < -10) { Destroy(gameObject); }
    }

    public void SetData(Material _mat, string _text)
    {
        renderer.material = Material = _mat;
        text.text = _text;
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