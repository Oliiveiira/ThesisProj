using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public string date;
        public List<string> time;
        public List<string> level;
        public string player1;
        public string player2;
        public List<int> errorCount;
    }

    [System.Serializable]
    public class DataToStore
    {
        public List<Data> gameData;
    }

    public DataToStore myGameData = new DataToStore();

    public void AddAnotherGameSession()
    {
        //Debug.Log("Data count before adding: " + myGameData.gameData.Count);
        myGameData.gameData.Add(new Data());

        Debug.Log("Data count before adding: " + myGameData.gameData.Count);
    }

    public void SaveDataToJson()
    {
        string json = JsonUtility.ToJson(myGameData);
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        File.WriteAllText(jsonFilePath, json);
    }

    public void RemoveDataAtIndex(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < myGameData.gameData.Count)
        {
            // Remove the element at the specified index
            myGameData.gameData.RemoveAt(index);

            // Serialize and save the updated data to the JSON file
            SaveDataToJson();
        }
    }
}
