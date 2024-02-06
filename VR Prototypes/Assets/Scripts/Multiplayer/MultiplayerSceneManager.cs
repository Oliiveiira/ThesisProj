using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSceneManager : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void NextLevelServerRPC(string nextSceneName)
    {
        NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
}
