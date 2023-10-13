using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public ActiveStateSelector restartPose;
    public ActiveStateSelector backToMenuPose;
    public ActiveStateSelector restartPoseL;
    public ActiveStateSelector backToMenuPoseL;

    public bool canRestart;
    public bool canGoBackToMenu;

    public bool canRestartL;
    public bool canGoBackToMenuL;

    //Restart Timer
    private float currentRestartTime = 0f;
    private float startingRestartTime = 5f;

    //Restart Timer
    private float currentMenuTime = 0f;
    private float startingMenuTime = 5f;

    private bool startTimer = false;

    [SerializeField]
    private TMP_Text restartCountdownText;
    [SerializeField]
    private TMP_Text menuCountdownText;

    [SerializeField]
    private TMP_Text restartCountdownTextL;
    [SerializeField]
    private TMP_Text menuCountdownTextL;

    [SerializeField]
    private GameObject restartTimerPanel;
    [SerializeField]
    private GameObject menuTimerPanel;

    [SerializeField]
    private GameObject restartTimerPanelL;
    [SerializeField]
    private GameObject menuTimerPanelL;

    [SerializeField]
    private GameManager mirror;

    // Start is called before the first frame update
    void Start()
    {
        currentRestartTime = startingRestartTime;
        currentMenuTime = startingMenuTime;

        restartPose.WhenSelected += () => canRestart = true;
        restartPose.WhenUnselected += () => canRestart = false;
        restartPoseL.WhenSelected += () => canRestartL = true;
        restartPoseL.WhenUnselected += () => canRestartL = false;

        backToMenuPose.WhenSelected += () => canGoBackToMenu = true;
        backToMenuPose.WhenUnselected += () => canGoBackToMenu = false;
        backToMenuPoseL.WhenSelected += () => canGoBackToMenuL = true;
        backToMenuPoseL.WhenUnselected += () => canGoBackToMenuL = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mirror.mirrorLeft && !mirror.mirrorRight)
        {
            if (canRestart)
            {
                restartTimerPanel.SetActive(true);
                StartRestartTimer();
            }
            else
            {
                restartTimerPanel.SetActive(false);
                currentRestartTime = startingRestartTime;
            }

            if (canGoBackToMenu)
            {
                menuTimerPanel.SetActive(true);
                ReturnToMenu();
            }
            else
            {
                menuTimerPanel.SetActive(false);
                currentMenuTime = startingMenuTime;
            }

            if (canRestartL)
            {
                restartTimerPanelL.SetActive(true);
                StartRestartTimer();
            }
            else
            {
                restartTimerPanelL.SetActive(false);
                currentRestartTime = startingRestartTime;
            }

            if (canGoBackToMenuL)
            {
                menuTimerPanelL.SetActive(true);
                ReturnToMenu();
            }
            else
            {
                menuTimerPanelL.SetActive(false);
                currentMenuTime = startingMenuTime;
            }
        }
        else if (mirror.mirrorLeft == true)
        {
            if (canRestart)
            {
                restartTimerPanelL.SetActive(true);
                StartRestartTimer();
            }
            else
            {
                restartTimerPanelL.SetActive(false);
                currentRestartTime = startingRestartTime;
            }

            if (canGoBackToMenu)
            {
                menuTimerPanelL.SetActive(true);
                ReturnToMenu();
            }
            else
            {
                menuTimerPanelL.SetActive(false);
                currentMenuTime = startingMenuTime;
            }
        }
        else if (mirror.mirrorRight == true)
        {
            if (canRestartL)
            {
                restartTimerPanel.SetActive(true);
                StartRestartTimer();
            }
            else
            {
                restartTimerPanel.SetActive(false);
                currentRestartTime = startingRestartTime;
            }

            if (canGoBackToMenuL)
            {
                menuTimerPanel.SetActive(true);
                ReturnToMenu();
            }
            else
            {
                menuTimerPanel.SetActive(false);
                currentMenuTime = startingMenuTime;
            }
        }
    }

    void StartRestartTimer()
    {
        currentRestartTime -= 1 * Time.deltaTime;
        restartCountdownText.text = currentRestartTime.ToString("0");
        //print(currentTime);

        if (currentRestartTime <= 0)
        {
            ResetScene();
        }
    }

    void ReturnToMenu()
    {
        currentMenuTime -= 1 * Time.deltaTime;
        menuCountdownText.text = currentMenuTime.ToString("0");
        //print(currentTime);

        if (currentMenuTime <= 0)
        {
            GoToMenuScene();
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
