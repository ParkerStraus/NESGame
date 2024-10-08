using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{

    public static void SaveLevelProgress(int level, bool progress)
    {
        SaveData save = LoadData();

        save.LevelCompletion[level] = progress;

        SaveData(save);
    }

    public static void SaveData(SaveData save)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save.data";
        Debug.Log("Now saving at " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static SaveData InitData()
    {
        
        SaveData save = new SaveData(new bool[5]);
        for (int i = 0; i < save.LevelCompletion.Length; i++)
        {
            save.LevelCompletion[i] = false;
        }
        SaveData(save);
        return save;
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/save.data";
        Debug.Log("Now loading at " + path);
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No save found providing new data");
            SaveData save = InitData();
            return save;
        }
    }
    public static bool CheckData()
    {
        string path = Application.persistentDataPath + "/save.data";
        Debug.Log("Now loading at " + path);
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
