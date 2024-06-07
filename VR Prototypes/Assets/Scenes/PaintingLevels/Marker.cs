using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Marker : NetworkBehaviour
{
    private Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pallete"))
        {
            Debug.Log("PickColor");
            Renderer palleteRenderer = other.GetComponent<Renderer>();
            render.material.color = palleteRenderer.material.color;
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SetClientOwnershipServerRPC();
        //}
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            transform.parent.GetComponent<NetworkObject>().ChangeOwnership(clientId);
            Debug.Log("Client is now the owner");
            Debug.Log(clientId);
        }
        else
        {
            Debug.Log("not working");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientHandMarkerOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
            Debug.Log("Client is now the owner");
            Debug.Log(clientId);
        }
        else
        {
            Debug.Log("not working");
        }
    }
}
