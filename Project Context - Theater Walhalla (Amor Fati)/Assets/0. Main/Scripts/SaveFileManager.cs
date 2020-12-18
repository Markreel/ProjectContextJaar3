using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileData SaveFileData = new SaveFileData();

    public static void SaveData()
    {
        BinaryFormatter _bf = new BinaryFormatter();
        FileStream _file = File.Create(Application.persistentDataPath + "/SaveFile.dat");

        _bf.Serialize(_file, SaveFileData);
        _file.Close();
    }

    public static void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);

            SaveFileData = (SaveFileData)bf.Deserialize(file);
            file.Close();
        }
    }
}

[System.Serializable]
public class SaveFileData
{
    public int EpisodeIndex = 0;
    public int Score = 0;
    public float Volume = 1;
}
