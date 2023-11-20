using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using RoboRyanTron.Unite2017.Events;
using System.IO;

public class PuzzleClientSender : NetworkBehaviour
{
    public string jsonFilePath = "path/to/your/data.json";

    public Button sendButtonPrefab; // Reference to the button prefab

    private Button sendButton; // Reference to the instantiated button

    public Button startButtonPrefab; // Reference to the button prefab

    private Button startButton; // Reference to the instantiated button
    [SerializeField]
    private GameEvent startPuzzleGame;
    [SerializeField]
    private GameEvent deserializeJson;

    private void Awake()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, "ImageLinks.txt");
    }

    private void Start()
    {
        // Find the Canvas GameObject by its name (adjust the name as needed)
        GameObject canvasObject = GameObject.Find("StartLevelScreen");

        // Check if the Canvas GameObject is found
        if (canvasObject != null)
        {
            // Get the Canvas component
            RectTransform canvas = canvasObject.GetComponent<RectTransform>();

            // Instantiate the button prefab as a child of the Canvas
            sendButton = Instantiate(sendButtonPrefab, canvas.transform);
            startButton = Instantiate(startButtonPrefab, canvas.transform);
            startButton.gameObject.SetActive(false);
            // Add a listener to the button click event
            sendButton.onClick.AddListener(SendJsonDataToServer);
            sendButton.onClick.AddListener(ActivateStartButton);
            startButton.onClick.AddListener(StartGameServerRpc);
        }
        else
        {
            Debug.LogError("Canvas not found. Make sure the Canvas GameObject is present in the scene.");
        }

    }

    // Method to be called when the button is clicked
    private void SendJsonDataToServer()
    {
        // Read JSON data from the file
        string jsonData = File.ReadAllText(jsonFilePath);
        // Send JSON data to the server using a ServerRpc
        SendDataServerRpc(jsonData);
    }

    private void ActivateStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    [ServerRpc]
    private void SendDataServerRpc(string jsonData)
    {
        // Receive JSON data on all clients
        // You can process or display the data as needed
        Debug.Log("Received JSON data on client: " + jsonData);
        File.WriteAllText(jsonFilePath, jsonData);
        deserializeJson.Raise();
        //startGame.Raise();
    }

    [ServerRpc]
    private void StartGameServerRpc()
    {
        File.ReadAllText(jsonFilePath);
        Debug.Log("Working");
        startPuzzleGame.Raise();
    }
}
