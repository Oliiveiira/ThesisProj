using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomMultiplayerSceneManager : CustomLevelsData
{
    private bool listPositionRemoved;
    private string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRPC()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        SendDataClientRpc(jsonText);
        NetworkManager.SceneManager.LoadScene(myLevelData.levelData[0].level, LoadSceneMode.Single);
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextLevelServerRPC()
    {
        Debug.Log("NextLevelRPC");
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);
        if (!listPositionRemoved)
        {
            RemoveDataAtIndex(0);
            listPositionRemoved = true;
            Debug.Log("NextLevelRPCRemoved");
        }
        if (myLevelData.levelData.Count > 0)
            nextSceneName = myLevelData.levelData[0].level;
        else
            nextSceneName = "CustomMultiplayerHub";

        jsonText = File.ReadAllText(jsonFilePath);
        SendDataClientRpc(jsonText);
        NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    [ClientRpc]
    public void SendDataClientRpc(string jsonData)
    {
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt"), jsonData);
    }
}
