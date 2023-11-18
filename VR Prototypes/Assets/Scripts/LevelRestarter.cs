using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
    public ActiveStateSelector restartPose;
    public ActiveStateSelector backToMenuPose;
    public bool canRestart;
    public bool canGoBackToMenu;


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
    private GameObject restartTimerPanel;
    [SerializeField]
    private GameObject menuTimerPanel;

    // Start is called before the first frame update
    void Start()
    {
        currentRestartTime = startingRestartTime;
        currentMenuTime = startingMenuTime;

        restartPose.WhenSelected += () => canRestart = true;
        restartPose.WhenUnselected += () => canRestart = false;

        backToMenuPose.WhenSelected += () => canGoBackToMenu = true;
        backToMenuPose.WhenUnselected += () => canGoBackToMenu = false;
    }

    // Update is called once per frame
    void Update()
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
        SceneManager.LoadScene("GameHUB");
    }
}
