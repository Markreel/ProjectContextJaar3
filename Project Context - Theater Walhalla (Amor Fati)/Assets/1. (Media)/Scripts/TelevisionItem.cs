using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelevisionItem : MonoBehaviour
{
    [SerializeField] public Renderer Renderer;
    [SerializeField] public bool TurnedOn = true;
    [SerializeField] public MediaTopic MediaTopic;
    public MediaItem mediaItem;
    public string tvName;

    public void SetScreen(MediaItem _mi)
    {
        mediaItem = _mi;
        Renderer.material = mediaItem.Material; 
    }
}