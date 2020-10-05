using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MediaTopic { BLM, Corona, ZwartePiet, Privacy }

public class MediaItem
{
    public Material Material;
    public bool Liked;
    public MediaTopic MediaTopic;

    public MediaItem(Material _mat, bool _liked, MediaTopic _mediaTopic)
    {
        Material = _mat;
        Liked = _liked;
        MediaTopic = _mediaTopic;
    }
}
