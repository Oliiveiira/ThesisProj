using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //public TMP_Text scoreText;
    //[SerializeField]
    private FloatSO score;
    [SerializeField]
    private IntSO level;

    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Tutorial" || scene.name == "TutorialTP")
        {
            level.Value = 0;
        }else if (scene.name == "Level1" || scene.name == "Level1 - Static" || scene.name == "Level1 - StaticTP" || scene.name == "Level1TP")
        {
            level.Value = 1;
        }else if (scene.name == "Level2" || scene.name == "Level2 - Static" || scene.name == "Level2 - StaticTP" || scene.name == "Level2TP")
        {
            level.Value = 2;
        }
        else if (scene.name == "Level3" || scene.name == "Level3 - Static" || scene.name == "Level3 - StaticTP" || scene.name == "Level3TP")
        {
            level.Value = 3;
        }
        else if (scene.name == "Level4" || scene.name == "Level4 - Static" || scene.name == "Level4 - StaticTP" || scene.name == "Level4TP")
        {
            level.Value = 4;
        }
        else if (scene.name == "Level5" || scene.name == "Level5 - Static" || scene.name == "Level5 - StaticTP" || scene.name == "Level5TP")
        {
            level.Value = 5;
        }
        else if (scene.name == "MainMenu")
        {
            level.Value = 6;
        }
        //score.Value = 0;
        //scoreText.text = score.Value.ToString() + " Points";
    }

    //public void AddPoint()
    //{
    //    score.Value += 1;
    //    scoreText.text = score.Value.ToString() + " Points";
    //}

    public void ResetScore()
    {
        score.Value = 0;
    }

    public void NextLevel()
    {
        level.Value++;
        SceneManager.LoadScene(sceneName);
    }

    public void GoToSupermaket()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
