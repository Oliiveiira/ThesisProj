using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ResultsUI : GameData
{
    public GameObject infoTemplate;
    public Transform content;
    public List<GameObject> resultTemplates; 

    // Start is called before the first frame update
    void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myGameData = JsonUtility.FromJson<DataToStore>(jsonText);

        int id = 0;
            for (int j = 0; j < myGameData.gameData.Count; j++)
            {
            Debug.Log(j);
                if(myGameData.gameData[j].time.Count >= 1)
                {
                    for (int i = 0; i < myGameData.gameData[j].time.Count; i++)
                    {
                        GameObject result = Instantiate(infoTemplate, content);
                        resultTemplates.Add(result);
                        ResultsManager resultManager = result.GetComponent<ResultsManager>();
                        resultManager.positionList = j;
                        resultManager.positionInnerList = i;
                        resultManager.blockId = id;
                        //Debug.Log(resultManager.positionList);
                        //Debug.Log(resultManager.positionInnerList);

                        result.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Jogador 1: " + myGameData.gameData[j].player1;
                        result.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Jogador 2: " + myGameData.gameData[j].player2;
                        result.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Tempo: " + myGameData.gameData[j].time[i];
                        result.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = myGameData.gameData[j].date;
                        result.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Nivel: " + myGameData.gameData[j].level[i];

                        id++;
                    }
                }
                else
                {
                RemoveDataAtIndex(j);
                j--;
                }
            }
        Destroy(infoTemplate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetPositions(0,0);
        }
    }

    public void GetPositions(int listPosition, int id)
    {
        resultTemplates.RemoveAt(id);
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myGameData = JsonUtility.FromJson<DataToStore>(jsonText);

        if(listPosition == 0)
        {
            for (int i = 0; i < myGameData.gameData[listPosition].time.Count; i++)
            {
                Debug.Log(myGameData.gameData[listPosition].time.Count);
                ResultsManager resultsManager = resultTemplates[i].GetComponent<ResultsManager>();
                resultsManager.positionInnerList = i;
                //resultsManager.blockId = i;
            }

        }
        else if (listPosition > 0)
        {
            Debug.Log("Entriuuuuu");
            for (int i = 0; i < myGameData.gameData[listPosition].time.Count; i++)
            {
                if (id < resultTemplates.Count)
                {
                    //Debug.Log(myGameData.gameData[listPosition].time.Count);
                    //Debug.Log(id);
                    ResultsManager resultsManager = resultTemplates[id].GetComponent<ResultsManager>();
                    if(resultsManager.positionList == listPosition)
                        resultsManager.positionInnerList--;
                    //resultsManager.blockId = i;
                    id++;
                }
                else
                {
                    for (int j = 0; j < resultTemplates.Count; j++)
                    {
                        ResultsManager resultsManager = resultTemplates[j].GetComponent<ResultsManager>();
                        resultsManager.blockId = j;
                    }
                    foreach (GameObject result in resultTemplates)
                    {
                        ResultsManager resultManager = result.GetComponent<ResultsManager>();
                        resultManager.myGameData = GetComponent<ResultsUI>().myGameData;
                    }
                    return;
                }

            }
        }

        for (int i = 0; i < resultTemplates.Count; i++)
        {
            ResultsManager resultsManager = resultTemplates[i].GetComponent<ResultsManager>();
            resultsManager.blockId = i;
        }

        foreach (GameObject result in resultTemplates)
        {
            ResultsManager resultManager = result.GetComponent<ResultsManager>();
            resultManager.myGameData = GetComponent<ResultsUI>().myGameData;
        }
    }

    public void ArrangeListPositions(int listPosition, int id)
    {
        resultTemplates.RemoveAt(id);
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myGameData = JsonUtility.FromJson<DataToStore>(jsonText);

        if (myGameData.gameData.Count >= 1 && listPosition == 0)
        {
            Debug.Log("Here2");
            for (int i = 0; i < resultTemplates.Count; i++)
            {
                ResultsManager resultsManager = resultTemplates[i].GetComponent<ResultsManager>();
                resultsManager.positionList --;
                resultsManager.blockId = i;
                resultsManager.myGameData = GetComponent<ResultsUI>().myGameData;
            }
        } 
        else if (myGameData.gameData.Count >= 1 && listPosition != 0)
        {
            Debug.Log("Here");
            for (int i = 0; i < resultTemplates.Count; i++)
            {
                ResultsManager resultsManager = resultTemplates[i].GetComponent<ResultsManager>();
                resultsManager.blockId = i;
                if (i >= id)
                {
                    resultsManager.positionList--;
                }
                resultsManager.myGameData = GetComponent<ResultsUI>().myGameData;
            }
        }
    }

}

