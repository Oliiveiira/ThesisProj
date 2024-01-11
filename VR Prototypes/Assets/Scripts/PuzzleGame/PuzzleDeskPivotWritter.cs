using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PuzzleDeskPivotWritter : PuzzleDeskPivotReader
{
    void Awake()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "PuzzleDeskPivot.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myDeskPivot = JsonUtility.FromJson<DeskPivot>(jsonText);
    }

    public void SavePivotToJson()
    {
        string json = JsonUtility.ToJson(myDeskPivot);
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "PuzzleDeskPivot.txt");
        File.WriteAllText(jsonFilePath, json);
    }
}
