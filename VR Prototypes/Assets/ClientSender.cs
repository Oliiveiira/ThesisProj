using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using RoboRyanTron.Unite2017.Events;

public class ClientSender : NetworkBehaviour
{
    public string jsonFilePath = "path/to/your/data.json";
    public Button sendButtonPrefab; // Reference to the button prefab

    private Button sendButton; // Reference to the instantiated button
    [SerializeField]
    private GameEvent startGame;

    private void Start()
    {
        // Find the Canvas GameObject by its name (adjust the name as needed)
        GameObject canvasObject = GameObject.Find("Canvas");

        // Check if the Canvas GameObject is found
        if (canvasObject != null)
        {
            // Get the Canvas component
            Canvas canvas = canvasObject.GetComponent<Canvas>();

            // Instantiate the button prefab as a child of the Canvas
            sendButton = Instantiate(sendButtonPrefab, canvas.transform);

            // Add a listener to the button click event
            sendButton.onClick.AddListener(SendJsonDataToServer);
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
        string jsonData = System.IO.File.ReadAllText(jsonFilePath);

        // Send JSON data to the server using a ServerRpc
        SendDataServerRpc(jsonData);
    }

    [ServerRpc]
    private void SendDataServerRpc(string jsonData)
    {
        // Receive JSON data on all clients
        // You can process or display the data as needed
        Debug.Log("Received JSON data on client: " + jsonData);
        startGame.Raise();
    }
}