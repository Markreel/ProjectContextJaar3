using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaItem
{
    public Material Material;
    public bool Liked;

    public MediaItem(Material _mat, bool _liked)
    {
        Material = _mat;
        Liked = _liked;
    }
}
