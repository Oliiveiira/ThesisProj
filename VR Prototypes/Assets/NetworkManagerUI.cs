using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField]
    private string insertedAdress = null;
   // private string insertedAdress = "192.168.1.70";
    public TMP_InputField textInput;


    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            SetIpAdress();
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            SetIpAdress();
            NetworkManager.Singleton.StartClient();
        });

      
    }

    public void SetIpAdress()
    {
        UnityTransport ipAdress = NetworkManager.Singleton.GetComponent<UnityTransport>();
        ipAdress.ConnectionData.Address = insertedAdress;
    }

    public void KeyLetter(string letter)
    {
        insertedAdress = insertedAdress + letter;
        textInput.text = insertedAdress;
    }

    public void EraseLastChar()
    {
        if (textInput.text.Length > 0)
        {
            textInput.text = textInput.text.Substring(0, textInput.text.Length - 1);
            insertedAdress = insertedAdress.Substring(0, insertedAdress.Length - 1);
        }
    }
}