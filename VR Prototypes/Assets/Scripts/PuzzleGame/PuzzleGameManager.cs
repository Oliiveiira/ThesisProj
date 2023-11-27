using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        foreach (var placeholder in placeholders)
        {
            MeshRenderer placeholderMeshRenderer = placeholder.GetComponent<MeshRenderer>();
            placeholderMeshRenderer.enabled = true;
        }

        //foreach (var puzzlePiece in puzzlePieces)
        //{
        //    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
        //    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
        //    puzzlePiece.SetPositionAndRotation(new Vector3(puzzlePiece.position.x, puzzlePiece.position.y, Random.Range(-0.3f, -0.7f)), Quaternion.Euler(-90, 0, 0));
        //}
    }

    void Update()
    {
        if (mirrorRightHand && !isPlaced)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                grid.position = new Vector3(grid.position.x, grid.position.y, 0.1f);
                puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0.1f);

                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    //for (int i =0; i < sidePlaceHolders.Length; i++)
                    //{
                    //    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(-90, 0, 0));
                    //}
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(-90, 0, 0));
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
                grid.position = new Vector3(grid.position.x, grid.position.y, -0.1f);
                puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, -0.1f);
                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    //  puzzlePiece.SetPositionAndRotation(new Vector3(puzzlePiece.position.x, puzzlePiece.position.y, Random.Range(-0.3f, -0.7f)), Quaternion.Euler(-90, 0, 0));
                    //for (int i = 0; i < sidePlaceHolders.Length; i++)
                    //{
                    //    puzzlePiece.SetPositionAndRotation(new Vector3(sidePlaceHolders[i].position.x,sidePlaceHolders[i].position.y, -sidePlaceHolders[i].position.z), Quaternion.Euler(-90, 0, 0));
                    //}
                    puzzlePiece.SetPositionAndRotation(new Vector3(sidePlaceHolders[i].position.x, sidePlaceHolders[i].position.y, -sidePlaceHolders[i].position.z), Quaternion.Euler(-90, 0, 0));
                    i++;
                    isPlaced = true;
                }
            }
        }
        else if(mirrorNoneHand && !isPlaced)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                grid.position = new Vector3(grid.position.x, grid.position.y, 0);
                puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0);
                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(-90, 0, 0));
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
                grid.position = new Vector3(grid.position.x, grid.position.y, 0);
                puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0);
                if (gameHasStarted)
                {
                    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
                    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(-90, 0, 0));
                    i++;
                    isPlaced = true;
                }
            }
        }

        if (AreTransformPositionsEqual() && isPlaced)
        {
            Debug.Log("Boa!");
            setWinPanel.Raise();
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

    public void StartGame()
    {
        gameHasStarted = true;
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
