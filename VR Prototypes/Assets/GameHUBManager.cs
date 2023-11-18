using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class GameHUBManager : NetworkBehaviour 
{
    // Start is called before the first frame update
    void Start()
    {
        ShutdownServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void ShutdownServer()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }
        Cleanup();
    }

    void Cleanup()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }

    public void StartMiniGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
