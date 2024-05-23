using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using Unity.Netcode;
using System.IO;

public class CustomPuzzleManagerMultiplayer : CustomLevelsData
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
    public NetworkVariable<bool> isPlaced;
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
    [SerializeField]
    private NetworkVariable<float> currentTime = new NetworkVariable<float>(0);
    [SerializeField]
    private TextMeshProUGUI timer;

    public bool listPositionRemoved = false; 
    [SerializeField]
    private GameObject objectsToHide;

    private CustomMultiplayerSceneManager multiplayerSceneManager;
    private GetErrorCountData errorCountData;
    public int numberOfErrors = 0;

    // Start is called before the first frame update
    void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        foreach (var placeholder in placeholders)
        {
            MeshRenderer placeholderMeshRenderer = placeholder.GetComponent<MeshRenderer>();
            placeholderMeshRenderer.enabled = true;
        }
        
        multiplayerSceneManager = GetComponent<CustomMultiplayerSceneManager>();
        errorCountData = GetComponent<GetErrorCountData>();

        if (IsServer)
            currentTime.Value = startingTime;
        
        ShuffleArray(sidePlaceHolders);
    }

    void Update()
    {
        if (mirrorRightHand && !isPlaced.Value && IsServer)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced.Value = true;
                }
            }
        }
        else if (mirrorLeftHand && !isPlaced.Value && IsServer)
        {
            int i = 0;

            foreach (var puzzlePiece in puzzlePieces)
            {
                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(new Vector3(sidePlaceHolders[i].position.x, sidePlaceHolders[i].position.y, -sidePlaceHolders[i].position.z), Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced.Value = true;
                }
            }
        }
        else if (mirrorNoneHand && !isPlaced.Value && IsServer)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced.Value = true;
                }
            }
        }
        else if (!mirrorRightHand && !mirrorLeftHand && !mirrorNoneHand && !isPlaced.Value && IsServer)
        {
            int i = 0;
            foreach (var puzzlePiece in puzzlePieces)
            {
                if (gameHasStarted.Value)
                {
                    PuzzlePieceMultiplayer puzzlePieceTransform = puzzlePiece.GetComponent<PuzzlePieceMultiplayer>();
                    puzzlePieceTransform.rightPosition.Value = puzzlePiece.transform.position;
                    puzzlePiece.SetPositionAndRotation(sidePlaceHolders[i].position, Quaternion.Euler(0, -90, 90));
                    i++;
                    isPlaced.Value = true;
                }
            }
        }

        if (AreTransformPositionsEqual() && isPlaced.Value)
        {
            Debug.Log("Boa!");
            if (IsServer)
            {
                if(numberOfErrors == 0)
                {
                    errorCountData.SaveData(numberOfErrors);
                    numberOfErrors++;
                }

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
        foreach (Transform pieceTransform in puzzlePieces)
        {
            PuzzlePieceMultiplayer piece = pieceTransform.gameObject.GetComponent<PuzzlePieceMultiplayer>();
            if (!piece.isInRightPlace.Value)
                return false;
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
        //string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        //string jsonText = File.ReadAllText(jsonFilePath);
        //myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);
        //if (!listPositionRemoved)
        //{
        //    RemoveDataAtIndex(0);
        //    listPositionRemoved = true;
        //}
        //if (myLevelData.levelData.Count > 0)
        //    nextSceneName = myLevelData.levelData[0].level;
        //else
        //    nextSceneName = "CustomMultiplayerHub";

        //NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        multiplayerSceneManager.NextLevelServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetLevelServerRPC()
    {
        NetworkManager.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
