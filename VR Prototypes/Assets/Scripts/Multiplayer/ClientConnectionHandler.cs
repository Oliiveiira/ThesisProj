using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientConnectionHandler : NetworkBehaviour
{
    public List<uint> AlternatePlayerPrefabs;

    public void SetClientPlayerPrefab(int index)
    {
        if (index > AlternatePlayerPrefabs.Count)
        {
            Debug.LogError($"Trying to assign player Prefab index of {index} when there are only {AlternatePlayerPrefabs.Count} entries!");
            return;
        }
        if (NetworkManager.IsListening || IsSpawned)
        {
            Debug.LogError("This needs to be set this before connecting!");
            return;
        }
        NetworkManager.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(index);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApprovalCallback;
            print("spawned");
        }
    }

    private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        print("entrou aqui");
        response.PlayerPrefabHash = 397963804;

        var playerPrefabIndex = System.BitConverter.ToInt32(request.Payload);
        if (AlternatePlayerPrefabs.Count > playerPrefabIndex)
        {
            response.PlayerPrefabHash = 3079346999;
        }
        else
        {
            Debug.LogError($"Client provided player Prefab index of {playerPrefabIndex} when there are only {AlternatePlayerPrefabs.Count} entries!");
            return;
        }

        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
        //response.PlayerPrefabHash = null;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        response.Reason = "Some reason for not approving the client";

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
        // Continue filling out the response
        print("aqui");
    }
}

    //public List<GameObject> alternatePlayerPrefabs;
    //private Dictionary<uint, GameObject> prefabDictionary = new Dictionary<uint, GameObject>();
    //[SerializeField]
    //private bool firstPlayerSpawned;

    //private void Start()
    //{
    //    // Populate the dictionary with prefab hashes and corresponding prefabs
    //    foreach (GameObject prefab in alternatePlayerPrefabs)
    //    {
    //        NetworkObject networkObject = prefab.GetComponent<NetworkObject>();
    //        if (networkObject != null)
    //        {
    //            prefabDictionary.Add(networkObject.PrefabIdHash, prefab);
    //            Debug.Log(networkObject.PrefabIdHash);
    //        }
    //    }
    //}

    //public override void OnNetworkSpawn()
    //{
    //    if (IsServer)
    //    {
    //        NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;
    //        print("Spawned");
    //    }
    //}

    ////private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    ////{
    ////    // Extract the player prefab index from the connection payload
    ////    int playerPrefabIndex = System.BitConverter.ToInt32(request.Payload);

    ////    // Check if the index is within bounds
    ////    if (playerPrefabIndex >= 0 && playerPrefabIndex < alternatePlayerPrefabs.Count)
    ////    {
    ////        // Set the player prefab hash based on the index
    ////        response.PlayerPrefabHash = alternatePlayerPrefabs[playerPrefabIndex].GetComponent<NetworkObject>().PrefabIdHash;
    ////    }
    ////    else
    ////    {
    ////        Debug.LogError($"Invalid player prefab index: {playerPrefabIndex}");
    ////    }
    ////}

    //private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    //{
    //    print("entrou aqui");
    //    // If the first player hasn't been spawned yet, choose a specific prefab for it
    //    if (!firstPlayerSpawned)
    //    {
    //        // Set the prefab hash for the first player
    //        response.PlayerPrefabHash = 3079346999;
    //        print("entrou aqui");
    //        firstPlayerSpawned = true;
    //    }
    //    else
    //    {
    //        // Extract the player prefab index from the connection payload
    //        int playerPrefabIndex = System.BitConverter.ToInt32(request.Payload);

    //        // Check if the index is within bounds
    //        if (playerPrefabIndex >= 0 && playerPrefabIndex < alternatePlayerPrefabs.Count)
    //        {
    //            // Set the player prefab hash based on the index
    //            response.PlayerPrefabHash = alternatePlayerPrefabs[playerPrefabIndex].GetComponent<NetworkObject>().PrefabIdHash;
    //        }
    //        else
    //        {
    //            Debug.LogError($"Invalid player prefab index: {playerPrefabIndex}");
    //        }
    //    }
    //}

    //public GameObject[] players;

    //public override void OnNetworkSpawn()
    //{
    //    GameObject player = Instantiate(players[1]);
    //    NetworkObject playerNetwork = player.GetComponent<NetworkObject>();
    //    playerNetwork.Spawn();
    //    //playerNetwork.TrySetParent(transform, true);
    //    NetworkManager.GetNetworkPrefabOverride(player);
    //}
    //public List<uint> AlternatePlayerPrefabs;

    //public void SetClientPlayerPrefab(int index)
    //{
    //    if (index < 0 || index >= AlternatePlayerPrefabs.Count)
    //    {
    //        Debug.LogError($"Invalid player Prefab index: {index}. Index should be between 0 and {AlternatePlayerPrefabs.Count - 1} inclusive.");
    //        return;
    //    }

    //    if (NetworkManager.Singleton.IsListening || IsSpawned)
    //    {
    //        Debug.LogError("This needs to be set before connecting!");
    //        return;
    //    }

    //    NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(index);
    //}

    //public override void OnNetworkSpawn()
    //{
    //    if (IsServer)
    //    {
    //        NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApprovalCallback;
    //    }
    //}

    //private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    //{
    //    var playerPrefabIndex = System.BitConverter.ToInt32(request.Payload);
    //    if (playerPrefabIndex >= 0 && playerPrefabIndex < AlternatePlayerPrefabs.Count)
    //    {
    //        response.PlayerPrefabHash = AlternatePlayerPrefabs[playerPrefabIndex];
    //    }
    //    else
    //    {
    //        Debug.LogError($"Client provided invalid player Prefab index: {playerPrefabIndex}. Index should be between 0 and {AlternatePlayerPrefabs.Count - 1} inclusive!");
    //        return;
    //    }
    //    // Continue filling out the response
    //}
//}
