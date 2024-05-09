using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelsDataWritter : CustomLevelsData
{
    private void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        myLevelData.levelData.RemoveRange(0, myLevelData.levelData.Count);
        SaveDataToJson();
        //AddAnotherLevel();
        //SaveDataToJson();
    }

    public void AddAnotherLevel()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        //Debug.Log("Data count before adding: " + myGameData.gameData.Count);
        myLevelData.levelData.Add(new LevelsData());

        Debug.Log("Data count before adding: " + myLevelData.levelData.Count);
    }

    public void SaveDataToJson()
    {
        string json = JsonUtility.ToJson(myLevelData);
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        File.WriteAllText(jsonFilePath, json);
    }

    public void RemoveDataAtIndex(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < myLevelData.levelData.Count)
        {
            // Remove the element at the specified index
            myLevelData.levelData.RemoveAt(index);

            // Serialize and save the updated data to the JSON file
            SaveDataToJson();
        }
    }
}
