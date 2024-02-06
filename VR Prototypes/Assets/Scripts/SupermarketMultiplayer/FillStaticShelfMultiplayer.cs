using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;

public class FillStaticShelfMultiplayer : StaticLevelsListReader
{
    public Transform[] placeHolders;

    public List<GameObject> allProducts;
    public List<GameObject> jsonPrefabs = new List<GameObject>();
    public GameObject placeHoldersObject;

    [SerializeField]
    private IntSO level;

    // Start is called before the first frame update
    void Awake()
    {
        allProducts = new List<GameObject>(Resources.LoadAll<GameObject>("NetworkProducts/"));
        mystaticLevelsLists = JsonUtility.FromJson<StaticLevelsList>(recipeJSON.text);
        //  myCustomRecipeList = JsonUtility.FromJson<CustomRecipeList>(customRecipeJSON.text);
    }

    private void Start()
    {
        if (IsServer)
            InstantiatePrefabs();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(myProductLists.recipes[0].ingredientsPath);
            //  LoadPrefabsFromJSON();
            InstantiatePrefabs();
        }
    }

    private void LoadPrefabsFromJSON()
    {
        List<string> ingredientPaths = mystaticLevelsLists.recipe[0].ingredientsPath;

        for (int i = 0; i < ingredientPaths.Count; i++)
        {
            string path = $"NetworkProducts/{mystaticLevelsLists.recipe[0].ingredientsPath[i].Split("/")[1]}";
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

        for (int i = 0; i < jsonPrefabs.Count; i++)
        {
            Random.seed = System.DateTime.Now.Millisecond;

            int randomIndex = Random.Range(0, placeHolders.Length);
            while (lastJsonPrefabIndex.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, placeHolders.Length);
            }
            lastJsonPrefabIndex.Add(randomIndex);
            // Instantiate(jsonPrefabs[i], placeHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
            GameObject InstantiatedPrefab = Instantiate(jsonPrefabs[i], placeHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
            InstantiatedPrefab.GetComponent<NetworkObject>().Spawn();
            InstantiatedPrefab.transform.parent = placeHoldersObject.transform;
            usedIndices.Add(randomIndex); // Mark the index as used
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
                InstantiatedPrefab.GetComponent<NetworkObject>().Spawn();
                InstantiatedPrefab.transform.parent = placeHoldersObject.transform;
            }

        }
    }
}
