using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool[] LevelCompletion;

    public SaveData(bool[] progress)
    {
        LevelCompletion = progress;
    }
}
