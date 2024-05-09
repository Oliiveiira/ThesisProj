using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;

public class CustomLevelsData : NetworkBehaviour
{
    [System.Serializable]
    public class LevelsData
    {
        public string level;
        //SupermarketInfo
        public string recipeName;
        public List<string> ingredientsName;
        public List<string> ingredientsPath;
        
        //Puzzle/Pairs/PlaceIngredientes Info
        public string textureURL;
        public int numberOfCubes;
        public float difficultyValue;
    }

    [System.Serializable]
    public class MultiplayerLevelsData
    {
        public List<LevelsData> levelData;
    }

    public MultiplayerLevelsData myLevelData = new MultiplayerLevelsData();

    public void AddAnotherLevel()
    {
        Debug.Log("Data count before adding: " + myLevelData.levelData.Count);
        myLevelData.levelData.Add(new LevelsData());

        Debug.Log("Data count after adding: " + myLevelData.levelData.Count);
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

    public void SaveDataToJson()
    {
        string json = JsonUtility.ToJson(myLevelData);
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        File.WriteAllText(jsonFilePath, json);
    }
}
