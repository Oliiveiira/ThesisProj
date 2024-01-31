using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PairsGameMultiplayerManager : PairsListReader
{
    public List<GameObject> pairsObjects;
    public List<GameObject> jsonPrefabs = new List<GameObject>();
    [SerializeField]
    private List<Transform> leftPlaceholders;
    [SerializeField]
    private List<Transform> rightPlaceholders;
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

        LoadPrefabsFromJSON();
        //AssignPlaceholders();
    }

    private void LoadPrefabsFromJSON()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject pairsToInstantiate = Resources.Load<GameObject>(myPairsList.pairlevel[i].pairPath);
            //guardar indexes que serão utilizados para dar load dos vários pares, para posteriormente escolher cada um recorrentemente
            usedIndexes.Add(i);
            jsonPrefabs.Add(pairsToInstantiate);
        }
    }

    private void ChoosePair()
    {
        //dar assign do material do par a ser feito
        // Choose a random index from the remaining prefabs list
        if(jsonPrefabs.Count > 0 && !pickPair)
        {
            randomIndex = Random.Range(0, jsonPrefabs.Count);

            gridPositions[0].name = leftPieces[randomIndex].transform.name;
            gridPositions[1].name = rightPieces[randomIndex].transform.name;

            //jsonPrefabs.RemoveAt(randomIndex);
            pickPair = true;
        }
    }

    private void ComparePositions()
    {
        if(Vector3.Distance(leftPieces[randomIndex].transform.position, gridPositions[0].transform.position) < 0.02f)
        {
            leftPieces[randomIndex].transform.position = gridPositions[0].transform.position;
            isInLeftPlace = true;
        }
        else if(Vector3.Distance(rightPieces[randomIndex].transform.position, gridPositions[1].transform.position) < 0.02f)
        {
            rightPieces[randomIndex].transform.position = gridPositions[1].transform.position;
            isInRightPlace = true;
        }
        else if(isInLeftPlace && isInRightPlace)
        {
            pickPair = false;
        }

    }

    private void AssignPlaceholders()
    {
        foreach(GameObject piece in jsonPrefabs)
        {
            GameObject leftPiece = Instantiate(piece.transform.GetChild(0).gameObject, leftPlaceholders[0].position, Quaternion.Euler(0, -90, 90));
            leftPiece.transform.parent = piecesParent.transform;
            leftPiece.transform.localScale = scale;
            leftPiece.name = piece.transform.GetChild(0).name;
            leftPieces.Add(leftPiece);

            GameObject rightPiece = Instantiate(piece.transform.GetChild(1).gameObject, rightPlaceholders[0].position, Quaternion.Euler(0, -90, 90));
            rightPiece.transform.parent = piecesParent.transform;
            rightPiece.transform.localScale = scale;
            rightPiece.name = piece.transform.GetChild(1).name;
            rightPieces.Add(rightPiece);
        }
        //GameObject piece1 = Instantiate(jsonPrefabs[0].transform.GetChild(0).gameObject, leftPlaceholders[0].position, Quaternion.Euler(0,-90,90));
        //piece1.transform.parent = piecesParent.transform;
        //piece1.transform.localScale = scale;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AssignPlaceholders();
            ChoosePair();
            ComparePositions();
        }
    }
}
