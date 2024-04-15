using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class LocalNetworkManager : MonoBehaviour
{
 
    [SerializeField]
    private string insertedAdress = null;
    // private string insertedAdress = "192.168.1.70";
    public TMP_InputField textInput;

    public NetworkConnect networkConnect;


    //private void Awake()
    //{
    //    serverBtn.onClick.AddListener(() =>
    //    {
    //        NetworkManager.Singleton.StartServer();
    //    });
    //    hostBtn.onClick.AddListener(() =>
    //    {
    //        SetIpAdress();
    //        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(networkConnect.prefabNumber);
    //        NetworkManager.Singleton.ConnectionApprovalCallback = networkConnect.ConnectionApprovalCallback;
    //        NetworkManager.Singleton.StartHost();
    //    });
    //    clientBtn.onClick.AddListener(() =>
    //    {
    //        SetIpAdress();
    //        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(networkConnect.prefabNumber);
    //        NetworkManager.Singleton.StartClient();
    //    });
    //}
    public void ConnectHost()
    {
        SetIpAdress("192.168.1.72");
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(networkConnect.prefabNumber);
        NetworkManager.Singleton.ConnectionApprovalCallback = networkConnect.ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    public void ConnectClient()
    {
        SetIpAdress("192.168.1.72");
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(networkConnect.prefabNumber);
        NetworkManager.Singleton.StartClient();
    }

    public void SetIpAdress(string adress)
    {
        UnityTransport ipAdress = NetworkManager.Singleton.GetComponent<UnityTransport>();
        //ipAdress.ConnectionData.Address = textInput.text;
        ipAdress.SetConnectionData(adress, 7777);
    }

}
