using Oculus.Interaction.HandGrab;
using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

public class PairsGameManager : PairsListReader
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
    private bool pickPair;
    [SerializeField]
    private int randomIndex;
    [SerializeField]
    private List<GameObject> leftPieces;
    [SerializeField]
    private List<GameObject> rightPieces;
    public bool isInLeftPlace;
    public bool isInRightPlace;
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
        // Choose a random index from the remaining prefabs list
        if (jsonPrefabs.Count > 0 && !pickPair)
        {
            Debug.Log(jsonPrefabs.Count - 1);
            Debug.Log("choosePair");
            randomIndex = random.Next(0, jsonPrefabs.Count);
            Debug.Log(randomIndex);
            gridPositions[0].name = leftPieces[jsonPrefabs.Count - 1].transform.name;
            gridPositions[1].name = rightPieces[jsonPrefabs.Count - 1].transform.name;
            SetPairToMakeMaterial();

            //jsonPrefabs.RemoveAt(randomIndex);
            pickPair = true;
        }
        else
        {
            Debug.Log("Winner!");
            setWinPanel.Raise();
        }
    }

    public void SetPairToMakeMaterial()
    {
        Renderer leftRendererMaterials = leftPieces[jsonPrefabs.Count - 1].GetComponent<Renderer>();
        leftPieceMaterial.materials = leftRendererMaterials.materials;
        Renderer rightRendererMaterials = rightPieces[jsonPrefabs.Count - 1].GetComponent<Renderer>();
        rightPieceMaterial.materials = rightRendererMaterials.materials;
    }

    private void ComparePositions()
    {
        if (Vector3.Distance(leftPieces[jsonPrefabs.Count - 1].transform.position, gridPositions[0].transform.position) < 0.02f && gridPositions[0].name == leftPieces[jsonPrefabs.Count - 1].transform.name && !isInLeftPlace)
        {
            Debug.Log("leftPiece");
            leftPieces[jsonPrefabs.Count - 1].transform.position = gridPositions[0].transform.position;
            popSound.Play();
            HandGrabInteractable handGrabInteractable = leftPieces[jsonPrefabs.Count - 1].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }

            isInLeftPlace = true;
        }
        if (Vector3.Distance(rightPieces[jsonPrefabs.Count - 1].transform.position, gridPositions[1].transform.position) < 0.02f && gridPositions[1].name == rightPieces[jsonPrefabs.Count - 1].transform.name && !isInRightPlace)
        {
            Debug.Log("rightPiece");
            rightPieces[jsonPrefabs.Count - 1].transform.position = gridPositions[1].transform.position;
            popSound.Play();
            HandGrabInteractable handGrabInteractable = rightPieces[jsonPrefabs.Count - 1].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }

            isInRightPlace = true;
        }
        if (isInLeftPlace && isInRightPlace && pickPair)
        {
            isInLeftPlace = false;
            isInRightPlace = false;
            Debug.Log("Entrou aqui");
            StartCoroutine(DeactivatePair(jsonPrefabs.Count - 1));
            jsonPrefabs.RemoveAt(jsonPrefabs.Count - 1);
            pickPair = false;
            ChoosePair();
        }
    }

    private void AssignPlaceholders()
    {
        ShuffleArray(leftPlaceholders);
        ShuffleArray(rightPlaceholders);

        int i = 0;
        foreach (GameObject piece in jsonPrefabs)
        {
            GameObject leftPiece = Instantiate(piece.transform.GetChild(0).gameObject, leftPlaceholders[i].position, Quaternion.Euler(0, -90, 90));
            leftPiece.transform.parent = piecesParent.transform;
            leftPiece.transform.localScale = scale;
            leftPiece.name = piece.transform.GetChild(0).name;
            leftPieces.Add(leftPiece);

            GameObject rightPiece = Instantiate(piece.transform.GetChild(1).gameObject, rightPlaceholders[i].position, Quaternion.Euler(0, -90, 90));
            rightPiece.transform.parent = piecesParent.transform;
            rightPiece.transform.localScale = scale;
            rightPiece.name = piece.transform.GetChild(1).name;
            rightPieces.Add(rightPiece);
            i++;
        }
    }

    IEnumerator DeactivatePair(int lastRandomIndex)
    {
        yield return new WaitForSeconds(2);
        leftPieces[lastRandomIndex].SetActive(false);
        rightPieces[lastRandomIndex].SetActive(false);
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


    // Update is called once per frame
    void Update()
    {

        ComparePositions();
    }
}
