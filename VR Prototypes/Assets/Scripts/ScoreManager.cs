using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    [SerializeField]
    private FloatSO score;

    // Start is called before the first frame update
    void Start()
    {
        score.Value = 0;
        scoreText.text = score.Value.ToString() + " Points";
    }

    public void AddPoint()
    {
        score.Value += 1;
        scoreText.text = score.Value.ToString() + " Points";
    }

    public void ResetScore()
    {
        score.Value = 0;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
