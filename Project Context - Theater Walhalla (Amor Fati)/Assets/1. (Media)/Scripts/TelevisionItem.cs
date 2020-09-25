using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelevisionItem : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] public bool TurnedOn = true;
    public MediaItem mediaItem;
    public string tvName;

    public void SetScreen(int index)
    {
        mediaItem = DataManager.Instance.MediaData.MediaItems[index];
        renderer.material = mediaItem.Material; 
    }
}