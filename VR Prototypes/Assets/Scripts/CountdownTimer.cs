using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    private float currentTime = 0f;
    private float startingTime = 10f;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject transitionPanel;
    [SerializeField]
    private bool startTimer = false;
    [SerializeField]
    private FloatSO level;
    [SerializeField]
    private TMP_Text countdown;
    [SerializeField]
    private GameEvent resetScene;

    void Start()
    {
        startTimer = false;
        SetTimer();
        currentTime = startingTime;
    }

    private void Update()
    {
        if (startTimer)
            StartTimer();
    }

    void StartTimer()
    {
        currentTime -= 1 * Time.deltaTime;
        countdown.text = currentTime.ToString("0");
        //print(currentTime);

        if (currentTime <= 0)
        {
            EnableLosePanel();
            //currentTime = 0;
            //losePanel.SetActive(true);
        }
    }

    void SetTimer() //Lower the time in the timer while level raising
    {
        if(level.Value < 10)
        {
            startingTime = 90;

        }else if(level.Value >= 10 && level.Value < 20)
        {
            startingTime = 60;
        }
        else if (level.Value >= 25 && level.Value < 30)
        {
            startingTime = 45;
        }
        else if (level.Value >= 30 && level.Value < 40)
        {
            startingTime = 30;
        }
        else if (level.Value >= 40 && level.Value <= 50)
        {
            startingTime = 20;
        }
    }

    public void EnableTimer() //Event to Enable Timer
    {
        startTimer = true;
    }

    public void EnableWinPanel()
    {
        winPanel.SetActive(true);
        startTimer = false;

        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableLosePanel()
    {
        losePanel.SetActive(true);
        startTimer = false;

        Cursor.lockState = CursorLockMode.None;
    }

    void ResetTimer()
    {
        startTimer = false;
        currentTime = 0;
    }

    public void ResetLevel()
    {
        StartCoroutine(TransitionPanel());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        //ResetTimer();
        //winPanel.SetActive(false);
        level.Value++;
        StartCoroutine(TransitionPanel());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //resetScene.Raise();
    }

    private IEnumerator TransitionPanel()
    {
        transitionPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        transitionPanel.SetActive(false);
    }
}
