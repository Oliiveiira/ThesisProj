using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class TherapistPositionChanger : GameData
{
    public TMP_InputField textInput;
    public GameObject textInputObject;
    private IKTargetFollowVRRig player;
    public TextMeshProUGUI timer;
    public GameObject winPanel;
    public bool canSave = true;

    private void Start()
    {
        player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();

        string jsonFilePath = Path.Combine(Application.persistentDataPath, "GameData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myGameData = JsonUtility.FromJson<DataToStore>(jsonText);
    }

    private void Update()
    {
        if (!winPanel.activeInHierarchy)
            player.StartTimer(timer);
        else if (winPanel.activeInHierarchy && canSave)
        {
            canSave = false;
            SaveData();
        }
    }

    public void ChangeCameraPosition(int index) 
    {
        if (index >= 2) 
        {
            textInputObject.SetActive(true);
        }
        else
        {
            textInputObject.SetActive(false);
        }

       // IKTargetFollowVRRig player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
        player.ChangeCameraPosition(index);
    }

    public void ClearArrow()
    {
        //IKTargetFollowVRRig player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
        player.ClearArrowServerRpc();
    }

    public void SendText() 
    {
        //IKTargetFollowVRRig player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
        player.SendMessageToPlayerServerRpc(player.currentIndex - 1, textInput.text);
    }

    public void SaveData()
    {
        myGameData.gameData[myGameData.gameData.Count - 1].time.Add(timer.text);
        myGameData.gameData[myGameData.gameData.Count - 1].level.Add(SceneManager.GetActiveScene().name);
        //myGameData.gameData[myGameData.gameData.Count - 1].player1 = myGameData.gameData[myGameData.gameData.Count - 1].player1;
        //myGameData.gameData[myGameData.gameData.Count - 1].player2 = myGameData.gameData[myGameData.gameData.Count - 1].player2;
        SaveDataToJson();
    }
}
