using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.SceneManagement;

public class ActivateMirror : MonoBehaviour
{
    [SerializeField]
    private FloatSO mirror;
    [SerializeField]
    private GameEvent mirrorLeft;

    [SerializeField]
    private GameEvent mirrorRight;

    [SerializeField]
    private GameEvent mirrorNone;

    public GameManager gameManager;
    //private void Awake()
    //{
    //    if(mirror.Value == 1)
    //    {
    //        mirrorLeft.Raise();
    //    }
    //    else if(mirror.Value == 2)
    //    {
    //        mirrorRight.Raise();
    //    }
    //}

    public void MirrorLeft()
    {
       // mirror.Value = 1;
        mirrorLeft.Raise();
        gameManager.mirrorLeft = true;
        gameManager.mirrorRight = false;
    }

    public void MirrorRight()
    {
       // mirror.Value = 2;  
        mirrorRight.Raise();
        gameManager.mirrorRight = true;
        gameManager.mirrorLeft = false;
    }

    public void MirrorNone()
    {
        // mirror.Value = 2;  
        mirrorNone.Raise();
        gameManager.mirrorRight = false;
        gameManager.mirrorLeft = false;
    }



    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
