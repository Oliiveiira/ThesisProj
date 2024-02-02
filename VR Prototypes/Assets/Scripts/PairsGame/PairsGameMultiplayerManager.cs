using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;
using RoboRyanTron.Unite2017.Events;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PairsGameMultiplayerManager : PairsListReader
{
    public List<GameObject> pairsObjects;
    public List<GameObject> jsonPrefabs = new List<GameObject>();
    [SerializeField]
    private Transform[] leftPlaceholders;
    [SerializeField]
    private Transform[] rightPlaceholders;
    [SerializeField]
    private GameObject piecesParent;
    public Vector3 scale = new Vector3(10f, 10f, 10f); // Example scale values
    [SerializeField]
    private List<GameObject> gridPositions;
    [SerializeField]
    private List<int> usedIndexes;
    [SerializeField]
    private NetworkVariable<bool> pickPair;
    [SerializeField]
    private List<GameObject> leftPieces;
    [SerializeField]
    private List<GameObject> rightPieces;
    public NetworkVariable<bool> isInLeftPlace;
    public NetworkVariable<bool> isInRightPlace;
    private AudioSource popSound;
    [SerializeField]
    private Renderer leftPieceMaterial;
    [SerializeField]
    private Renderer rightPieceMaterial;
    private static Random random = new Random();
    [SerializeField]
    private GameEvent setWinPanel;
    [SerializeField]
    private List<int> usedPieceIndexes;
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private Transform ObjectsToHide;

    private void Awake()
    {
        //// Load 5 random GameObjects
        //for (int i = 0; i < 5; i++)
        //{
        //    // Generate a random index
        //    int randomIndex = Random.Range(1, 6); // Assuming you have Object1 to Object5

        //    // Load the random GameObject from the Resources folder
        //    pairsObjects = new List<GameObject>(Resources.LoadAll<GameObject>("Pairs/"));
        //}
    }

   

    // Start is called before the first frame update
    void Start()
    {
        string jsonFileName = "PairsList.txt";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        // Check if the file exists in the persistent data path
        string jsonText = File.ReadAllText(jsonFilePath);
        myPairsList = JsonUtility.FromJson<PairsList>(jsonText);

        popSound = GetComponent<AudioSource>();

        LoadPrefabsFromJSON();
        if(IsServer)
            AssignPlaceholders();
        ChoosePair();
        //AssignPlaceholders();
    }

    private void LoadPrefabsFromJSON()
    {
        for (int i = 0; i < leftPlaceholders.Length; i++)
        {
            // Get the count of pair paths for the current index
            int pairPathCount = myPairsList.pairlevel.Count;
            int randomPairIndex = random.Next(0, pairPathCount);

            do
            {
                randomPairIndex = random.Next(0, pairPathCount);
            } while (usedIndexes.Contains(randomPairIndex)); // Check if the index has been used before

            GameObject pairsToInstantiate = Resources.Load<GameObject>(myPairsList.pairlevel[randomPairIndex].pairPath);
            //guardar indexes que serão utilizados para dar load dos vários pares, para posteriormente escolher cada um recorrentemente
           
            usedIndexes.Add(randomPairIndex);
            jsonPrefabs.Add(pairsToInstantiate);
        }
    }

    private void ChoosePair()
    {
        if (!IsServer)
            return;

        // Choose a random index from the remaining prefabs list
        if(jsonPrefabs.Count > 0 && !pickPair.Value)
        {
            Debug.Log(jsonPrefabs.Count - 1);
            Debug.Log("choosePair");
            //randomIndex = random.Next(0, jsonPrefabs.Count);
            //Debug.Log(randomIndex);
            gridPositions[0].name = leftPieces[jsonPrefabs.Count - 1].transform.name + "l";
            gridPositions[1].name = rightPieces[jsonPrefabs.Count - 1].transform.name + "l";
            SetPairToMakeMaterialClientRpc(leftPieces[jsonPrefabs.Count - 1].transform.name, rightPieces[jsonPrefabs.Count - 1].transform.name);

            //jsonPrefabs.RemoveAt(randomIndex);
            pickPair.Value = true;
        }
        else
        {
            Debug.Log("Winner!");
            // setWinPanel.Raise();
            RaiseWinPanelClientRPC();
        }
    }


    [ClientRpc]
    public void SetPairToMakeMaterialClientRpc(string leftPieceName, string rightPieceName)
    {
        Renderer leftRendererMaterials = GameObject.Find(leftPieceName).GetComponent<Renderer>();
        leftPieceMaterial.materials = leftRendererMaterials.sharedMaterials;
        Renderer rightRendererMaterials = GameObject.Find(rightPieceName).GetComponent<Renderer>();
        rightPieceMaterial.materials = rightRendererMaterials.sharedMaterials;
        Debug.Log(leftRendererMaterials.sharedMaterials);
        Debug.Log(rightPieceName);
    }

    private void ComparePositions()
    {
        if(Vector3.Distance(leftPieces[jsonPrefabs.Count - 1].transform.position, gridPositions[0].transform.position) < 0.02f/* && gridPositions[0].name == leftPieces[jsonPrefabs.Count - 1].transform.name*/ && !isInLeftPlace.Value)
        {
            Debug.Log("leftPiece");
            leftPieces[jsonPrefabs.Count - 1].transform.position = gridPositions[0].transform.position;
            popSound.Play();
            HandGrabInteractable handGrabInteractable = leftPieces[jsonPrefabs.Count - 1].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }

            isInLeftPlace.Value = true;
        }
        if(Vector3.Distance(rightPieces[jsonPrefabs.Count - 1].transform.position, gridPositions[1].transform.position) < 0.02f/* && gridPositions[1].name == rightPieces[jsonPrefabs.Count - 1].transform.name */&& !isInRightPlace.Value)
        {
            Debug.Log("rightPiece");
            rightPieces[jsonPrefabs.Count - 1].transform.position = gridPositions[1].transform.position;
            popSound.Play();
            HandGrabInteractable handGrabInteractable = rightPieces[jsonPrefabs.Count - 1].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }

            isInRightPlace.Value = true;
        }
        if(isInLeftPlace.Value && isInRightPlace.Value && pickPair.Value)
        {
            isInLeftPlace.Value = false;
            isInRightPlace.Value = false;
            Debug.Log("Entrou aqui");
            StartCoroutine(DeactivatePair(jsonPrefabs.Count - 1));
            jsonPrefabs.RemoveAt(jsonPrefabs.Count - 1);
            pickPair.Value = false;
            ChoosePair();
        }
    }

    
    private void AssignPlaceholders()
    {
        ShuffleArray(leftPlaceholders);
        ShuffleArray(rightPlaceholders);

        int i = 0;
        foreach(GameObject piece in jsonPrefabs)
        {
            GameObject pair = Instantiate(piece.transform.gameObject, leftPlaceholders[i].position, Quaternion.Euler(0, 0, 90));
            pair.transform.localScale = leftPieceMaterial.transform.parent.lossyScale;
            NetworkObject pairNetwork = pair.GetComponent<NetworkObject>();
            pairNetwork.Spawn();
            GameObject leftPiece = pair.transform.GetChild(0).gameObject;
            //leftPiece.name = piece.transform.GetChild(0).name;
            leftPiece.transform.position = leftPlaceholders[i].position;
            //leftPiece.transform.localScale = leftPieceMaterial.transform.lossyScale * (10 * 0.65f);
            leftPieces.Add(leftPiece);
            Debug.Log(leftPlaceholders[i].position);
            GameObject rightPiece = pair.transform.GetChild(1).gameObject;
            //rightPiece.name = piece.transform.GetChild(1).name;
            rightPiece.transform.position = rightPlaceholders[i].position;
           // rightPiece.transform.localScale = leftPieceMaterial.transform.localScale;
            rightPieces.Add(rightPiece);
            //// pairNetwork.TrySetParent(ObjectsToHide);
            i++;
        }
    }

    IEnumerator DeactivatePair(int lastRandomIndex)
    {
        yield return new WaitForSeconds(2);
        leftPieces[lastRandomIndex].SetActive(false);
        rightPieces[lastRandomIndex].SetActive(false);
        Destroy(leftPieces[lastRandomIndex].transform.parent.gameObject);
    }

    [ClientRpc]
    public void RaiseWinPanelClientRPC()
    {
        setWinPanel.Raise();
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

    [ClientRpc]
    private void DisableInteractableClientRpc()
    {
        popSound.Play();
        HandGrabInteractable handGrabInteractable = rightPieces[jsonPrefabs.Count - 1].GetComponent<HandGrabInteractable>();
        if (handGrabInteractable != null)
        {
            handGrabInteractable.enabled = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextLevelServerRPC()
    {
        NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
            ComparePositions();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetClientOwnershipServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
            Debug.Log("Client is now the owner");
        }
        else
        {
            Debug.Log("not working");
        }
    }

}
