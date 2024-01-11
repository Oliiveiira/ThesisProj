using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class CustomPuzzleGameManager : PuzzleListReader
{
    //Puzzle Game Manager
    [SerializeField]
    private FloatSO mirror;
    [SerializeField]
    private GameEvent mirrorLeft;

    [SerializeField]
    private GameEvent mirrorRight;
    [SerializeField]
    private GameEvent setWinPanel;

    [SerializeField]
    private GameEvent mirrorNone;

    public List<Transform> placeholders;
    public List<Transform> puzzlePieces;

    public bool mirrorRightHand;
    public bool mirrorLeftHand;
    public bool mirrorNoneHand;
    public bool isPlaced;
    public bool setNormalMode;

    public Transform grid;
    public Transform puzzle;

    public Transform[] sidePlaceHolders;

    public AudioSource winSound;

    public string nextSceneName;
    [SerializeField]
    public bool gameHasStarted;
    //Timer to trigger next scene
    private float startingTime = 5;
    private float currentTime;
    [SerializeField]
    private TextMeshProUGUI timer;

    [SerializeField]
    private IntSO puzzleLevelSO;

    [SerializeField]
    private FloatSO difficultyValue;

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
        //set the difficultyValueSO to set the distance between the cubes and the grid placement 
        difficultyValue.Value = myPuzzleList.puzzlelevel[puzzleLevelSO.Value].difficultyValue;

        foreach (var placeholder in placeholders)
        {
            MeshRenderer placeholderMeshRenderer = placeholder.GetComponent<MeshRenderer>();
            placeholderMeshRenderer.enabled = true;
        }

        currentTime = startingTime;
        //foreach (var puzzlePiece in puzzlePieces)
        //{
        //    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
        //    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
        //    puzzlePiece.SetPositionAndRotation(new Vector3(puzzlePiece.position.x, puzzlePiece.position.y, Random.Range(-0.3f, -0.7f)), Quaternion.Euler(-90, 0, 0));
        //}
        ShuffleArray(sidePlaceHolders);
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
            difficultyValue.Value = 0.035f;
        }
       // SceneManager.LoadScene("CustomPuzzleLevel");
    }

    public void GoToAppMenu()
    {
        SceneManager.LoadScene("AppHub");
    }

    public void DeserializeJson()
    {
        string jsonFileName = "ImageLinks.txt";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);
        string jsonText = File.ReadAllText(jsonFilePath);
        myPuzzleList = JsonUtility.FromJson<PuzzleList>(jsonText);

    }

    //PuzzleGameManager
    void Update()
    {
        if (mirrorRightHand && !isPlaced)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                //grid.position = new Vector3(grid.position.x, grid.position.y, 0.1f);
                //puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0.1f);

                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    //for (int i =0; i < sidePlaceHolders.Length; i++)
                    //{
                    //    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(-90, 0, 0));
                    //}
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced = true;
                }
            }
        }
        else if (mirrorLeftHand && !isPlaced)
        {
            int i = 0;

            foreach (var puzzlePiece in puzzlePieces)
            {
                //  grid.position = new Vector3(grid.position.x, grid.position.y, -0.1f);
                // puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, -0.1f);
                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    //  puzzlePiece.SetPositionAndRotation(new Vector3(puzzlePiece.position.x, puzzlePiece.position.y, Random.Range(-0.3f, -0.7f)), Quaternion.Euler(-90, 0, 0));
                    //for (int i = 0; i < sidePlaceHolders.Length; i++)
                    //{
                    //    puzzlePiece.SetPositionAndRotation(new Vector3(sidePlaceHolders[i].position.x,sidePlaceHolders[i].position.y, -sidePlaceHolders[i].position.z), Quaternion.Euler(-90, 0, 0));
                    //}
                    puzzlePiece.SetPositionAndRotation(new Vector3(sidePlaceHolders[i].position.x, sidePlaceHolders[i].position.y, -sidePlaceHolders[i].position.z), Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced = true;
                }
            }
        }
        else if (mirrorNoneHand && !isPlaced)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                //grid.position = new Vector3(grid.position.x, grid.position.y, 0);
                //puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0);
                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced = true;
                }
            }
        }
        else if (!mirrorRightHand && !mirrorLeftHand && !mirrorNoneHand && !isPlaced)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                //grid.position = new Vector3(grid.position.x, grid.position.y, 0);
                //puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0);
                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced = true;
                }
            }
        }

        if (AreTransformPositionsEqual() && isPlaced)
        {
            Debug.Log("Boa!");
            setWinPanel.Raise();
            timer.SetText("Proximo nivel em " + currentTime.ToString("0"));
            currentTime -= 1 * Time.deltaTime;
            if (currentTime <= 0)
            {
                NextLevel();
            }
        }
    }

    bool AreTransformPositionsEqual()
    {
        if (placeholders.Count != puzzlePieces.Count)
        {
            return false; // Lists must have the same length to compare positions.
        }

        for (int i = 0; i < placeholders.Count; i++)
        {
            if (placeholders[i].position != puzzlePieces[i].position)
            {
                return false; // If any pair of positions is not equal, return false.
            }
        }

        return true; // All positions are equal.
    }

    public static void ShuffleArray<T>(T[] array)
    {
        Random random = new Random();

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            // Swap array[i] and array[j]
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public void MirrorLeft()
    {
        // mirror.Value = 1;
        mirrorLeft.Raise();
        mirrorLeftHand = true;
        mirrorRightHand = false;
        mirrorNoneHand = false;
    }

    public void MirrorRight()
    {
        // mirror.Value = 2;  
        mirrorRight.Raise();
        mirrorRightHand = true;
        mirrorLeftHand = false;
        mirrorNoneHand = false;
    }

    public void MirrorNone()
    {
        // mirror.Value = 2;  
        mirrorNone.Raise();
        mirrorNoneHand = true;
        mirrorLeftHand = false;
        mirrorRightHand = false;
    }

    public void StartGamePuzzleManager()
    {
        gameHasStarted = true;
    }

    public void NextLevelPuzzleManager()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
