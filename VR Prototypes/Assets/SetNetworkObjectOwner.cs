using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SetNetworkObjectOwner : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void SetClientOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetOwner();
            Debug.Log("TRY TO CHANGE OWNERsHIP");
        }
    }

    public void SetOwner()
    {
        SetClientOwnershipServerRPC();
    }
}
