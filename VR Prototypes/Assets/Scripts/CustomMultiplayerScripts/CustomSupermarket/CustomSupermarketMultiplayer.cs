using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSupermarketMultiplayer : CustomLevelsData
{
    //public TextAsset recipeJSON;
    public TextMeshProUGUI budgetText;
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI[] productsToGet;
    [SerializeField]
    private GameObject listPaper;
    public int arraySize = 3;
    public GameEvent startTimer;
    //private bool alreadyShowed;
    public int randomIndex;
    //To use in ScanProduct Script
    public int budget;

    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject leftButton;
    [SerializeField]
    private GameObject rightButton;

    //To control the Data Watch
    public TextMeshProUGUI budgettoWatchR;
    public TextMeshProUGUI[] productsToGettoWatchR;
    [SerializeField]
    private GameObject watchR;

    public TextMeshProUGUI budgettoWatchL;
    public TextMeshProUGUI[] productsToGettoWatchL;
    [SerializeField]
    private GameObject watchL;

    public bool mirrorLeft;
    public bool mirrorRight;

    //fillShelf
    public Transform[] placeHolders;
    public Transform[] leftplaceHolders;
    public Transform[] rightplaceHolders;

    public List<GameObject> allProducts;
    public List<GameObject> jsonPrefabs = new List<GameObject>();
    public GameObject placeHoldersObject;

    private void Start()
    {
        allProducts = new List<GameObject>(Resources.LoadAll<GameObject>("NetworkProducts/"));

        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        if (!IsServer)
            return;
        StartCoroutine(InstantiatePrefabsAfterDelay());
        ShowRecipe();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowRecipe();
        }
    }

    public void ShowRecipe()
    {
        budgettoWatchL.text = myLevelData.levelData[0].recipeName;
        budgettoWatchR.text = myLevelData.levelData[0].recipeName;

        for (int i = 0; i < myLevelData.levelData[0].ingredientsName.Count; i++)
        {
            productsToGet[i].text = myLevelData.levelData[0].ingredientsName[i];
            productsToGettoWatchR[i].text = myLevelData.levelData[0].ingredientsName[i]; //to Watch
            productsToGettoWatchL[i].text = myLevelData.levelData[0].ingredientsName[i]; //to Watch
        }
        SetListClientRPC();
    }

    [ClientRpc]
    public void SetListClientRPC()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);
        
        budgettoWatchL.text = myLevelData.levelData[0].recipeName;
        budgettoWatchR.text = myLevelData.levelData[0].recipeName;

        for (int i = 0; i < myLevelData.levelData[0].ingredientsName.Count; i++)
        {
            productsToGet[i].text = myLevelData.levelData[0].ingredientsName[i];
            productsToGettoWatchR[i].text = myLevelData.levelData[0].ingredientsName[i]; //to Watch
            productsToGettoWatchL[i].text = myLevelData.levelData[0].ingredientsName[i]; //to Watch
        }
    }

    private IEnumerator InstantiatePrefabsAfterDelay()
    {
        yield return new WaitForSeconds(3);

        InstantiatePrefabs();
        // Allow interaction after the delay
    }

    private void LoadPrefabsFromJSON()
    {
        List<string> ingredientPaths = myLevelData.levelData[0].ingredientsPath;

        for (int i = 0; i < ingredientPaths.Count; i++)
        {
            string path = $"NetworkProducts/{myLevelData.levelData[0].ingredientsPath[i].Split("/")[1]}";
            GameObject ingredientToCatch = Resources.Load<GameObject>(path);
            jsonPrefabs.Add(ingredientToCatch);
        }
    }

    public void InstantiatePrefabs()
    {
        LoadPrefabsFromJSON();
        allProducts.RemoveAll(item => jsonPrefabs.Contains(item)); //Remove all products that are common in both lists to avoid duplicated items

        List<int> usedIndices = new List<int>(); // Store the indices that have been used
        List<int> lastJsonPrefabIndex = new List<int>();

        for (int i = 1; i < jsonPrefabs.Count; i += 2)
        {
            Random.seed = System.DateTime.Now.Millisecond;

            int randomIndex = Random.Range(0, leftplaceHolders.Length);
            while (lastJsonPrefabIndex.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, leftplaceHolders.Length);
            }
            lastJsonPrefabIndex.Add(randomIndex);
            // Instantiate(jsonPrefabs[i], placeHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
            GameObject oddInstantiatedPrefab = Instantiate(jsonPrefabs[i], leftplaceHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
            oddInstantiatedPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(1, destroyWithScene: true);
            //oddInstantiatedPrefab.transform.parent = placeHoldersObject.transform;
            usedIndices.Add(randomIndex); // Mark the index as used
        }

        for (int i = 0; i < jsonPrefabs.Count; i += 2)
        {
            Random.seed = System.DateTime.Now.Millisecond;

            int randomIndex = Random.Range(0, leftplaceHolders.Length);
            while (lastJsonPrefabIndex.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, leftplaceHolders.Length);
            }
            lastJsonPrefabIndex.Add(randomIndex);
            GameObject evenInstantiatedPrefab = Instantiate(jsonPrefabs[i], rightplaceHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
            evenInstantiatedPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(2, destroyWithScene: true);
            //evenInstantiatedPrefab.transform.parent = placeHoldersObject.transform;
            usedIndices.Add(randomIndex + leftplaceHolders.Length); // Mark the index as used
        }

        List<int> lastIndex = new List<int>();
        for (int j = 0; j < placeHolders.Length; j++)
        {
            if (!usedIndices.Contains(j))
            {
                Random.seed = System.DateTime.Now.Millisecond;

                int randomIndex = Random.Range(0, allProducts.Count - 1);
                while (lastIndex.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, allProducts.Count - 1);
                }
                lastIndex.Add(randomIndex);
                // Instantiate(allProducts[randomIndex], placeHolders[j].position, allProducts[randomIndex].transform.rotation);
                GameObject InstantiatedPrefab = Instantiate(allProducts[randomIndex], placeHolders[j].position, allProducts[randomIndex].transform.rotation);
                if (j < leftplaceHolders.Length)
                {
                    InstantiatedPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(1, destroyWithScene: true);
                }
                else
                {
                    InstantiatedPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(2, destroyWithScene: true);
                }

                //InstantiatedPrefab.transform.parent = placeHoldersObject.transform;
            }

        }
    }

    [ServerRpc]
    public void DebugServerRpc(string debug)
    {
        Debug.Log(debug);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene(string sceneName)
    {
        NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
