using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using Unity.Netcode;

public class PuzzleGameManagerMultiplayer : NetworkBehaviour
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
    public NetworkVariable<bool> gameHasStarted;
    //Timer to trigger next scene
    private float startingTime = 5;
    private NetworkVariable<float> currentTime;
    [SerializeField]
    private TextMeshProUGUI timer;

    [SerializeField]
    private GameObject objectsToHide;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var placeholder in placeholders)
        {
            MeshRenderer placeholderMeshRenderer = placeholder.GetComponent<MeshRenderer>();
            placeholderMeshRenderer.enabled = true;
        }

        currentTime.Value = startingTime;
        //foreach (var puzzlePiece in puzzlePieces)
        //{
        //    PuzzlePiece puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePiece>();
        //    puzzlePieceTransform.rightPosition = puzzlePiece.transform.position;
        //    puzzlePiece.SetPositionAndRotation(new Vector3(puzzlePiece.position.x, puzzlePiece.position.y, Random.Range(-0.3f, -0.7f)), Quaternion.Euler(-90, 0, 0));
        //}
        ShuffleArray(sidePlaceHolders);
    }

    void Update()
    {
        if (mirrorRightHand && !isPlaced && IsServer)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                //grid.position = new Vector3(grid.position.x, grid.position.y, 0.1f);
                //puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0.1f);

                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
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
        else if (mirrorLeftHand && !isPlaced && IsServer)
        {
            int i = 0;

            foreach (var puzzlePiece in puzzlePieces)
            {
                //  grid.position = new Vector3(grid.position.x, grid.position.y, -0.1f);
                // puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, -0.1f);
                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
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
        else if (mirrorNoneHand && !isPlaced && IsServer)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                //grid.position = new Vector3(grid.position.x, grid.position.y, 0);
                //puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0);
                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced = true;
                }
            }
        }
        else if (!mirrorRightHand && !mirrorLeftHand && !mirrorNoneHand && !isPlaced && IsServer)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                //grid.position = new Vector3(grid.position.x, grid.position.y, 0);
                //puzzle.position = new Vector3(puzzle.position.x, puzzle.position.y, 0);
                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced = true;
                }
            }
        }

        if (AreTransformPositionsEqual() && isPlaced)
        {
            Debug.Log("Boa!");
            if (IsServer)
            {
                RaiseWinPanelClientRPC();
                currentTime.Value -= 1 * Time.deltaTime;
                if (currentTime.Value <= 0)
                {
                    NextLevelServerRPC();
                }
            }
            timer.SetText("Proximo nivel em " + currentTime.Value.ToString("0"));
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

    [ClientRpc]
    public void RaiseWinPanelClientRPC()
    {
        setWinPanel.Raise();
    }

    [ClientRpc]
    public void StartGameClientRPC()
    {
        objectsToHide.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRPC()
    {
        gameHasStarted.Value = true;
        objectsToHide.SetActive(true);
        StartGameClientRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextLevelServerRPC()
    {
        NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetLevelServerRPC()
    {
        NetworkManager.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
