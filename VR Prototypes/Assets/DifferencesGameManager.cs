using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.SceneManagement;

public class DifferencesGameManager : MonoBehaviour
{
    // Static instance of the DifferencesGameManager
    public static DifferencesGameManager Instance;

    public int numberOfDifferences;
    public List<int> spottedDifferences;
    public string nextSceneName;

    [SerializeField]
    private FloatSO mirror;
    [SerializeField]
    private GameEvent mirrorLeft;
    [SerializeField]
    private GameEvent mirrorRight;
    [SerializeField]
    private GameEvent mirrorNone;

    [SerializeField]
    private GameEvent setWinPanel;

    private void Awake()
    {
        // Ensure only one instance of DifferencesGameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Destroy the duplicate instance
            Destroy(gameObject);
        }
    }

    public void Win()
    {
        if(spottedDifferences.Count == numberOfDifferences)
        {
            setWinPanel.Raise();
            Debug.Log("Win!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Win();
    }

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

    public void NextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
