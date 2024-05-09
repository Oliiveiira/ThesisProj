using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AddGameLevelMultiplayer : LevelsDataWritter
{
    private void Start()
    {

    }

    public void AddGameLevel(string gameLevel)
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        AddAnotherLevel();
        myLevelData.levelData[^1].level = gameLevel;
        SaveDataToJson();
    }
}
