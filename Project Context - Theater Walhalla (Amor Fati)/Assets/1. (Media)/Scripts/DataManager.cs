using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public MediaData MediaData = new MediaData();

    private void Awake()
    {
        Instance = this;
    }
}

public class MediaData
{
    public List<MediaItem> MediaItems = new List<MediaItem>();

    public void AddMediaItem(MediaItem _mi)
    {
        MediaItems.Add(_mi);
    }
}

public class MaatschappijData
{

}

public class JezelfData
{
    
}

public class LiefdeData
{

}

public class KlimaatveranderingData
{

}

public class DoodData
{

}
