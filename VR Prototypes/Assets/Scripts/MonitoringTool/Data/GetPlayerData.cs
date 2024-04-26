using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GetPlayerData : GameData
{
    public TextMeshProUGUI player1;
    public TextMeshProUGUI player2;

    private void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        //if (myGameData.gameData.Count > 0)
        //{
            myGameData = JsonUtility.FromJson<DataToStore>(jsonText);
       // }

    }

    public void SetPlayerName()
    {
        AddAnotherGameSession();
        myGameData.gameData[myGameData.gameData.Count - 1].date = DateTime.Now.ToString();
        myGameData.gameData[myGameData.gameData.Count - 1].player1 = player1.text;
        myGameData.gameData[myGameData.gameData.Count - 1].player2 = player2.text;
        SaveDataToJson();
    }
}
