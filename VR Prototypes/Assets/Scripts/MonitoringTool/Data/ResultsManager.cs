using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResultsManager : GameData
{
    public int positionList;
    public int positionInnerList;
    public int blockId;
    private ResultsUI positionsToOrder;

    private void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myGameData = JsonUtility.FromJson<DataToStore>(jsonText);
        positionsToOrder = transform.parent.parent.parent.GetComponent<ResultsUI>();
    }

    public void RemoveEntry()
    {
        // Check if the index is valid
        if (positionList >= 0)
        {
            Debug.Log("Aqui 1");
            if(positionInnerList > 0)
            {
                string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
                string jsonText = File.ReadAllText(jsonFilePath);
                myGameData = JsonUtility.FromJson<DataToStore>(jsonText);

                Debug.Log("EntrouLista");
                myGameData.gameData[positionList].time.RemoveAt(positionInnerList);
                myGameData.gameData[positionList].level.RemoveAt(positionInnerList);
                SaveDataToJson();
                positionsToOrder.GetPositions(positionList,blockId);
                Destroy(gameObject);
            }
            else if (positionInnerList == 0 && myGameData.gameData[positionList].time.Count > 1)
            {
                string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
                string jsonText = File.ReadAllText(jsonFilePath);
                myGameData = JsonUtility.FromJson<DataToStore>(jsonText);

                myGameData.gameData[positionList].time.RemoveAt(positionInnerList);
                myGameData.gameData[positionList].level.RemoveAt(positionInnerList);
                SaveDataToJson();
                positionsToOrder.GetPositions(positionList,blockId);
                Destroy(gameObject);
            }
            else if(positionInnerList == 0 && myGameData.gameData[positionList].time.Count == 1)
            {
                Debug.Log("Aqui");
                string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
                string jsonText = File.ReadAllText(jsonFilePath);
                myGameData = JsonUtility.FromJson<DataToStore>(jsonText);
                RemoveDataAtIndex(positionList);
                positionsToOrder.ArrangeListPositions(positionList, blockId);
                Destroy(gameObject);
            }
            // Serialize and save the updated data to the JSON file
            //SaveDataToJson();
        }
    }

    void Update()
    {
        //Sort positions from array
    }
}
