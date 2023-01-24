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
    }

    public void MirrorRight()
    {
       // mirror.Value = 2;  
        mirrorRight.Raise();
    }

    public void MirrorNone()
    {
        // mirror.Value = 2;  
        mirrorNone.Raise();
    }



    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
