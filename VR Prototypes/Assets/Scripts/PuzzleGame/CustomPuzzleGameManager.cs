using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPuzzleGameManager : PuzzleListReader
{
    [SerializeField]
    private IntSO puzzleLevelSO;

   // public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        string jsonFileName = "ImageLinks.txt";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        // Check if the file exists in the persistent data path
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            myPuzzleList = JsonUtility.FromJson<PuzzleList>(jsonText);
            Debug.Log("File Exists");
        }
        else
        {
            // Handle the case when the file doesn't exist in the persistent data path
            Debug.LogError("JSON file not found in the persistent data path.");
        }
    }

    public void StartGame()
    {
        puzzleLevelSO.Value = 0;
        if (myPuzzleList.puzzlelevel[0].numberOfCubes == 4)
        {
            SceneManager.LoadScene("PuzzleLevelNetWork4Cubes");
        }
        else if(myPuzzleList.puzzlelevel[0].numberOfCubes == 9)
        {
            SceneManager.LoadScene("PuzzleLevelNetWork9Cubes");
        }
        else if(myPuzzleList.puzzlelevel[0].numberOfCubes == 16)
        {
            SceneManager.LoadScene("PuzzleLevelNetWork16Cubes");
        }
        else if (myPuzzleList.puzzlelevel[0].numberOfCubes == 6)
        {
            SceneManager.LoadScene("PuzzleLevelNetWork6Cubes");
        }
        else if (myPuzzleList.puzzlelevel[0].numberOfCubes == 12)
        {
            SceneManager.LoadScene("PuzzleLevelNetWork12Cubes");
        }
    }

    public void NextLevel()
    {
        //To implement more custom Levels in the future
         puzzleLevelSO.Value++;
        // Check if the puzzle level is within the bounds of the list
        if (puzzleLevelSO.Value < myPuzzleList.puzzlelevel.Count)
        {
            if (myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes == 4)
            {
                SceneManager.LoadScene("PuzzleLevelNetWork4Cubes");
            }
            else if (myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes == 9)
            {
                SceneManager.LoadScene("PuzzleLevelNetWork9Cubes");
            }
            else if (myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes == 16)
            {
                SceneManager.LoadScene("PuzzleLevelNetWork16Cubes");
            }
            else if (myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes == 6)
            {
                SceneManager.LoadScene("PuzzleLevelNetWork6Cubes");
            }
            else if (myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes == 12)
            {
                SceneManager.LoadScene("PuzzleLevelNetWork12Cubes");
            }
        }
        else
        {
            SceneManager.LoadScene("PuzzleMainMenu");
        }
       // SceneManager.LoadScene("CustomPuzzleLevel");
    }

    public void GoToAppMenu()
    {
        SceneManager.LoadScene("AppHub");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
