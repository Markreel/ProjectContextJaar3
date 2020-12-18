using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LerpSpriteColor : LerpBaseClass
{
    [SerializeField] Color colorA;
    [SerializeField] Color colorB;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Lerp(float _t)
    {
        spriteRenderer.color = Color.Lerp(colorA, colorB, _t);
    }
}