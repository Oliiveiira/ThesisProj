using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RoboRyanTron.Unite2017.Events;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public CanvasGroup cvPauseMenu;
    public Canvas canvas;

    public GameEvent playerFreeze;
    public GameEvent playerUnfreeze;
  //  public GameObject crosshair;

    private void Awake()
    {
        //cvPauseMenu = GetComponentInChildren<CanvasGroup>();
        ResumeGame();
        //crosshair.SetActive(true);
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        cvPauseMenu.alpha = 0;
        cvPauseMenu.blocksRaycasts = false;
        // pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        playerFreeze.Raise();
       // crosshair.SetActive(true);
    }

    private void PauseGame()
    {
        cvPauseMenu.alpha = 1;
        cvPauseMenu.blocksRaycasts = true;
        // pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        playerUnfreeze.Raise();
       // crosshair.SetActive(false);
    }
}
