using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public MediaData MediaData;

    private void Awake()
    {
        Instance = this;
    }
}

public class MediaData
{
    public List<MediaItem> MediaItems = new List<MediaItem>();
}

public class JezelfData
{
    
}
