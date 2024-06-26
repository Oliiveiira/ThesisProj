using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SetNetworkObjectOwner : NetworkBehaviour
{
    public float syncInterval = 0.05f; // Sync every 0.05 seconds (20 times per second)
    private float lastSyncTime;

    public NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public bool isSending = false;

    private void Update()
    {
        if (IsOwner)
        {
            //if (Time.time - lastSyncTime >= syncInterval)
            //{
                Debug.Log("Sending Vector3");
                networkedPosition.Value = transform.position;
                //lastSyncTime = Time.time;
            //}
        }
        else if(!IsOwner)
        {
            transform.position = networkedPosition.Value;
        }
    }

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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SetOwner();
    //        Debug.Log("TRY TO CHANGE OWNERsHIP");
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (IsOwner)
        {
            if (other.gameObject.CompareTag("Canvas"))
            {
                isSending = true;
                Debug.Log("Collided");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOwner)
        {
            if (other.gameObject.CompareTag("Canvas"))
            {
                isSending = false;
                Debug.Log("Collided");
            }
        }
    }

    public void SetOwner()
    {
        SetClientOwnershipServerRPC();
    }
}
