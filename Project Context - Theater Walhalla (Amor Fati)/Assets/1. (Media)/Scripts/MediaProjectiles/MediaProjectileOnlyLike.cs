using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MediaProjectileOnlyLike : MediaProjectile
{

    public override void Focus(Vector2 _mousePos)
    {
        likeHighlight.SetActive(true);
    }

    public override void Unfocus(Vector2 _mousePos)
    {
        likeHighlight.SetActive(false);
    }
}