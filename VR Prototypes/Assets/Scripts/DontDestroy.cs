using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //if (SceneManager.GetActiveScene().name == "Scene1"){
        //    transform.position = new Vector3(2, 0, 0);
        //}
        transform.position = new Vector3(2, 0, 0);
    }
}
