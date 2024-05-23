using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetErrorCountData : GameData
{
    // Start is called before the first frame update
    void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myGameData = JsonUtility.FromJson<DataToStore>(jsonText);
    }

    public void SaveData(int numberOfErrors)
    {
        myGameData.gameData[myGameData.gameData.Count - 1].errorCount.Add(numberOfErrors);
        SaveDataToJson();
    }
}
