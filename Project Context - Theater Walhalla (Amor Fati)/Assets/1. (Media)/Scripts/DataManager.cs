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
    public List<List<MediaItem>> MediaItems = new List<List<MediaItem>>();
    private int amountOfMediaItemLists = 4;
    private bool initialized = false;

    public void AddMediaItem(MediaItem _mi)
    {
        //Initialize the media item list
        if (!initialized)
        {
            for (int i = 0; i < amountOfMediaItemLists; i++)
            {
                MediaItems.Add(new List<MediaItem>());
            }

            initialized = true;
        }

        MediaItems[(int)_mi.MediaTopic].Add(_mi);
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
