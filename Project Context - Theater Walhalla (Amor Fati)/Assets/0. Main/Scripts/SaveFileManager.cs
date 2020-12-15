using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileData SaveFileData = new SaveFileData();

    public static void SaveGame()
    {
        BinaryFormatter _bf = new BinaryFormatter();
        FileStream _file = File.Create(Application.persistentDataPath + "/SaveFile.dat");

        _bf.Serialize(_file, SaveFileData);
        _file.Close();
    }

    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);

            SaveFileData = (SaveFileData)bf.Deserialize(file);
            file.Close();

            EpisodeManager.Instance.LoadEpisode(SaveFileData.EpisodeIndex);

        }
    }
}

public class SaveFileData
{
    public int EpisodeIndex;
    public int Score;
}
